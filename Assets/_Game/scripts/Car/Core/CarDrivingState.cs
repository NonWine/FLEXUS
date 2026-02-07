using UnityEngine;
using Infrastructure;
using Zenject;

public class CarDrivingState : CarState
{
    private readonly ICarInputHandler input;
    private readonly CarCameraHandler cameraHandler;
    private readonly GameStateController gameStateController;
    private readonly CarPhysics physics;
    private readonly CarData data;

    public CarDrivingState(
        ICarInputHandler input, 
        CarCameraHandler cameraHandler, 
        GameStateController gameStateController,
        CarPhysics physics,
        CarData data,
        SignalBus signalBus) : base(signalBus)
    {
        this.input = input;
        this.cameraHandler = cameraHandler;
        this.gameStateController = gameStateController;
        this.physics = physics;
        this.data = data;
    }

    public override void Enter()
    {
        input.Enable();
        cameraHandler.SetActive(true);
        gameStateController.SetState(GameState.Car);
        signalBus.Subscribe<CarExitRequestSignal>(HandleExitRequest);
    }

    public override void Exit()
    {
        input.Disable();
        cameraHandler.SetActive(false);
        gameStateController.SetState(GameState.Player);
        signalBus.Unsubscribe<CarExitRequestSignal>(HandleExitRequest);
    }

    private void HandleExitRequest()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;
        
        ChangeState<CarExitState>();
    }

}
