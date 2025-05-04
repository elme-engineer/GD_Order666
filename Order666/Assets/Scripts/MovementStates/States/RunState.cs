using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.animator.SetBool("Running", true);
    }

    public override void UpdateState(PlayerController player)
    {
        if (!player.IsSprintPressed())
            Transition(player, player.Walk);

        else if (!player.IsMoving())
            Transition(player, player.Idle);

        player.currentSpeed = player.vInput < 0
            ? player.runBackSpeed
            : player.runSpeed;


        if (player.IsJumpPressed() && player.controller.isGrounded)
        {
            player.previousState = this;
            Transition(player, player.Jump);
        }
    }

    public override void ExitState(PlayerController player)
    {
        player.animator.SetBool("Running", false);
    }

    private void Transition(PlayerController player, MovementBaseState newState)
    {
        ExitState(player);
        player.SwitchState(newState);
    }
}
