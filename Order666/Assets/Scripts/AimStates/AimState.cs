using UnityEngine;

public class AimState : AimBaseState
{
    public override void EnterState(AimManager aim)
    {
        aim.animator.SetBool("Aiming", true);
        aim.currentFov = aim.aimFov;
    }

    public override void UpdateState(AimManager aim)
    {


    }
}
