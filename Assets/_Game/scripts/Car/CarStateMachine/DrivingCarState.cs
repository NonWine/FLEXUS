
public class DrivingCarState : CarState
{
    private readonly ICarInputHandler input;
    private readonly CarCameraHandler cameraHandler;
    private readonly CarPhysics physics;
    private readonly CarData data;

    public DrivingCarState(
        ICarInputHandler input, 
        CarCameraHandler cameraHandler, 
        CarPhysics physics,
        CarData data) 
    {
        this.input = input;
        this.cameraHandler = cameraHandler;
        this.physics = physics;
        this.data = data;
    }

    public override void Enter()
    {
        input.Enable();
        cameraHandler.SetActive(true);
        signalBus.Fire(new VehicleOccupiedSignal { IsOccupied = true });
        signalBus.Subscribe<CarExitRequestSignal>(HandleExitRequest);
    }

    public override void Exit()
    {
        input.Disable();
        cameraHandler.SetActive(false);
        signalBus.Fire(new VehicleOccupiedSignal { IsOccupied = false });
        signalBus.Unsubscribe<CarExitRequestSignal>(HandleExitRequest);
    }

    private void HandleExitRequest()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;
        
        ChangeState<ExitCarState>();
    }

}
