using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject aboutPanel;

    // Called when the Settings button is clicked
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        aboutPanel.SetActive(false); // hide About just in case
    }

    // Called when the ❌ Close button is clicked
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    // Called when "About" is clicked
    public void OpenAbout()
    {
        aboutPanel.SetActive(true);
    }

    // Called when "Back" is clicked inside About Panel
    public void CloseAbout()
    {
        aboutPanel.SetActive(false);
    }
}
