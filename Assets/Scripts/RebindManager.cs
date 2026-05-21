using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    [Header("UI Text")]
    public TMP_Text cactusInteractText;
    public TMP_Text jumpText;
    public TMP_Text shroomInteractText;

    private PlayerInputSystem inputActions;
    private InputActionRebindingExtensions.RebindingOperation currentRebind;

    private void Awake()
    {
        inputActions = new PlayerInputSystem();

        // Load saved bindings
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            inputActions.asset.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString("InputBindings"));
        }
    }

    private void Start()
    {
        RefreshTexts();
    }

    public void RefreshTexts()
    {
        cactusInteractText.text =
            inputActions.Player1.Attack.GetBindingDisplayString();

        jumpText.text =
            inputActions.Player1.Jump.GetBindingDisplayString();

        shroomInteractText.text =
            inputActions.Player2.Interact.GetBindingDisplayString();
    }

    public void RebindCactusInteract()
    {
        StartRebind(
            inputActions.Player1.Attack,
            cactusInteractText);
    }

    public void RebindJump()
    {
        StartRebind(
            inputActions.Player1.Jump,
            jumpText);
    }

    public void RebindShroomInteract()
    {
        StartRebind(
            inputActions.Player2.Interact,
            shroomInteractText);
    }

    private void StartRebind(
        InputAction action,
        TMP_Text textField)
    {
        if (currentRebind != null)
            currentRebind.Dispose();

        action.Disable();

        textField.text = "Press Key...";

        currentRebind = action.PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
                .OnComplete(operation =>
                {
                    string newPath = action.bindings[0].effectivePath;

                    bool duplicateFound = false;

                    foreach (InputAction otherAction in inputActions.asset)
                    {
                        if (otherAction == action)
                            continue;

                        foreach (InputBinding binding in otherAction.bindings)
                        {
                            if (binding.effectivePath == newPath)
                            {
                                duplicateFound = true;
                                break;
                            }
                        }

                        if (duplicateFound)
                            break;
                    }

                    if (duplicateFound)
                    {
                        Debug.Log("That key is already in use!");

                        action.RemoveBindingOverride(0);

                        textField.text =
                            action.GetBindingDisplayString();
                    }
                    else
                    {
                        textField.text =
                            action.GetBindingDisplayString();

                        SaveBindings();

                        PlayerMovement3D[] players =
                            FindObjectsByType<PlayerMovement3D>(
                                FindObjectsSortMode.None);

                        foreach (PlayerMovement3D player in players)
                        {
                            player.ReloadBindings();
                        }
                    }

                    action.Enable();

                    operation.Dispose();
                    currentRebind = null;
                });

        currentRebind.Start();
    }

    private void SaveBindings()
    {
        string json =
            inputActions.asset.SaveBindingOverridesAsJson();

        Debug.Log("Saving bindings: " + json);

        PlayerPrefs.SetString("InputBindings", json);
        PlayerPrefs.Save();
    }

    public void ResetBindings()
    {
        inputActions.asset.RemoveAllBindingOverrides();

        PlayerPrefs.DeleteKey("InputBindings");

        RefreshTexts();
    }
}