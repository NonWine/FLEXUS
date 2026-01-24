using UnityEngine;
using Zenject;
using Unity.Cinemachine;

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
    }

    private void Sounds()
    {
        Container.BindInstance(soundView).AsSingle();
        Container.BindInstance(view.CarData.soundSettings).AsSingle();
        Container.Bind<ICarSoundModule>().To<CarEngineSoundModule>().AsCached();
        Container.Bind<ICarSoundModule>().To<CarTireSoundModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarSoundController>().AsSingle();
    }

    private void VFX()
    {
        Container.BindInstance(vfxView).AsSingle();
        Container.BindInstance(view.CarData.vfxSettings).AsSingle();
        Container.Bind<ICarVFXModule>().To<CarLightsVFXModule>().AsCached();
        Container.Bind<ICarVFXModule>().To<CarTireEffectsVFXModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarVFXController>().AsSingle();
    }

    private void SubComponents()
    {
        Container.Bind<IOccupancyHandler>().To<CarOccupancyHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarPhysics>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarBrain>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarVisuals>().AsSingle();
    }
}
