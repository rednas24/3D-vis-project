using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public GameObject portalObject;

    [Header("Sound")]
    public AudioClip activationSound;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        PlayerMovement3D player =
            other.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        activated = true;

        // Play sound even after object is destroyed
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(
                activationSound,
                transform.position
            );
        }

        // Activate portal
        if (portalObject != null)
        {
            portalObject.SetActive(true);
        }

        Debug.Log("Portal activated");

        // Destroy immediately
        Destroy(gameObject);
    }
}