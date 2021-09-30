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

        itemTitle = currentShop.GetComponentsInChildren<Text>()[0];
        itemCost = currentShop.GetComponentsInChildren<Text>()[1];
        itemCost.gameObject.SetActive(false);
        walletText = currentShop.GetComponentsInChildren<Text>()[2];
        walletText.gameObject.SetActive(false);

        buyButton = currentShop.GetComponentsInChildren<Button>()[4];
        buyButton.gameObject.SetActive(false);
        buyButton.onClick.AddListener(() => BuyItem());

        SetupShop();
    }

    public void SetupShop()
    {
        for (int i = 0; i < iconHolders.Count; i++)
        {
            if (i < availableItems.Count)
            {
                int itemIndex = i;
                iconHolders[i].transform.parent.GetComponent<Button>().onClick.AddListener(() =>
                    DisplayItem(availableItems[itemIndex])
                );

                iconHolders[i].enabled = true;
                iconHolders[i].sprite = availableItems[i].shopIcon;
            }
            else
            {
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
