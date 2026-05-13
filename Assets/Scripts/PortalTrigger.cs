using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public GameObject portalObject;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        PlayerMovement3D player =
            other.GetComponent<PlayerMovement3D>();

        if (player == null) return;

        activated = true;

        if (portalObject != null)
        {
            portalObject.SetActive(true);
        }

        // Destroy the object that entered the trigger
        Destroy(gameObject);

        Debug.Log("Portal activated");
    }
}