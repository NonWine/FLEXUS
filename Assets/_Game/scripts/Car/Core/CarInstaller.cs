using UnityEngine;
using Zenject;

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
        Signals();
        Physics();
        StateMachine();

        Container.Bind<CarFacade>().AsSingle();
    }

    private void StateMachine()
    {
        Container.BindInterfacesAndSelfTo<CarStateMachine>().AsSingle();
        Container.Bind<IState>().To<EmptyCarState>().AsSingle();
        Container.Bind<IState>().To<EnterCarState>().AsSingle();
        Container.Bind<IState>().To<DrivingCarState>().AsSingle();
        Container.Bind<IState>().To<ExitCarState>().AsSingle();
    }

    private void Signals()
    {
        Container.DeclareSignal<ChangeCarStateSignal>();
        Container.DeclareSignal<CarInteractionSignal>();
        Container.DeclareSignal<CarExitRequestSignal>();
    }

    private void ViewDependencies()
    {
        Container.BindInstance(view).AsSingle();
        Container.BindInstance(view.Rigidbody).AsSingle();
        Container.BindInstance(view.Wheels).AsSingle();
        Container.BindInstance(view.Meshes).AsSingle();
        Container.Bind<Transform>().FromInstance(view.transform).AsSingle();
    }

    private void Sounds()
    {
        Container.BindInstance(soundView).AsSingle();
        Container.BindInterfacesAndSelfTo<CarEngineSoundModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarTireSoundModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarSoundController>().AsSingle().NonLazy();
    }

    private void VFX()
    {
        Container.BindInstance(vfxView).AsSingle();
        Container.BindInterfacesAndSelfTo<CarLightsVFXModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarTireEffectsVFXModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarVFXController>().AsSingle().NonLazy();
    }

    private void SubComponents()
    {
        Container.Bind<IOccupancyHandler>().To<CarOccupancyHandler>().AsSingle();
        Container.Bind<CarCameraHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarInteractionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarUxHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<CarVisuals>().AsSingle();
    }
    
    private void Physics()
    {
        Container.BindInterfacesTo<CarMotorModule>().AsCached();
        Container.Bind<ICarPhysicsModule>().To<CarSteeringModule>().AsCached();
        Container.Bind<ICarPhysicsModule>().To<CarFrictionModule>().AsCached();
        Container.Bind<ICarPhysicsModule>().To<CarStabilizerModule>().AsCached();
        Container.BindInterfacesAndSelfTo<CarPhysics>().AsSingle();
    }
    
}
