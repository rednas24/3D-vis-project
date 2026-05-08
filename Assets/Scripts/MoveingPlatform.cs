using UnityEngine;

public class LiftPlatformDetector : MonoBehaviour
{
    public PressurePlateLift pressurePlateLift;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement3D player =
            collision.gameObject.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        if (player.playerType == PlayerMovement3D.PlayerType.Player2)
        {
            pressurePlateLift.SetShroomOnPlatform(true);

            collision.transform.SetParent(transform);
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

            collision.transform.SetParent(null);
        }
    }
}