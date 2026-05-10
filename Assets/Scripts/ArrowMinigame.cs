using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArrowMinigame : MonoBehaviour,
    PlayerInputSystem.IPlayer2Actions
{
    [Header("UI")]
    public GameObject minigameUI;

    [Header("Minigame Settings")]
    public int sequenceLength = 5;
    public float timeLimit = 3f;

    [Header("Arrow UI")]
    public Image[] arrowImages;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Chest")]
    public Animator chestAnimator;
    public Transform keyObject;
    public Transform keyRaisedPosition;

    public float keyRiseSpeed = 2f;
    public float chestOpenTime = 5f;

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

    private void Start()
    {
        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }
    }

    public void StartMinigame(PlayerMovement3D player)
    {
        Debug.Log("StartMinigame called");

        if (minigameUI != null)
        {
            minigameUI.SetActive(true);
        }

        if (arrowImages.Length < sequenceLength)
        {
            Debug.LogError("Not enough arrow images assigned!");
            return;
        }

        currentPlayer = player;

        // LOCK PLAYER MOVEMENT
        if (currentPlayer != null)
        {
            currentPlayer.enabled = false;
        }

        active = true;

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

        Vector2[] directions =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(
                directions[Random.Range(0, directions.Length)]
            );
        }

        // RESET UI
        for (int i = 0; i < arrowImages.Length; i++)
        {
            arrowImages[i].gameObject.SetActive(false);
        }

        // DISPLAY SEQUENCE
        for (int i = 0; i < sequence.Count; i++)
        {
            arrowImages[i].gameObject.SetActive(true);

            if (sequence[i] == Vector2.up)
            {
                arrowImages[i].sprite = upSprite;
            }
            else if (sequence[i] == Vector2.down)
            {
                arrowImages[i].sprite = downSprite;
            }
            else if (sequence[i] == Vector2.left)
            {
                arrowImages[i].sprite = leftSprite;
            }
            else if (sequence[i] == Vector2.right)
            {
                arrowImages[i].sprite = rightSprite;
            }

            arrowImages[i].color = Color.white;
        }

        Debug.Log("Sequence generated");
    }

    private void CheckInput(Vector2 inputDir)
    {
        if (!active) return;

        if (currentIndex >= sequence.Count) return;

        Debug.Log("Input: " + inputDir);

        if (Vector2.Dot(inputDir, sequence[currentIndex]) > 0.9f)
        {
            Debug.Log("Correct!");

            // CURRENT ARROW TURNS GREEN
            arrowImages[currentIndex].color = Color.green;

            currentIndex++;

            // FINISHED
            if (currentIndex >= sequence.Count)
            {
                Success();
            }
        }
        else
        {
            Debug.Log("Wrong input!");

            // CURRENT ARROW TURNS RED
            arrowImages[currentIndex].color = Color.red;

            Fail();
        }
    }

    private void Success()
    {
        Debug.Log("SUCCESS!");

        StartCoroutine(ChestRewardRoutine());

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

        input.Player2.Disable();

        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }

        // UNLOCK PLAYER
        if (currentPlayer != null)
        {
            currentPlayer.enabled = true;
        }

        Debug.Log("Minigame Ended");
    }

    private IEnumerator ChestRewardRoutine()
    {
        Debug.Log("Reward routine started");

        // OPEN CHEST
        if (chestAnimator != null)
        {
            chestAnimator.Play("Chest_Open");
        }

        if (keyObject == null)
        {
            Debug.LogError("KEY OBJECT IS NULL");
            yield break;
        }

        if (keyRaisedPosition == null)
        {
            Debug.LogError("KEY RAISED POSITION IS NULL");
            yield break;
        }

        Debug.Log("Moving key");

        Vector3 startPos = keyObject.position;
        Vector3 targetPos = keyRaisedPosition.position;

        float t = 0f;

        // RAISE KEY
        while (t < 1f)
        {
            t += Time.deltaTime * keyRiseSpeed;

            keyObject.position = Vector3.Lerp(
                startPos,
                targetPos,
                t
            );

            yield return null;
        }

        Debug.Log("Key fully raised");

        yield return new WaitForSeconds(chestOpenTime);

        // LOWER KEY
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * keyRiseSpeed;

            keyObject.position = Vector3.Lerp(
                targetPos,
                startPos,
                t
            );

            yield return null;
        }

        Debug.Log("Closing chest");

        if (chestAnimator != null)
        {
            chestAnimator.Play("Chest_Close");
        }
    }

    // ===== PLAYER 2 INPUT =====

    public void OnArrowsMovement(InputAction.CallbackContext context)
    {
        if (!active) return;

        if (!context.performed) return;

        Vector2 inputDir = context.ReadValue<Vector2>();

        CheckInput(inputDir);
    }

    // REQUIRED BUT UNUSED

    public void OnJump(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }
}