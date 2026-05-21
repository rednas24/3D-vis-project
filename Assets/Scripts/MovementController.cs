using UnityEngine;
using UnityEngine.InputSystem;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerMovement3D : MonoBehaviour,
    PlayerInputSystem.IPlayer1Actions,
    PlayerInputSystem.IPlayer2Actions
{
    public enum PlayerType { Player1, Player2 }
    public PlayerType playerType;
    

    public float moveSpeed = 6f;
    public float jumpForce = 6f;
    public float bounceForce = 10f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip stompSound;

    private PlayerInputSystem input;
    private Rigidbody rb;
    private PlayerAnimationController anim;

    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isJumping;

    public bool IsMovingInput => moveInput.magnitude > 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<PlayerAnimationController>();

        input = new PlayerInputSystem();

        if (PlayerPrefs.HasKey("InputBindings"))
        {
            input.asset.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString("InputBindings"));
        }

        if (playerType == PlayerType.Player1)
            input.Player1.AddCallbacks(this);
        else
            input.Player2.AddCallbacks(this);

            Debug.Log(
            "Attack binding: " +
            input.Player1.Attack.bindings[0].effectivePath);

        if (PlayerPrefs.HasKey("InputBindings"))
        {
            string json = PlayerPrefs.GetString("InputBindings");

            Debug.Log("Loading bindings: " + json);

            input.asset.LoadBindingOverridesFromJson(json);
        }
    }

        public void ReloadBindings()
    {
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            input.asset.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString("InputBindings"));

            Debug.Log(
                "Attack binding now: " +
                input.Player1.Attack.bindings[0].effectivePath);
        }
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
        // Movement
Vector3 currentVelocity = rb.linearVelocity;

        Vector3 targetVelocity = new Vector3(
            -moveInput.y * moveSpeed,
            currentVelocity.y,
            moveInput.x * moveSpeed
        );

        // Smooth velocity instead of hard overwrite
        rb.linearVelocity = Vector3.Lerp(
            currentVelocity,
            targetVelocity,
            10f * Time.fixedDeltaTime
        );

        // Stop unwanted spinning
        rb.angularVelocity = Vector3.zero;

        // Rotation
        RotateCharacter();

        // Jump
        if (jumpRequested && IsGrounded() && !isJumping)
        {
            isJumping = true;

            anim.PlayJump();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpRequested = false;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            groundCheckDistance + 0.1f,
            groundLayer
        );

        if (grounded)
            isJumping = false;

        return grounded;
    }

    private void RotateCharacter()
    {
        Vector3 moveDirection = new Vector3(-moveInput.y, 0f, moveInput.x);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.fixedDeltaTime
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement3D otherPlayer = collision.gameObject.GetComponent<PlayerMovement3D>();

        if (otherPlayer == null) return;

        // Only cactus (Player1) stomps
        if (playerType != PlayerType.Player1) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                if (audioSource != null && stompSound != null)
                {
                    audioSource.PlayOneShot(stompSound);
                }

                Bounce();
                otherPlayer.OnStomped();
                break;
            }
        }
    }

    private void Bounce()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

        anim.PlayJump();
    }

    public void OnStomped()
    {
        anim.PlayDizzy();
    }

    public void PerformPunchHit()
    {
        // Only cactus (Player1) can destroy
        if (playerType != PlayerType.Player1) return;

        float radius = 1.5f;
        Vector3 center = transform.position + transform.forward * 1f;

        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Destroy"))
            {
                WeaponsAndPropsAssetPack_NAS.Scripts.Breakable breakable =
                    hit.GetComponent<WeaponsAndPropsAssetPack_NAS.Scripts.Breakable>();

                if (breakable != null)
                {
                    breakable.TriggerBreak();
                    Debug.Log("Broke: " + hit.name);
                }
            }
        }
    }
      
        // ===== Player 1 =====
    public void OnWASD(InputAction.CallbackContext context)
    {
        if (playerType != PlayerType.Player1) return;
        moveInput = context.ReadValue<Vector2>();
    }

    // ===== Player 2 =====
    public void OnArrowsMovement(InputAction.CallbackContext context)
    {
        if (playerType != PlayerType.Player2) return;
        moveInput = context.ReadValue<Vector2>();
    }

    // ===== Shared =====
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        jumpRequested = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        anim.PlayAttack();
        Debug.Log(playerType + " Attack");
    }

    public void OnInteract(InputAction.CallbackContext context)
{
    if (!context.performed) return;

    float range = 2f;
    anim.PlayAttack();
    Vector3 center = transform.position + transform.forward * 1f;

    Collider[] hits = Physics.OverlapSphere(center, range);

    foreach (Collider hit in hits)
    {
        Interactable interactable = hit.GetComponent<Interactable>();

        if (interactable != null)
        {
            interactable.Interact(this);
            break;
        }
    }
}
}