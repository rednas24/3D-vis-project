using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalUnlock : MonoBehaviour
{
    public int requiredKeys = 3;

    private int insertedKeys = 0;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        PlayerPickup player = other.GetComponent<PlayerPickup>();

        if (player == null) return;

        if (player.hasKey)
        {
            player.hasKey = false;

            insertedKeys++;

            Debug.Log("Inserted Keys: " + insertedKeys);

            // Remove carried key
            if (player.carriedKey != null)
            {
                Destroy(player.carriedKey.gameObject);
                player.carriedKey = null;
            }

            // Check if enough keys have been inserted
            if (insertedKeys >= requiredKeys)
            {
                ActivatePortal();
            }
        }
    }

    private void ActivatePortal()
    {
        activated = true;

        Debug.Log("Portal Activated!");

        // Load score menu scene
        SceneManager.LoadScene("ScoreMenu");
    }
}