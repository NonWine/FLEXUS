public abstract class CarState : ICarState
{
    protected readonly CarStateMachine stateMachine;

    public abstract void Enter();

    public abstract void Exit();

    public virtual void Tick()
    {
        
    }
}