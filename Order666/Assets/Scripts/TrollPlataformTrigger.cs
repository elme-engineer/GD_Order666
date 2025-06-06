using UnityEngine;

public class TrollPlataformTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject wallToActivate;
    public Renderer platformRenderer;
    public Material newMaterial;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // 1. Activate wall
            if (wallToActivate != null)
                wallToActivate.SetActive(true);

            // 2. Change platform texture
            if (platformRenderer != null && newMaterial != null)
                platformRenderer.material = newMaterial;

        }
    }

}
