using UnityEngine;
using Zenject;

public class EmptyCarState : CarState
{
    private readonly CarView view;
    private readonly IOccupancyHandler occupancy;

    public EmptyCarState(CarView view, IOccupancyHandler occupancy, SignalBus signalBus) : base(signalBus)
    {
        this.view = view;
        this.occupancy = occupancy;
    }

    public override void Enter()
    {
        signalBus.Subscribe<CarInteractionSignal>(HandleInteraction);
    }

    public override void Exit()
    {
        signalBus.Unsubscribe<CarInteractionSignal>(HandleInteraction);
    }

    private void HandleInteraction(CarInteractionSignal signal)
    {
        if (occupancy.IsOccupied) return;
        
        occupancy.Enter(signal.Interactor);
        ChangeState<DrivingCarState>();
    }

    public override void Tick() { }
    
}
