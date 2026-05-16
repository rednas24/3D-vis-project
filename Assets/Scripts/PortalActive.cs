using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalUnlock : MonoBehaviour
{
    public int requiredKeys = 3;

    private int insertedKeys = 0;
    private bool activated = false;

    [Header("Portal Effects")]
    public ParticleSystem[] keyEffects;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        PlayerPickup player =
            other.GetComponent<PlayerPickup>();

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

            // Activate particle effect
            int effectIndex = insertedKeys - 1;

            if (effectIndex >= 0 &&
                effectIndex < keyEffects.Length)
            {
                keyEffects[effectIndex].gameObject.SetActive(true);

                keyEffects[effectIndex].Play();
            }

            // Check if enough keys inserted
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

        SceneManager.LoadScene("ScoreMenu");
    }
}