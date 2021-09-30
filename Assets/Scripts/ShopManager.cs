using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Variables")]
    public GameObject shopPrefab;
    public Vector3 offset;
    public bool shopping = false;

    [Header("UI Variables")]
    public Transform canvas;
    public List<Image> iconHolders = new List<Image>();
    public Text itemTitle;
    public Text itemCost;
    public Button buyButton;
    public Text walletText;

    [Header("Item Variables")]
    public List<ScriptableItem> availableItems = new List<ScriptableItem>();
    public ScriptableItem currentDisplayedItem;

    private GameObject currentShop;

    private Interactables interactor;

    private void Start()
    {
        interactor = GetComponent<Interactables>();

        for (int i = 0; i < availableItems.Count; i++)
        {
            availableItems[i].equipped = false;
        }

        SpawnShop();
    }

    private void FixedUpdate()
    {
        if(interactor.interacting && !shopping)
        {
            OpenShop();
        }

        if(!interactor.interacting && shopping)
        {
            CloseShop();
        }

        if (interactor.interacting)
            SetupShop();
    }

    public void OpenShop()
    {
        SetupShop();
        currentShop.GetComponent<Animator>().SetBool("activated", true);
        shopping = true;

        itemTitle.text = "No item selected - use mouse";

        itemCost.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        walletText.gameObject.SetActive(false);

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void SpawnShop()
    {
        if (currentShop == null)
        {
            GameObject shopClone = Instantiate(shopPrefab, transform.position + offset, transform.rotation, canvas);
            currentShop = shopClone;
        }

        Image[] shopIcons = currentShop.GetComponentsInChildren<Image>();
        for (int i = 0; i < shopIcons.Length; i++)
        {
            if (shopIcons[i].gameObject.CompareTag("shop_icon"))
            {
                iconHolders.Add(shopIcons[i].transform.GetComponentInChildren<Image>());
            }
        }

        Text[] inventoryTexts = currentShop.GetComponentsInChildren<Text>();

        itemTitle = inventoryTexts[4];
        itemCost = inventoryTexts[5];
        itemCost.gameObject.SetActive(false);
        walletText = inventoryTexts[7];
        walletText.gameObject.SetActive(false);

        buyButton = currentShop.GetComponentsInChildren<Button>()[4];
        buyButton.gameObject.SetActive(false);
        buyButton.onClick.AddListener(() => BuyItem());

        SetupShop();
    }

    public bool HasItem(ScriptableItem item, List<ScriptableItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(item))
                return true;
        }

        return false;
    }

    public void SetupShop()
    {
        List<ScriptableItem> setupList = new List<ScriptableItem>();
        for (int i = 0; i < availableItems.Count; i++)
        {
            setupList.Add(availableItems[i]);
        }

        for (int i = 0; i < iconHolders.Count; i++)
        {
            if (i < availableItems.Count && setupList.Count > 0)
            {
                int itemIndex = 0;
                Text iconCounText = iconHolders[i].transform.parent.GetComponentInChildren<Text>();
                int itemCount = 0;

                ScriptableItem currentItem = setupList[itemIndex];

                while (HasItem(currentItem, setupList))
                {
                    itemCount++;
                    setupList.Remove(currentItem);
                }

                if (itemCount < 1)
                    iconCounText.text = "";
                else
                    iconCounText.text = itemCount.ToString();

                iconHolders[i].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
                    DisplayItem(currentItem)
                );

                iconHolders[i].enabled = true;
                iconHolders[i].sprite = currentItem.shopIcon;
            }
            else
            {
                Text iconCounText = iconHolders[i].transform.parent.GetComponentInChildren<Text>();
                iconCounText.text = "";

                iconHolders[i].enabled = false;
            }
        }

        walletText.text = WalletManager.Instance.wallet.Value.ToString();
    }

    public void CloseShop()
    {
        currentShop.GetComponent<Animator>().SetBool("activated", false);

        shopping = false;
    }

    public void DisplayItem(ScriptableItem item)
    {
        currentDisplayedItem = item;

        itemTitle.text = item.itemName;
        itemCost.text = "Cost: " + item.buyValue;
        itemCost.gameObject.SetActive(true);

        walletText.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(true);
    }

    public void BuyItem()
    {
        if (currentDisplayedItem == null) return;

        if(WalletManager.Instance.HasAmmount(currentDisplayedItem.buyValue))
        {
            WalletManager.Instance.ReduceAmmount(currentDisplayedItem.buyValue);
           
            InventoryManager.Instance.inventoryItems.Add(currentDisplayedItem);
            InventoryManager.Instance.SetupDropInventory();

            availableItems.Remove(currentDisplayedItem);
            currentDisplayedItem = null;

            /// Resets Shop selection
            itemTitle.text = "No item selected - use mouse";
            itemCost.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            walletText.gameObject.SetActive(false);

            SetupShop();
        }
    }

}
