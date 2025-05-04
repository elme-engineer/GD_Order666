using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(PlayerController playerController)
    {
        if (playerController.previousState == playerController.Idle)
            playerController.animator.SetTrigger("IdleJump");
        else if (playerController.previousState == playerController.Walk || playerController.previousState == playerController.Run)
            playerController.animator.SetTrigger("RunJump");


    }

    public override void UpdateState(PlayerController playerController)
    {
        if (playerController.jumped && playerController.controller.isGrounded)
        {
            playerController.jumped = false;

            if(playerController.hzInput == 0 && playerController.vInput == 0)
                playerController.SwitchState(playerController.Idle);
            else if (playerController.IsSprintPressed())
                playerController.SwitchState(playerController.Run);
            else
                playerController.SwitchState(playerController.Walk);
        }
        
    }

    public override void ExitState(PlayerController playerController)
    {

    }
}