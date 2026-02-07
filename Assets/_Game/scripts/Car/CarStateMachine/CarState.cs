using Zenject;

public abstract class  CarState: IState 
{
   [Inject] protected readonly SignalBus signalBus;
   
    
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