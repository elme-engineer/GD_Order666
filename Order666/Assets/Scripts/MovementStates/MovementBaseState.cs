
public abstract class MovementBaseState
{
    public abstract void EnterState(PlayerController playerController);

    public abstract void UpdateState(PlayerController playerController);

    public abstract void ExitState(PlayerController playerController);
}
