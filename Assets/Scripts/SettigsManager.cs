using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    // TEMP VALUES
    private int pendingResolutionIndex;
    private float pendingVolume;
    private bool pendingFullscreen;

    private List<Resolution> filteredResolutions =
        new List<Resolution>();

    private void Start()
    {
        SetupResolutions();

        // Load saved volume
        pendingVolume =
            PlayerPrefs.GetFloat(
                "MasterVolume",
                AudioListener.volume);

        volumeSlider.value = pendingVolume;

        // Load saved fullscreen
        pendingFullscreen =
            PlayerPrefs.GetInt(
                "Fullscreen",
                Screen.fullScreen ? 1 : 0) == 1;

        fullscreenToggle.isOn = pendingFullscreen;

        volumeSlider.onValueChanged.AddListener(
            UpdatePendingVolume);

        resolutionDropdown.onValueChanged.AddListener(
            UpdatePendingResolution);

        fullscreenToggle.onValueChanged.AddListener(
            UpdatePendingFullscreen);
    }

    void SetupResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        Resolution[] allResolutions = Screen.resolutions;

        HashSet<string> uniqueResolutions =
            new HashSet<string>();

        int currentResolutionIndex = 0;

        foreach (Resolution resolution in allResolutions)
        {
            string option =
                resolution.width + " x " + resolution.height;

            if (!uniqueResolutions.Contains(option))
            {
                uniqueResolutions.Add(option);

                filteredResolutions.Add(resolution);

                options.Add(option);

                if (resolution.width ==
                    Screen.currentResolution.width &&
                    resolution.height ==
                    Screen.currentResolution.height)
                {
                    currentResolutionIndex =
                        filteredResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        pendingResolutionIndex =
            PlayerPrefs.GetInt(
                "ResolutionIndex",
                currentResolutionIndex);

        resolutionDropdown.value =
            pendingResolutionIndex;

        resolutionDropdown.RefreshShownValue();
    }

    // STORE TEMP RESOLUTION
    public void UpdatePendingResolution(int index)
    {
        pendingResolutionIndex = index;
    }

    // STORE TEMP VOLUME
    public void UpdatePendingVolume(float volume)
    {
        pendingVolume = volume;
    }

    // STORE TEMP FULLSCREEN
    public void UpdatePendingFullscreen(bool fullscreen)
    {
        pendingFullscreen = fullscreen;
    }

    // APPLY SETTINGS
    public void ApplySettings()
    {
        ApplyResolution();
        ApplyVolume();

        SaveSettings();

        Debug.Log("Settings Applied");
    }

    void ApplyResolution()
    {
        Resolution resolution =
            filteredResolutions[pendingResolutionIndex];

        Screen.SetResolution(
            resolution.width,
            resolution.height,
            pendingFullscreen
        );

        Debug.Log(
            "Applied Resolution: " +
            resolution.width + "x" +
            resolution.height +
            " Fullscreen: " +
            pendingFullscreen
        );
    }

    void ApplyVolume()
    {
        AudioListener.volume = pendingVolume;
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt(
            "ResolutionIndex",
            pendingResolutionIndex
        );

        PlayerPrefs.SetFloat(
            "MasterVolume",
            pendingVolume
        );

        PlayerPrefs.SetInt(
            "Fullscreen",
            pendingFullscreen ? 1 : 0
        );

        PlayerPrefs.Save();
    }
}