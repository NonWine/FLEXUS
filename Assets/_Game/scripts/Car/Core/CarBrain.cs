using System;
using Zenject;
using Infrastructure;
using UnityEngine;

public class CarBrain : IInitializable, IDisposable
{
    private readonly GameStateController gameStateController;
    private readonly CarPhysics physics;
    private readonly CarData data;
    private readonly CarView view;
    private readonly CarCameraHandler cameraHandler;
    private readonly ICarInputHandler input;
    private readonly IOccupancyHandler occupancy;

    public bool IsOccupied => occupancy.IsOccupied;

    public CarBrain(
        GameStateController gameStateController, 
        ICarInputHandler input, 
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
        view.OnInteractedEvent += EnterCar;
    }

    public void Dispose()
    {
        input.OnExitPerformed -= TryExit;
        view.OnInteractedEvent -= EnterCar;
    }

    public void EnterCar(GameObject interactor)
    {
        if (IsOccupied) return;

        input.Enable();
        occupancy.Enter(interactor);
        cameraHandler.SetActive(true);
        gameStateController.SetState(GameState.Car);
        view.HideUx();
    }

    private void TryExit()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;
        ExitCar();
    }

    public void ExitCar()
    {
        occupancy.Exit((player, warpDelta) => 
        {
            cameraHandler.WarpPlayerCamera(player, warpDelta);
            cameraHandler.SetActive(false);
            input.Disable();
            gameStateController.SetState(GameState.Player);
            view.ShowUx();
        });
    }
}
