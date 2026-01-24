using UnityEngine;
using Zenject;

public class CarInstaller : MonoInstaller
{
    [SerializeField] private CarView view;

    public override void InstallBindings()
    {
        Container.BindInstance(view).AsSingle();
        Container.BindInstance(view.CarData).AsSingle();
        Container.BindInstance(view.Rigidbody).AsSingle();
        Container.BindInstance(view.Wheels).AsSingle();
        Container.BindInstance(view.Meshes).AsSingle();
        
        Container.Bind<Transform>().FromInstance(view.transform).AsSingle();

        Container.BindInterfacesAndSelfTo<CarInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarPhysics>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarBrain>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarVisuals>().AsSingle();

        Container.Bind<CarFacade>().AsSingle();
    }
}
