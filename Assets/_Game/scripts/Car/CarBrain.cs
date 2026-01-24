using System;
using Zenject;
using Infrastructure;
using UnityEngine;

public class CarBrain : IInitializable, IDisposable
{
    private readonly GameStateController gameStateController;
    private readonly CarInputHandler input;
    private readonly CarPhysics physics;
    private readonly CarData data;
    private readonly CarView view;
    private readonly IOccupancyHandler occupancy;
    private readonly CarCameraHandler cameraHandler;

    public bool IsOccupied => occupancy.IsOccupied;

    public CarBrain(
        GameStateController gameStateController, 
        CarInputHandler input, 
        CarPhysics physics, 
        CarData data,
        CarView view,
        IOccupancyHandler occupancy,
        CarCameraHandler cameraHandler)
    {
        this.gameStateController = gameStateController;
        this.input = input;
        this.physics = physics;
        this.data = data;
        this.view = view;
        this.occupancy = occupancy;
        this.cameraHandler = cameraHandler;
    }

    public void Initialize()
    {
        input.OnExitPerformed += TryExit;
        view.OnInteracted += EnterCar;
    }

    public void Dispose()
    {
        input.OnExitPerformed -= TryExit;
        view.OnInteracted -= EnterCar;
    }

    public void EnterCar(GameObject interactor)
    {
        if (IsOccupied) return;

        input.Enable();
        occupancy.Enter(interactor);
        cameraHandler.SetActive(true);
        gameStateController.SetState(GameState.Car);
    }

    private void TryExit()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;
        ExitCar();
    }

    private void ExitCar()
    {
        occupancy.Exit((player, warpDelta) => 
        {
            cameraHandler.WarpPlayerCamera(player, warpDelta);
            cameraHandler.SetActive(false);
            input.Disable();
            gameStateController.SetState(GameState.Player);
        });
    }
}