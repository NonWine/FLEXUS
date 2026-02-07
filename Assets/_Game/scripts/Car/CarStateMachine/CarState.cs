using Zenject;

public abstract class  CarState: IState 
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
    
    protected void ChangeState<T>() where T : IState 
    {
        signalBus.Fire(new ChangeCarStateSignal()
        {
            TargetStateType = typeof(T)
        });
    }
    
}