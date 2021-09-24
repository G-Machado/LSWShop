using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    [Header("Interaction Variables")]
    private Animator _anim;
    public bool selected = false;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerManager pManager = collision.gameObject.GetComponent<PlayerManager>();

            if (pManager.closestInteractable != null)
            {
                float interactableDistance = Vector2.Distance(pManager.transform.position, pManager.closestInteractable.transform.position);
                float myDistance = Vector2.Distance(pManager.transform.position, transform.position);

                if (myDistance <= interactableDistance)
                {
                    pManager.closestInteractable = this;
                    selected = true;

                    _anim.SetBool("selected", true);
                }
                else
                {
                    selected = false;
                    _anim.SetBool("selected", false);
                }
            }
            else
            {
                pManager.closestInteractable = this;
                selected = true;

                _anim.SetBool("selected", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerManager pManager = collision.gameObject.GetComponent<PlayerManager>();

            if (pManager.closestInteractable == this)
            {
                pManager.disableInteraction();
                selected = false;

                _anim.SetBool("selected", false);
            }
        }
    }
}
