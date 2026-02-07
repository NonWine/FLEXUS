using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputActionAsset inputActions;
        
        public override void InstallBindings()
        {
            Container.BindInstance(inputActions).AsSingle();
            
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<VehicleOccupiedSignal>(); 
            
            Container.BindInterfacesAndSelfTo<GameStateController>()
                .AsSingle()
                .NonLazy();

            Container.BindInitializableExecutionOrder<GameStateController>(-1000);
        
            
        }
    }
}
