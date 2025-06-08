using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string sceneToLoad = "Cutscene"; // Name of the scene you want to load

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
