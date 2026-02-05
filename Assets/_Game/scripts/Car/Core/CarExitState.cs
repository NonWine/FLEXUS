public class CarExitState : ICarState
{
    private readonly IOccupancyHandler occupancy;
    private readonly CarCameraHandler cameraHandler;
    private readonly CarStateMachine stateMachine;

    public CarExitState(IOccupancyHandler occupancy, CarCameraHandler cameraHandler, CarStateMachine stateMachine, CarData data)
    {
        this.occupancy = occupancy;
        this.cameraHandler = cameraHandler;
        this.stateMachine = stateMachine;
    }

    
    public void Enter()
    {
        occupancy.Exit((player, warpDelta) => 
        {
            cameraHandler.WarpPlayerCamera(player, warpDelta);
            stateMachine.ChangeState<CarEmptyState>();
        });
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
    }
}