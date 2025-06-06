using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuitButton : MonoBehaviour
{
    public string sceneToLoad = "MainMenu"; 

    public void QuitGame()
    {
        Debug.Log("Returning to scene: " + sceneToLoad);

        SceneManager.LoadScene(sceneToLoad);
    }
}
