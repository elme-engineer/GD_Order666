using UnityEngine;

public class SettingsPanelCloser : MonoBehaviour
{
    public GameObject settingsPanel;

    public void CloseSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
    }
}
