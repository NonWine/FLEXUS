using UnityEngine;
using System;

public class CarEmptyState : ICarState, IDisposable
{
    private readonly CarView view;
    private readonly IOccupancyHandler occupancy;
    private readonly CarStateMachine stateMachine;

    public CarEmptyState(CarView view, IOccupancyHandler occupancy, CarStateMachine stateMachine)
    {
        this.view = view;
        this.occupancy = occupancy;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        view.OnInteractedEvent += HandleInteraction;
        view.ShowUx();
    }

    public void Exit()
    {
        view.OnInteractedEvent -= HandleInteraction;
        view.HideUx();
    }

    private void HandleInteraction(GameObject interactor)
    {
        if (occupancy.IsOccupied) return;
        
        occupancy.Enter(interactor);
        stateMachine.ChangeState<CarDrivingState>();
    }

    public void Tick() { }
    public void Dispose() => Exit();
}
