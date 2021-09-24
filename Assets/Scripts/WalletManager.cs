using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance;
    private void Awake()
    {
        if (WalletManager.Instance == null)
            WalletManager.Instance = this;
    }

    public IntVariable wallet;
    public Text walletUIText;

    private void Start()
    {
        UpdateUI();
    }

    public bool HasAmmount(int value)
    {
        if (wallet.Value >= value)
            return false;

        return false;
    }

    public void ReduceAmmount(int value)
    {
        wallet.Value -= value;

        UpdateUI();
    }

    public void AddAmmount(int value)
    {
        wallet.Value += value;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if(walletUIText)
            walletUIText.text = wallet.Value.ToString();
    }
}
