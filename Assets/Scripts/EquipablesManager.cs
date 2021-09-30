using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipablesManager : MonoBehaviour
{
    [System.Serializable]
    public class equipableType
    {
        public ScriptableItem.equipType type;
        public ScriptableItem item;
        public Transform pivot;
        public SpriteRenderer renderer;
    }
    public List<equipableType> equipables = new List<equipableType>();

    private PlayerManager _player;

    private void Start()
    {
        _player = GetComponent<PlayerManager>();
    }
    private void FixedUpdate()
    {
        UpdateEquipedItems();
    }

    public void UpdateEquipedItems()
    {
        for (int i = 0; i < equipables.Count; i++)
        {
            if (!equipables[i].item || !equipables[i].item.equipped) continue;

            if (_player.renderer.flipX && equipables[i].type == ScriptableItem.equipType.HAND)
                equipables[i].renderer.sortingOrder = 0;
            else
            {
                //Debug.Log("in front" + equipables[i].type);

                if (equipables[i].renderer.sortingOrder >= 0)
                    equipables[i].renderer.sortingOrder = 2;
                
                equipables[i].renderer.flipX = _player.renderer.flipX;
            }
        }

    }

    public void EquipItem(ScriptableItem item)
    {
        for (int i = 0; i < equipables.Count; i++)
        {
            if(equipables[i].type == item.equip)
            {
                if(equipables[i].item != null)
                    UnEquipItem(equipables[i].item);

                equipables[i].item = item;
                item.equipped = true;

                GameObject itemClone = Instantiate(item.wearableObject, equipables[i].pivot.position, transform.rotation, equipables[i].pivot);
                equipables[i].renderer = itemClone.transform.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    public void UnEquipItem(ScriptableItem item)
    {
        for (int i = 0; i < equipables.Count; i++)
        {
            if (equipables[i].type == item.equip)
            {
                Destroy(equipables[i].renderer.transform.parent.gameObject);

                equipables[i].item = null;
                equipables[i].renderer = null;
                item.equipped = false;
            }
        }
    }
}
