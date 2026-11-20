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
    
    private Vector2 moveInput;
    private bool jumpInput;
    
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D groundCheckCollider; 

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        groundCheckCollider = GetComponent<CapsuleCollider2D>();
    }
    
    void Update()
    {
        // Apply horizontal velocity from input
        rb.linearVelocityX = moveInput.x * maxSpeed;

        if (rb.linearVelocity.x != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
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

    private bool IsGrounded()
    {
        // Is our groundCheckCollider touching the groundLayer? If so, return the value "true"
        if (groundCheckCollider.IsTouchingLayers(~0)) // ~0 is referring to EVERY layer.
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("Jumping", false);
            return true;
        }
        else
        {
            anim.SetBool("Grounded", false);
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
}
