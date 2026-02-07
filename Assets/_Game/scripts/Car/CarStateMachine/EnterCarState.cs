using UnityEngine;
using Zenject;

public class EnterCarState : CarState
{
    private readonly IOccupancyHandler occupancy;

    public EnterCarState( IOccupancyHandler occupancy) 
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
            TargetStateType = typeof(ExitCarState) 
        });    
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}