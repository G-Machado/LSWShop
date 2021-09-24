using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
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
    public Text walletText;

    [Header("Item Variables")]
    public List<ScriptableItem> inventoryItems = new List<ScriptableItem>();

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

        inventoryInstance.transform.position = Vector3.Lerp(inventoryInstance.transform.position, 
            transform.position + offset, .3f);
    }

    public void OpenInventory()
    {
        SetupInventory();
        inventoryInstance.GetComponent<Animator>().SetBool("activated", true);
        equipping = true;

        itemTitle.text = "No item selected - use mouse";

        itemCost.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        walletText.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);

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

        dropButton = inventoryButtons[5];
        dropButton.gameObject.SetActive(false);

        SetupInventory();
    }

    public void SetupInventory()
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
    }

    public void CloseInventory()
    {
        inventoryInstance.GetComponent<Animator>().SetBool("activated", false);

        equipping = false;
    }

    public void DisplayItem(ScriptableItem item)
    {
        itemTitle.text = item.itemName;
        itemCost.text = "Cost: " + item.buyValue;
        itemCost.gameObject.SetActive(true);

        walletText.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(true);
        dropButton.gameObject.SetActive(true);
    }
}
