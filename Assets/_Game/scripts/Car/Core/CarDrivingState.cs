using UnityEngine;
using Infrastructure;
using System;

public class CarDrivingState : ICarState, IDisposable
{
    private readonly ICarInputHandler input;
    private readonly CarCameraHandler cameraHandler;
    private readonly GameStateController gameStateController;
    private readonly CarPhysics physics;
    private readonly CarData data;
    private readonly CarStateMachine stateMachine;

    public CarDrivingState(
        ICarInputHandler input, 
        CarCameraHandler cameraHandler, 
        GameStateController gameStateController,
        CarPhysics physics,
        CarData data,
        CarStateMachine stateMachine)
    {
        this.input = input;
        this.cameraHandler = cameraHandler;
        this.gameStateController = gameStateController;
        this.physics = physics;
        this.data = data;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        input.Enable();
        cameraHandler.SetActive(true);
        gameStateController.SetState(GameState.Car);
        input.OnExitPerformed += TryExit;
    }

    public void Exit()
    {
        input.OnExitPerformed -= TryExit;
        input.Disable();
        cameraHandler.SetActive(false);
        gameStateController.SetState(GameState.Player);
    }

    private void TryExit()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;

        stateMachine.ChangeState<CarExitState>();
    } 

    public void Tick() { }
    public void Dispose() => Exit();
}
