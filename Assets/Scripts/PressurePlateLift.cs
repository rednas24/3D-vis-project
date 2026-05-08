using UnityEngine;

public class PressurePlateLift : MonoBehaviour
{
    [Header("Platform To Move")]
    public Transform liftPlatform;

    [Header("Particle Effect")]
    public ParticleSystem liftParticles;

    [Header("Movement")]
    public Vector3 raisedOffset = new Vector3(0, 5f, 0);
    public float moveSpeed = 2f;

    [Header("Activation Delay")]
    public float moveDelay = 1f;

    private float activationTimer = 0f;

    private Vector3 startPosition;
    private Vector3 raisedPosition;

    private bool activated = false;

    // Is Player2/Shroom standing on lift?
    private bool shroomOnPlatform = false;

    private void Start()
    {
        if (liftPlatform != null)
        {
            startPosition = liftPlatform.position;
            raisedPosition = startPosition + raisedOffset;
        }

        if (liftParticles != null)
        {
            liftParticles.Stop();
        }
    }

    private void Update()
    {
        if (liftPlatform == null) return;

        // Desired active state
        bool wantsToMove = activated && shroomOnPlatform;

        // Smooth timer
        if (wantsToMove)
        {
            activationTimer += Time.deltaTime;
        }
        else
        {
            activationTimer -= Time.deltaTime;
        }

        // Clamp timer
        activationTimer = Mathf.Clamp(
            activationTimer,
            0f,
            moveDelay
        );

        // Move only when timer fully charged
        bool shouldMove = activationTimer >= moveDelay;

        Vector3 targetPos =
            shouldMove ? raisedPosition : startPosition;

        liftPlatform.position = Vector3.Lerp(
            liftPlatform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Particles only depend on pressure plate
        if (liftParticles != null)
        {
            if (activated && !liftParticles.isPlaying)
            {
                liftParticles.Play();
            }
            else if (!activated && liftParticles.isPlaying)
            {
                liftParticles.Stop();
            }
        }
    }

    // Pressure plate activation (Player1/Cactus)
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement3D player =
            other.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        if (player.playerType == PlayerMovement3D.PlayerType.Player1)
        {
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement3D player =
            other.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        if (player.playerType == PlayerMovement3D.PlayerType.Player1)
        {
            activated = false;
        }
    }

    // Detect Player2/Shroom standing on lift
    public void SetShroomOnPlatform(bool value)
    {
        shroomOnPlatform = value;
    }
}