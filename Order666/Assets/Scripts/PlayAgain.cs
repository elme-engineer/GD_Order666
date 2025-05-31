using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayAgain : MonoBehaviour
{
    public float delayInSeconds = 20f;
    public string sceneToLoad = "MainMenu";

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneToLoad);
    }
}

