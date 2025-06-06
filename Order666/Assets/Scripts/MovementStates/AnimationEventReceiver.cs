using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private PlayerController playerController;



    private void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }
    public void Jumped()
    {
        if (playerController != null)
        {

            playerController.Jumped();
        }
    }

    public void JumpedForce()
    {
        //Debug.Log("JumpForce animation event triggered");
        if (playerController != null)
        {
            playerController.JumpForce();
        }
    }
}