using UnityEngine;

public class OpenGateController : MonoBehaviour
{
    [Tooltip("GameObject that should be disabled.")]
    public GameObject catGate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable the cat (this GameObject)
            gameObject.SetActive(false);

            // Disable the assigned door GameObject
            if (catGate != null)
            {
                catGate.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameObject not assigned in OpenGateController.");
            }
        }
    }
}