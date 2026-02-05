using UnityEngine;
using Zenject;
using Unity.Cinemachine;
using Infrastructure;

public class CarInstaller : MonoInstaller
{
    [SerializeField] private CarView view;
    [SerializeField] private CarSoundView soundView;
    [SerializeField] private CarVFXView vfxView;

    public override void InstallBindings()
    {

        ViewDependencies();
        Sounds();
        VFX();
        SubComponents();
        
        // State Machine
        Container.BindInterfacesAndSelfTo<CarStateMachine>().AsSingle();
        Container.Bind<CarEmptyState>().AsSingle();
        Container.Bind<CarDrivingState>().AsSingle();
        Container.Bind<CarExitState>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<CarBrain>().AsSingle();
        Container.Bind<CarFacade>().AsSingle();
    }

    private void ViewDependencies()
    {
        Container.BindInstance(view).AsSingle();
        Container.BindInstance(view.CarData).AsSingle();
        Container.BindInstance(view.Rigidbody).AsSingle();
        Container.BindInstance(view.Wheels).AsSingle();
        Container.BindInstance(view.Meshes).AsSingle();
        Container.Bind<Transform>().FromInstance(view.transform).AsSingle();
        Container.Bind<CinemachineCamera>().WithId("Car").FromResolve();
    }

    private void Sounds()
    {
        Container.BindInstance(soundView).AsSingle();
        Container.BindInstance(view.CarData.soundSettings).AsSingle();
        Container.Bind<ICarSoundModule>().To<CarEngineSoundModule>().AsCached();
        Container.Bind<ICarSoundModule>().To<CarTireSoundModule>().AsCached();
    }

    private void VFX()
    {
        Container.BindInstance(vfxView).AsSingle();
        Container.BindInstance(view.CarData.vfxSettings).AsSingle();
        Container.Bind<ICarVFXModule>().To<CarLightsVFXModule>().AsCached();
        Container.Bind<ICarVFXModule>().To<CarTireEffectsVFXModule>().AsCached();
    }

    private void SubComponents()
    {
        Container.Bind<IOccupancyHandler>().To<CarOccupancyHandler>().AsSingle();
        Container.Bind<CarCameraHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarUxHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarPhysics>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarVisuals>().AsSingle();
    }
}
