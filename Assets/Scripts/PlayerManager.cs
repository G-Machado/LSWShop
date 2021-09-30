using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    /// Graphics Variables
    private Animator _anim;
    public SpriteRenderer renderer;

    [Header("Movement Variables")]
    public float movSpeed = 1;
    [Range(0, 1)]
    public float movDamp = .3f;
    private Rigidbody2D _rb;

    [Header("Interaction Variables")]
    public Interactables closestInteractable;
    public bool interacting = false;
    private bool interactDown = false;

    [Header("Inventory Variables")]
    public bool inventoring = false;
    private bool inventoryDown = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();

        if (closestInteractable != null && Input.GetKey(KeyCode.E) && !interactDown)
        {
            interactDown = true;
            triggerInteractable();
        }

        if (!Input.GetKey(KeyCode.E))
            interactDown = false;

        if(Input.GetKey(KeyCode.I) && !inventoryDown)
        {
            inventoryDown = true;

            if (!inventoring)
                triggerInventory();
            else
                disableInventory();
        }

        if (!Input.GetKey(KeyCode.I))
            inventoryDown = false;

    }

    private void MovePlayer()
    {
        Vector2 finalInput = new Vector2(0, 0);

        finalInput.x = Input.GetAxis("Horizontal");
        finalInput.y = Input.GetAxis("Vertical");

        _rb.velocity = Vector2.Lerp(_rb.velocity, finalInput * movSpeed, 1 - movDamp);
        _anim.SetFloat("movSpeed", _rb.velocity.magnitude);

        if (_rb.velocity.x > 0)
        {
            renderer.flipX = false;
        }
        else
        {
            renderer.flipX = true;
        }
    }

    private void triggerInteractable()
    {
        Debug.Log("Interacting with " + closestInteractable.name);

        closestInteractable.SetInteracting(!closestInteractable.interacting);

        interacting = closestInteractable.interacting;
    }

    public void disableInteraction()
    {
        Debug.Log("Disabling interaction");

        closestInteractable.SetInteracting(false);

        interacting = false;
        closestInteractable = null;
    }

    private void triggerInventory()
    {
        inventoring = true;
    }

    private void disableInventory()
    {
        inventoring = false;
    }
}
