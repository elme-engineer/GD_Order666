using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public PlayerController playerController;
    
    public void Jumped()
    {
        if (playerController != null)
        {

            playerController.Jumped();
        }
    }

    public void JumpedForce()
    {
        Debug.Log("JumpForce animation event triggered");
        if (playerController != null)
        {
            playerController.JumpForce();
        }
    }
}