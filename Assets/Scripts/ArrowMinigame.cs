using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ArrowMinigame : MonoBehaviour,
    PlayerInputSystem.IPlayer2Actions
{
    public int sequenceLength = 5;
    public float timeLimit = 3f;
    public TextMeshProUGUI debugText;

    private List<Vector2> sequence = new List<Vector2>();
    private int currentIndex = 0;
    private float timer;

    private PlayerInputSystem input;
    private PlayerMovement3D currentPlayer;
    private bool active = false;

    private void Awake()
    {
        input = new PlayerInputSystem();
    }

    public void StartMinigame(PlayerMovement3D player)
{
    Debug.Log("StartMinigame called");

    if (input == null)
    {
        Debug.LogError("INPUT IS NULL");
        input = new PlayerInputSystem();
    }

    currentPlayer = player;
    active = true;
    if (debugText == null)
{
    Debug.LogError("DEBUG TEXT IS NULL!");
}
else
{
    Debug.Log("DEBUG TEXT FOUND → enabling");
    debugText.gameObject.SetActive(true);
}

    GenerateSequence();
    currentIndex = 0;
    timer = timeLimit;

    input.Player2.AddCallbacks(this);
    input.Player2.Enable();

    Debug.Log("Minigame successfully started");
}

    private void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Fail();
        }
    }

    private void GenerateSequence()
{
    sequence.Clear();

    Vector2[] directions = {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    for (int i = 0; i < sequenceLength; i++)
    {
        sequence.Add(directions[Random.Range(0, directions.Length)]);
    }

    string seqText = "";

    foreach (var dir in sequence)
    {
        if (dir == Vector2.up) seqText += "↑ ";
        else if (dir == Vector2.down) seqText += "↓ ";
        else if (dir == Vector2.left) seqText += "← ";
        else if (dir == Vector2.right) seqText += "→ ";
    }

    Debug.Log("Sequence: " + seqText);

    if (debugText != null)
        debugText.text = seqText;
}

    private void CheckInput(Vector2 inputDir)
    {
        if (!active) return;
        if (currentIndex >= sequence.Count) return; // ✅ safety

        Debug.Log("Input: " + inputDir);

        if (Vector2.Dot(inputDir, sequence[currentIndex]) > 0.9f)
        {
            Debug.Log("Correct!");

            currentIndex++;

            if (currentIndex >= sequence.Count)
            {
                Success();
            }
        }
        else
        {
            Debug.Log("Wrong input!");
            Fail();
        }
    }

    private void Success()
    {
        Debug.Log("SUCCESS!");
        EndMinigame();
    }

    private void Fail()
    {
        Debug.Log("FAILED!");
        EndMinigame();
    }

    private void EndMinigame()
    {
        active = false;
        debugText.gameObject.SetActive(false);

        input.Player2.Disable();

        if (currentPlayer != null)
            currentPlayer.enabled = true;
    }

    // ===== INPUT (Player2 ONLY) =====

    public void OnArrowsMovement(InputAction.CallbackContext context)
    {
        if (!active) return;
        if (!context.performed) return;

        Vector2 inputDir = context.ReadValue<Vector2>();
        CheckInput(inputDir);
    }

    // Required but unused
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context) { }
}