using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(PlayerController player)
    {
        // Optionally reset animator states
    }

    public override void UpdateState(PlayerController player)
    {
        if (player.IsMoving())
        {
            if (player.IsSprintPressed())
                player.SwitchState(player.Run);
            else
                player.SwitchState(player.Walk);
        }

        if (player.IsCrouchInitiated())
            player.SwitchState(player.Crouch);
    }

    public override void ExitState(PlayerController player)
    {
        // Optional cleanup
    }
}
