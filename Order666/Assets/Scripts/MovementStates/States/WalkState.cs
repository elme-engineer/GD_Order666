using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.animator.SetBool("Walking", true);
    }

    public override void UpdateState(PlayerController player)
    {
        if (player.IsSprintPressed())
            Transition(player, player.Run);

        else if (player.IsCrouchInitiated())
            Transition(player, player.Crouch);

        else if (!player.IsMoving())
            Transition(player, player.Idle);

        player.currentSpeed = player.vInput < 0
            ? player.walkBackSpeed
            : player.walkSpeed;

        if (player.IsJumpPressed() && player.controller.isGrounded)
        {
            player.previousState = this;
            Transition(player, player.Jump);
        }
    }

    public override void ExitState(PlayerController player)
    {
        player.animator.SetBool("Walking", false);
    }

    private void Transition(PlayerController player, MovementBaseState newState)
    {
        ExitState(player);
        player.SwitchState(newState);
    }
}
