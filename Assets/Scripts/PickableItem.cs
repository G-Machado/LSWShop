using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    private Interactables _interactor;
    public ScriptableItem item;

    // Start is called before the first frame update
    void Start()
    {
        _interactor = GetComponent<Interactables>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_interactor.interacting)
        {
            InventoryManager.Instance.inventoryItems.Add(item);

            Destroy(this.gameObject);
        }
    }
}
