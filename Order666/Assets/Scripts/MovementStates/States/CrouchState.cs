using UnityEngine;

public class CrouchState : MovementBaseState
{
    private bool crouchReleasedSinceEntry;

    public override void EnterState(PlayerController player)
    {
        player.animator.SetBool("Crouching", true);
        crouchReleasedSinceEntry = false;
    }

    public override void UpdateState(PlayerController player)
    {
        // Exit condition
        if (player.crouchToggleEnabled)
        {
            // Wait for player to release then press again for toggle mode
            if (!player.IsCrouchPressed())
                crouchReleasedSinceEntry = true;

            if (player.IsCrouchPressed() && crouchReleasedSinceEntry)
                Transition(player, player.Idle);
        }
        else
        {
            // In hold mode, exit when key is released
            if (!player.IsCrouchPressed())
                Transition(player, player.Idle);
        }

        if (player.IsMoving())
        {
            player.currentSpeed = player.vInput < 0
                ? player.crouchBackSpeed
                : player.crouchSpeed;
        }
        else
        {
            player.currentSpeed = 0f;
        }
    }

    public override void ExitState(PlayerController player)
    {
        player.animator.SetBool("Crouching", false);
    }

    private void Transition(PlayerController player, MovementBaseState newState)
    {
        ExitState(player);
        player.SwitchState(newState);
    }
}
