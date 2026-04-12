using UnityEngine;

[RequireComponent(typeof(PlayerMovement3D))]
public class PlayerPickup : MonoBehaviour
{
    private PlayerMovement3D playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement3D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only cactus (Player1)
        if (playerMovement.playerType != PlayerMovement3D.PlayerType.Player1)
            return;

        Key key = other.GetComponent<Key>();

        if (key != null)
        {
            key.Pickup(gameObject);
        }
    }
}