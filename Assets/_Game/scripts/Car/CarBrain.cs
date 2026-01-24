using UnityEngine;
using Zenject;
using Infrastructure;
using System;
using Unity.Cinemachine;

public class CarBrain : IInitializable, IDisposable
{
    private const float GROUND_CHECK_DISTANCE = 5f;
    private const float GROUND_OFFSET = 0.05f;

    private readonly GameStateController gameStateController;
    private readonly CarInputHandler input;
    private readonly CarPhysics physics;
    private readonly CarData data;
    private readonly CarView view;
    private readonly CinemachineCamera carCamera;

    private GameObject currentPlayer;
    private bool isOccupied;

    public bool IsOccupied => isOccupied;

    public CarBrain(
        GameStateController gameStateController, 
        CarInputHandler input, 
        CarPhysics physics, 
        CarData data,
        CarView view,
        [Inject(Id = "Car")] CinemachineCamera carCamera) 
    {
        this.gameStateController = gameStateController;
        this.input = input;
        this.physics = physics;
        this.data = data;
        this.view = view;
        this.carCamera = carCamera;
    }

    public void Initialize()
    {
        input.OnExitPerformed += TryExit;
        view.OnInteracted += EnterCar;
        isOccupied = false;
    }

    public void Dispose()
    {
        input.OnExitPerformed -= TryExit;
        view.OnInteracted -= EnterCar;
    }

    public void EnterCar(GameObject interactor)
    {
        if (isOccupied) return;

        currentPlayer = interactor;
        isOccupied = true;
        
        
        if (currentPlayer != null)
        {
            currentPlayer.transform.SetParent(view.transform);
            currentPlayer.transform.localPosition = Vector3.zero;
            currentPlayer.SetActive(false);
        }

        if (carCamera != null) 
        {
            carCamera.Follow = view.transform;
            carCamera.LookAt = view.transform;
            carCamera.Priority = CameraConstants.ActiveVehiclePriority;
        }
        
        gameStateController.SetState(GameState.Car);
        input.Enable();

    }

    private void TryExit()
    {
        if (physics.CurrentSpeedKmH > data.maxExitSpeedKmH) return;
        
        ExitCar();
    }

    public void ExitCar()
    {
        if (currentPlayer != null)
        {
            currentPlayer.transform.SetParent(null);
            
            Vector3 targetPosition = view.ExitPoint.position;
            if (Physics.Raycast(view.ExitPoint.position + Vector3.up, Vector3.down, out RaycastHit hit, GROUND_CHECK_DISTANCE))
            {
                targetPosition = hit.point + Vector3.up * GROUND_OFFSET;
            }

            var controller = currentPlayer.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            Vector3 oldPosition = currentPlayer.transform.position;
            currentPlayer.transform.position = targetPosition;
            currentPlayer.transform.rotation = view.ExitPoint.rotation;

            if (carCamera != null)
            {
                var playerVcam = currentPlayer.GetComponentInChildren<CinemachineCamera>();
                if (playerVcam != null)
                {
                    playerVcam.OnTargetObjectWarped(currentPlayer.transform, targetPosition - oldPosition);
                }
                carCamera.Priority = CameraConstants.InactivePriority;
            }
            
            currentPlayer.SetActive(true);
            if (controller != null) controller.enabled = true;
        }

        isOccupied = false;
        
        input.Disable();

        gameStateController.SetState(GameState.Player);
    }
}
