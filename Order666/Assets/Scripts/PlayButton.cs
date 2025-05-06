using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string sceneToLoad = "SampleScene"; // Name of the scene you want to load

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
