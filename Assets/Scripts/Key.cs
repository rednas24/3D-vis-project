using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 2f, -1.5f); // above + behind
    public float followSpeed = 5f;
    public float rotationSpeed = 10f;
    public Vector3 rotationOffset = new Vector3(-90f, 0f, 0f);

    private Transform targetPlayer;
    private bool isFollowing = false;

    public void Pickup(GameObject player)
    {
        Debug.Log(player.name + " picked up the key");

        // Disable physics
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        targetPlayer = player.transform;
        isFollowing = true;
    }

    private void Update()
    {
        if (!isFollowing || targetPlayer == null) return;

        // Target position (relative to player's facing direction)
        Vector3 targetPosition =
            targetPlayer.position +
            targetPlayer.TransformDirection(offset);

        // Smooth movement (lag effect)
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

            Quaternion targetRotation =
            targetPlayer.rotation * Quaternion.Euler(rotationOffset);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}