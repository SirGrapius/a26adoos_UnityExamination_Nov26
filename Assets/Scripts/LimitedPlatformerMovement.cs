using System;
using UnityEngine;
using UnityEngine.InputSystem;

            /////////////// INFORMATION ///////////////
// SETUP:
// Add the script to the object you want the player to control

// TIPS:
// - The Rigidbody2D component should (probably) have some constraints: Freeze Rotation Z
// - Adjust the CapsuleCollider2D to fit the object appropriately
// - Objects that should be used as platforms needs to have a collider (non-trigger)

// LIMITATIONS:
// This script enables very limited platformer movement, but with minimal setup required.
// The available movement consists of: a jump with a fixed height (based on jumpForce), and moving side to side (based on maxSpeed).
// It will NOT easily enable you to add animations or sound or similar effects.

// This script automatically adds a PlayerInput, Rigidbody2D and a CapsuleCollider2D component in the inspector.
[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class LimitedPlatformerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int groundLayer;
    
    private Vector2 moveInput;
    private bool jumpInput; 
    public bool inDialogue;
    public bool hasGem;
    [SerializeField] private NPC currentNPC;
    
    private Rigidbody2D rb;
    private SpriteRenderer renderer;
    private Animator anim;
    [SerializeField] private CapsuleCollider2D groundCheckCollider; 

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        groundCheckCollider = GetComponent<CapsuleCollider2D>();
    }
    
    void Update()
    {
        // Apply horizontal velocity from input
        if (!inDialogue)
        {
            rb.linearVelocityX = moveInput.x * maxSpeed;
        }
        
        if (Keyboard.current[Key.E].wasPressedThisFrame && !inDialogue && currentNPC != null)
        {
            currentNPC.StartDialogue();
        }

        if (rb.linearVelocityX != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        
        anim.SetBool("Grounded", IsGrounded());

        if (IsGrounded())
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }

        if (rb.linearVelocity.y < 0)
        {
            anim.SetBool("Falling", true);
        }
        
        // Apply vertical velocity from jump
        if (jumpInput && IsGrounded())
        {
            rb.linearVelocityY = jumpForce;
            anim.SetBool("Jumping", true);
        }
        jumpInput = false; // only jump once for each press of the jump-button
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocityX < 0)
        {
            renderer.flipX = true;
        }
        else if (rb.linearVelocityX > 0)
        {
            renderer.flipX = false;
        }
    }

    private bool IsGrounded()
    {
        // Is our groundCheckCollider touching the groundLayer? If so, return the value "true"
        if (groundCheckCollider.IsTouchingLayers(~0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
    
    public void OnJump(InputValue input)
    {
        jumpInput = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.gameObject.GetComponent<NPC>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null;
        }
    }
}
