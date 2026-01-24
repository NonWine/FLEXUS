using UnityEngine;
using Zenject;
using Unity.Cinemachine;

public class CarInstaller : MonoInstaller
{
    [SerializeField] private CarView view;
    [SerializeField] private CarSoundView soundView;

    public override void InstallBindings()
    {
        // Базові залежності
        Container.BindInstance(view).AsSingle();
        Container.BindInstance(view.CarData).AsSingle();
        Container.BindInstance(view.Rigidbody).AsSingle();
        Container.BindInstance(view.Wheels).AsSingle();
        Container.BindInstance(view.Meshes).AsSingle();
        

        Container.Bind<Transform>().FromInstance(view.transform).AsSingle();

        // Sound Bindings
        Container.BindInstance(soundView).AsSingle();
        Container.BindInstance(view.CarData.soundSettings).AsSingle();
        Container.Bind<ICarSoundModule>().To<CarEngineSoundModule>().AsCached();
        Container.Bind<ICarSoundModule>().To<CarTireSoundModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarSoundController>().AsSingle();

        // Infrastructure
        Container.BindInterfacesAndSelfTo<CarInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarPhysics>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarBrain>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarVisuals>().AsSingle();

        Container.Bind<CarFacade>().AsSingle();
    }
}
