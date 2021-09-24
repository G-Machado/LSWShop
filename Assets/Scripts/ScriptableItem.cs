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

    [Header("Shop Variables")]
    public int buyValue;
    public int sellValue;
}
