using UnityEngine;

public class CarEnterState : CarState
{
    private readonly IOccupancyHandler occupancy;
    
    public override void Enter()
    {
        
    }
    
    private void HandleInteraction(GameObject interactor)
    {
        if (occupancy.IsOccupied) return;

        occupancy.Enter(interactor);
        stateMachine.ChangeState<CarDrivingState>();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}