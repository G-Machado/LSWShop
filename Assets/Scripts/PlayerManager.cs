using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    /// Graphics Variables
    private Animator _anim;
    private SpriteRenderer _renderer;

    [Header("Movement Variables")]
    public float movSpeed = 1;
    [Range(0, 1)]
    public float movDamp = .3f;
    private Rigidbody2D _rb;

    [Header("Interaction Variables")]
    public Interactables closestInteractable;
    public bool interacting = false;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();

        if (closestInteractable != null && Input.GetAxis("Interact") > 0)
            triggerInteractable();

    }

    private void MovePlayer()
    {
        Vector2 finalInput = new Vector2(0, 0);

        finalInput.x = Input.GetAxis("Horizontal");
        finalInput.y = Input.GetAxis("Vertical");

        _rb.velocity = Vector2.Lerp(_rb.velocity, finalInput * movSpeed, 1 - movDamp);
        _anim.SetFloat("movSpeed", _rb.velocity.magnitude);

        if (_rb.velocity.x > 0)
            _renderer.flipX = false;
        else
            _renderer.flipX = true;
    }

    private void triggerInteractable()
    {
        Debug.Log("Interacting with " + closestInteractable.name);
        interacting = true;
    }

    public void disableInteraction()
    {
        Debug.Log("Disabling interaction");
        interacting = false;
        closestInteractable = null;
    }
}