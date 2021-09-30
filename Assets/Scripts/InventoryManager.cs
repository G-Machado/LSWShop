using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private void Awake()
    {
        if (InventoryManager.Instance == null)
            InventoryManager.Instance = this;
    }

    [Header("Shop Variables")]
    public GameObject shopPrefab;
    public Vector3 normalOffset;
    public Vector3 shopOffset;
    public bool equipping = false;

    [Header("UI Variables")]
    public Transform canvas;
    public List<Image> iconHolders = new List<Image>();
    public Text itemTitle;
    public Text itemCost;
    public Button equipButton;
    public Button dropButton;
    public Button sellButton;
    public Text walletText;

    [Header("Item Variables")]
    public List<ScriptableItem> inventoryItems = new List<ScriptableItem>();
    public ScriptableItem currentDisplayedItem;

    [Header("Equip Variables")]
    public EquipablesManager equipManager;

    private GameObject inventoryInstance;

    private PlayerManager player;

    private void Start()
    {
        player = GetComponent<PlayerManager>();
        equipManager = GetComponent<EquipablesManager>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].equipped = false;
        }

        SpawnInventory();
    }

    private void FixedUpdate()
    {
        if (player.inventoring && !equipping)
        {
            OpenInventory();
        }

        if (!player.inventoring && equipping)
        {
            CloseInventory();
        }

        if (player.closestInteractable)
        {
            if (player.closestInteractable.gameObject.CompareTag("frog_shop"))
                SetupSellInventory();
        }
        else
            SetupDropInventory();

        Vector3 finalOffset = normalOffset;
        if (player.closestInteractable != null && player.closestInteractable.CompareTag("frog_shop") && player.interacting)
            finalOffset = shopOffset;

        inventoryInstance.transform.position = Vector3.Lerp(inventoryInstance.transform.position, 
            transform.position + finalOffset, .2f);
    }

    public void OpenInventory()
    {
        inventoryInstance.GetComponent<Animator>().SetBool("activated", true);
        equipping = true;

        itemTitle.text = "No item selected - use mouse";

        itemCost.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void SpawnInventory()
    {
        if (inventoryInstance == null)
        {
            GameObject shopClone = Instantiate(shopPrefab, transform.position + normalOffset, transform.rotation, canvas);
            inventoryInstance = shopClone;
        }

        Image[] shopIcons = inventoryInstance.GetComponentsInChildren<Image>();
        for (int i = 0; i < shopIcons.Length; i++)
        {
            if (shopIcons[i].gameObject.CompareTag("shop_icon"))
            {
                iconHolders.Add(shopIcons[i].transform.GetComponentInChildren<Image>());
            }
        }

        Button[] inventoryButtons = inventoryInstance.GetComponentsInChildren<Button>();

        itemTitle = inventoryInstance.GetComponentsInChildren<Text>()[0];
        itemCost = inventoryInstance.GetComponentsInChildren<Text>()[1];
        itemCost.gameObject.SetActive(false);
        walletText = inventoryInstance.GetComponentsInChildren<Text>()[2];

        equipButton = inventoryButtons[4];
        equipButton.gameObject.SetActive(false);
        equipButton.onClick.AddListener(() => EquipItem());

        dropButton = inventoryButtons[5];
        dropButton.gameObject.SetActive(false);
        dropButton.onClick.AddListener(() => DropItem());

        sellButton = inventoryButtons[6];
        sellButton.gameObject.SetActive(false);
        sellButton.onClick.AddListener(() => SellItem());

        SetupDropInventory();
    }

    public void SetupDropInventory()
    {
        for (int i = 0; i < iconHolders.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                int itemIndex = i;
                iconHolders[i].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
                    DisplayItem(inventoryItems[itemIndex])
                );

                iconHolders[i].enabled = true;
                iconHolders[i].sprite = inventoryItems[i].shopIcon;
            }
            else
            {
                iconHolders[i].enabled = false;
            }
        }

        if (currentDisplayedItem != null)
        {
            dropButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(false);
        }

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void SetupSellInventory()
    {
        for (int i = 0; i < iconHolders.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                int itemIndex = i;
                iconHolders[i].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
                    DisplayItem(inventoryItems[itemIndex])
                );

                iconHolders[i].enabled = true;
                iconHolders[i].sprite = inventoryItems[i].shopIcon;
            }
            else
            {
                iconHolders[i].enabled = false;
            }
        }

        if (currentDisplayedItem != null)
        {
            dropButton.gameObject.SetActive(false);
            sellButton.gameObject.SetActive(true);
        }

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void CloseInventory()
    {
        currentDisplayedItem = null;

        inventoryInstance.GetComponent<Animator>().SetBool("activated", false);

        equipping = false;
    }

    public void DisplayItem(ScriptableItem item)
    {
        currentDisplayedItem = item;

        itemTitle.text = item.itemName;
        itemCost.text = "Sell value: " + item.sellValue;
        itemCost.gameObject.SetActive(true);

        walletText.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);

        if (currentDisplayedItem.equipped)
            equipButton.transform.GetChild(0).GetComponent<Text>().text = "UNEQUIP";
        else
            equipButton.transform.GetChild(0).GetComponent<Text>().text = "EQUIP";

    }

    public void SellItem()
    {
        if (!currentDisplayedItem) return;

       
        if(currentDisplayedItem.equipped)
        {
            equipManager.UnEquipItem(currentDisplayedItem);
            equipButton.transform.GetChild(0).GetComponent<Text>().text = "EQUIP";
        }

        ShopManager shop = player.closestInteractable.gameObject.GetComponent<ShopManager>();
        shop.availableItems.Add(currentDisplayedItem);
        shop.SetupShop();

        inventoryItems.Remove(currentDisplayedItem);

        WalletManager.Instance.AddAmmount(currentDisplayedItem.sellValue);
        walletText.text = WalletManager.Instance.wallet.Value.ToString();

        /// Resets Shop selection
        ResetSelection();
        //Debug.Log("ITEM SOLD!!");
    }

    public void ResetSelection()
    {
        itemTitle.text = "No item selected - use mouse";
        itemCost.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);

        currentDisplayedItem = null;
    }

    public void DropItem()
    {
        if (currentDisplayedItem.equipped)
            equipManager.UnEquipItem(currentDisplayedItem);

        GameObject pickableClone = Instantiate(currentDisplayedItem.pickableInstance, transform.position, transform.rotation);

        inventoryItems.Remove(currentDisplayedItem);
        ResetSelection();

        Debug.Log("ITEM DROPPED!!");
    }

    public void EquipItem()
    {
        if (!currentDisplayedItem.equipped)
        {
            equipManager.EquipItem(currentDisplayedItem);
            equipButton.transform.GetChild(0).GetComponent<Text>().text = "UNEQUIP";

            TriggerInventoryButton(currentDisplayedItem);
        }
        else
        {
            equipManager.UnEquipItem(currentDisplayedItem);
            equipButton.transform.GetChild(0).GetComponent<Text>().text = "EQUIP";

            TriggerInventoryButton(null);
        }

        //Debug.Log("ITEM EQUIPPED!!");
    }

    private void TriggerInventoryButton(ScriptableItem item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] == item && inventoryItems[i].equipped)
                iconHolders[i].transform.parent.GetComponent<Animator>().SetBool("equipped", true);
            else if(!inventoryItems[i].equipped)
                iconHolders[i].transform.parent.GetComponent<Animator>().SetBool("equipped", false);
        }
    }

}
