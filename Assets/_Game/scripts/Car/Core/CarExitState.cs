using Zenject;

public class CarExitState : CarState
{
    private readonly IOccupancyHandler occupancy;
    private readonly CarCameraHandler cameraHandler;

    public CarExitState(IOccupancyHandler occupancy, CarCameraHandler cameraHandler, SignalBus signalBus) : base(signalBus)
    {
        this.occupancy = occupancy;
        this.cameraHandler = cameraHandler;
    }
    
    public override void Enter()
    {
        occupancy.Exit((player, warpDelta) => 
        {
            cameraHandler.WarpPlayerCamera(player, warpDelta);
            ChangeState<CarEmptyState>();
        });
    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
    }
}