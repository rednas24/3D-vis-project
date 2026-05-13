using UnityEngine;

public class LiftPlatformDetector : MonoBehaviour
{
    public PressurePlateLift pressurePlateLift;

    private Transform originalParent;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement3D player =
            collision.gameObject.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        if (player.playerType == PlayerMovement3D.PlayerType.Player2)
        {
            pressurePlateLift.SetShroomOnPlatform(true);

            // Store original parent
            originalParent = collision.transform.parent;

            // Parent player to moving platform
            collision.transform.SetParent(
                pressurePlateLift.liftPlatform,
                true
            );
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerMovement3D player =
            collision.gameObject.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        if (player.playerType == PlayerMovement3D.PlayerType.Player2)
        {
            pressurePlateLift.SetShroomOnPlatform(false);

            // Restore original parent
            collision.transform.SetParent(
                originalParent,
                true
            );
        }
    }
}