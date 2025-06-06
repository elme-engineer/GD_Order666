using UnityEngine;

public class HipFireState : AimBaseState
{
    public override void EnterState(AimManager aim)
    {
        aim.animator.SetBool("Aiming", false);
        aim.currentFov = aim.hipFov;
    }

    public override void UpdateState(AimManager aim)
    {


    }
}
