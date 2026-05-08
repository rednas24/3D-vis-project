using UnityEngine;

[RequireComponent(typeof(PlayerMovement3D))]
public class PlayerPickup : MonoBehaviour
{
    private PlayerMovement3D playerMovement;
    public Key carriedKey;

    public bool hasKey = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement3D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only cactus (Player1)
        if (playerMovement.playerType != PlayerMovement3D.PlayerType.Player1)
            return;

        // Already carrying a key
        if (hasKey)
            return;

        Key key = other.GetComponentInParent<Key>();

        if (key != null)
        {
            key.Pickup(gameObject);

            carriedKey = key;
            hasKey = true;

            Debug.Log("Picked up key");
        }
    }
}