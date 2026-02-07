using Zenject;

public abstract class CarState : ICarState
{
 
    protected readonly SignalBus signalBus;

    public CarState(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }
    
    public abstract void Enter();

    public abstract void Exit();

    public virtual void Tick()
    {
        
    }
    
    protected void ChangeState<T>() where T : ICarState 
    {
        signalBus.Fire(new ChangeCarStateSignal()
        {
            TargetStateType = typeof(T)
        });
    }
}