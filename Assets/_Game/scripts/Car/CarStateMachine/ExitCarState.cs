using Zenject;

public class ExitCarState : CarState
{
    private readonly IOccupancyHandler occupancy;
    private readonly CarCameraHandler cameraHandler;

    public ExitCarState(IOccupancyHandler occupancy, CarCameraHandler cameraHandler, SignalBus signalBus) : base(signalBus)
    {
        this.occupancy = occupancy;
        this.cameraHandler = cameraHandler;
    }
    
    public override void Enter()
    {
        occupancy.Exit((player, warpDelta) => 
        {
            cameraHandler.WarpPlayerCamera(player, warpDelta);
            ChangeState<EmptyCarState>();
        });
    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
    }
}