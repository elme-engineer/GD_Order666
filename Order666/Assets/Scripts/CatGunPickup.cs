using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CatGunPickup : MonoBehaviour
{
    [Tooltip("GameObject that should be disabled.")]
    public GameObject catGate;
    [HideInInspector] public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            AimManager aimManager = other.GetComponent<AimManager>();
            
            if (aimManager != null)
            {
                aimManager.UnlockGunMode();
                //animator = other.GetComponentInChildren<Animator>();
                //animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 1);

            }

            // Disable the assigned door GameObject
            if (catGate != null)
            {
                catGate.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameObject not assigned in OpenGateController.");
            }

            Destroy(gameObject);

        }
    }
}
