using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement3D player =
            other.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        CharacterController cc =
            other.GetComponent<CharacterController>();

        // Teleport player
        other.transform.position =
            teleportTarget.position;

        Debug.Log(player.playerType + " teleported");
    }
}