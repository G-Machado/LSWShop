using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptables/Item")]
public class ScriptableItem : ScriptableObject
{
    [Header("Display Variables")]
    public Sprite shopIcon;
    public Sprite playerWear;
    public string itemName;

    [Header("Equip Variables")]
    public GameObject wearableObject;
    [System.Serializable]
    public enum equipType
    {
        HEAD,
        BODY,
        HAND
    }
    public equipType equip;
    public bool equipped = false;

    [Header("Shop Variables")]
    public int buyValue;
    public int sellValue;

    [Header("Pick Variables")]
    public GameObject pickableInstance;
}
