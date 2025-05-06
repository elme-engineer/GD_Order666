using UnityEngine;

public class SettingsPanelToggle : MonoBehaviour
{
    public GameObject settingsPanel;

    void Start()
    {
        // Hide the settings panel at the start of the game
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}
