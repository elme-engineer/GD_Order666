using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // This lets us stop Play Mode in the Unity Editor
#endif

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        // This works only in the built game
        Application.Quit();

        // This part works in the Unity Editor
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
