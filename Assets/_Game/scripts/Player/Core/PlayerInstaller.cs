using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerView view;

    public override void InstallBindings()
    {
        Container.BindInstance(view).AsSingle();
        Container.BindInstance(view.PlayerData).AsSingle();
        Container.BindInstance(view.transform);

        Container.BindInterfacesAndSelfTo<PlayerInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerCameraHandler>().AsSingle();
        Interaction();

        Container.Bind<PlayerFacade>().FromNew().AsSingle();
    }

    private void Interaction()
    {
        Container.BindInterfacesAndSelfTo<PlayerInteractionScanner>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInteraction>().AsSingle();
    }
}
