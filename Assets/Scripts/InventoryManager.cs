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
    public Vector3 offset;
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

    private GameObject inventoryInstance;

    private PlayerManager player;

    private void Start()
    {
        player = GetComponent<PlayerManager>();
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

        inventoryInstance.transform.position = Vector3.Lerp(inventoryInstance.transform.position, 
            transform.position + offset, .3f);
    }

    public void OpenInventory()
    {
        inventoryInstance.GetComponent<Animator>().SetBool("activated", true);
        equipping = true;

        itemTitle.text = "No item selected - use mouse";

        itemCost.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        walletText.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void SpawnInventory()
    {
        if (inventoryInstance == null)
        {
            GameObject shopClone = Instantiate(shopPrefab, transform.position + offset, transform.rotation, canvas);
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
        Debug.Log("inventory buttons lenght " + inventoryButtons.Length);

        itemTitle = inventoryInstance.GetComponentsInChildren<Text>()[0];
        itemCost = inventoryInstance.GetComponentsInChildren<Text>()[1];
        itemCost.gameObject.SetActive(false);
        walletText = inventoryInstance.GetComponentsInChildren<Text>()[2];
        walletText.gameObject.SetActive(false);

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
        Debug.Log("setting up sell inventory");

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
    }

    public void SellItem()
    {
        if (!currentDisplayedItem) return;

        /// Resets Shop selection
        itemTitle.text = "No item selected - use mouse";
        itemCost.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);
        walletText.gameObject.SetActive(false);

        ShopManager shop = player.closestInteractable.gameObject.GetComponent<ShopManager>();
        shop.availableItems.Add(currentDisplayedItem);
        shop.SetupShop();

        inventoryItems.Remove(currentDisplayedItem);

        WalletManager.Instance.AddAmmount(currentDisplayedItem.sellValue);
        walletText.text = WalletManager.Instance.wallet.Value.ToString();

        currentDisplayedItem = null;

        Debug.Log("ITEM SOLD!!");
    }

    public void DropItem()
    {
        Debug.Log("ITEM DROPPED!!");
    }

    public void EquipItem()
    {
        Debug.Log("ITEM EQUIPPED!!");
    }
}
