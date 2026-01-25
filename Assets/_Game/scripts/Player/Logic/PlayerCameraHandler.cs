using UnityEngine;
using Unity.Cinemachine;
using Zenject;
using Infrastructure;

public class PlayerCameraHandler : IInitializable
{
    private readonly CinemachineCamera playerCamera;
    private readonly PlayerView view;

    public PlayerCameraHandler(
        [Inject(Id = "Player")] CinemachineCamera playerCamera, 
        PlayerView view)
    {
        this.playerCamera = playerCamera;
        this.view = view;
    }

    public void Initialize()
    {
        playerCamera.LookAt = view.LookAt;
        playerCamera.Follow = view.LookAt;
        playerCamera.Priority = CameraConstants.DefaultPriority;
    }
    
}
