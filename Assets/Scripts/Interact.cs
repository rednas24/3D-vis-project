using UnityEngine;

public class Interactable : MonoBehaviour
{
    public ArrowMinigame minigame;

    public void Interact(PlayerMovement3D player)
{
    if (minigame == null)
    {
        Debug.LogError("Minigame is NOT assigned!");
        return;
    }

    minigame.StartMinigame(player);
}
}