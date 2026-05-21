using TMPro;
using UnityEngine;

public class WinScreenManager : MonoBehaviour
{
    public TMP_Text timeText;

    private void Start()
    {
        timeText.text = LevelTimer.Instance.GetFormattedTime();
    }
}