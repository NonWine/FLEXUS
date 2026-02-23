using UnityEngine;
using Unity.Cinemachine;
using Infrastructure;
using Zenject;

public class CarCameraHandler
{
    private readonly CinemachineCamera carCamera;
    private readonly CinemachineCamera playerCamera;
    private readonly CarView view;

    public CarCameraHandler(
        [Inject(Id = "Car")] CinemachineCamera carCamera,
        [Inject(Id = "Player")] CinemachineCamera playerCamera,
        CarView view)
    {
        this.carCamera = carCamera;
        this.playerCamera = playerCamera;
        this.view = view;
    }

    public void SetActive(bool active)
    {

        if (active)
        {
            carCamera.PreviousStateIsValid = false;
            carCamera.Follow = view.transform;
            carCamera.LookAt = view.transform;
            carCamera.Priority = CameraConstants.ActiveVehiclePriority;
        }
        else
        {
            carCamera.Priority = CameraConstants.InactivePriority;
        }
    }

    public void WarpPlayerCamera(GameObject player, Vector3 warpDelta)
    {
        playerCamera.OnTargetObjectWarped(player.transform, warpDelta);
    }
}
