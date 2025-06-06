using UnityEngine;

public class FryerTriggerSave : MonoBehaviour
{
    public FairyRescue fairyRescue;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fairyRescue.TriggerFryerRescue();
        }
    }
}
