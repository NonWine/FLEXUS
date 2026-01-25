using UnityEngine;
using Zenject;
using Unity.Cinemachine;

namespace Infrastructure
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private CameraManager cameraManager;

        public override void InstallBindings()
        {
            Container.BindInstance(cameraManager).AsSingle();
            
            Container.Bind<CinemachineCamera>()
                .WithId("Player")
                .FromInstance(cameraManager.PlayerCamera)
                .AsCached();

            Container.Bind<CinemachineCamera>()
                .WithId("Car")
                .FromInstance(cameraManager.CarCamera)
                .AsCached();
        }
    }
}
