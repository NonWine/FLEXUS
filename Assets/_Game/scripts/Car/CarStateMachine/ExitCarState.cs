
public class ExitCarState : CarState
{
    private readonly IOccupancyHandler occupancy;
    private readonly CarCameraHandler cameraHandler;

    public ExitCarState(IOccupancyHandler occupancy, CarCameraHandler cameraHandler) 
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
}