using UnityEngine;
using Zenject;

public class CarEnterState : CarState
{
    private readonly IOccupancyHandler occupancy;

    public CarEnterState(SignalBus signalBus, IOccupancyHandler occupancy) : base(signalBus)
    {
        this.occupancy = occupancy;
    }

    public override void Enter()
    {
        
    }
    
    private void HandleInteraction(GameObject interactor)
    {
        if (occupancy.IsOccupied) return;

        occupancy.Enter(interactor);
        signalBus.Fire(new ChangeCarStateSignal 
        { 
            TargetStateType = typeof(CarExitState) 
        });    
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}