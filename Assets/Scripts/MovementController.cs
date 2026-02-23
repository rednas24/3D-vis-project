using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement3D : MonoBehaviour,
    PlayerInputSystem.IPlayer1Actions,
    PlayerInputSystem.IPlayer2Actions
{
    public enum PlayerType { Player1, Player2 }
    public PlayerType playerType;

    public float moveSpeed = 6f;
    public float jumpForce = 6f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    private PlayerInputSystem input;
    private Rigidbody rb;

    private Vector2 moveInput;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerInputSystem();

        if (playerType == PlayerType.Player1)
            input.Player1.AddCallbacks(this);
        else
            input.Player2.AddCallbacks(this);
    }

    private void OnEnable()
    {
        if (playerType == PlayerType.Player1)
            input.Player1.Enable();
        else
            input.Player2.Enable();
    }

    private void OnDisable()
    {
        if (playerType == PlayerType.Player1)
            input.Player1.Disable();
        else
            input.Player2.Disable();
    }

    private void FixedUpdate()
    {
        // Horizontal movement
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        velocity.z = moveInput.y * moveSpeed;
        rb.linearVelocity = velocity;

        // Jump (physics-safe)
        if (jumpRequested && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpRequested = false;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            groundCheckDistance + 0.1f,
            groundLayer
        );
    }

    // ===== Player 1 =====
    public void OnWASD(InputAction.CallbackContext context)
    {
        if (playerType != PlayerType.Player1) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        jumpRequested = true;
    }

    // ===== Player 2 =====
    public void OnArrowsMovement(InputAction.CallbackContext context)
    {
        if (playerType != PlayerType.Player2) return;
        moveInput = context.ReadValue<Vector2>();
    }
}