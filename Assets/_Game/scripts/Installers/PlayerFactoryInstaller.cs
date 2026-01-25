using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class PlayerFactoryInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<PlayerFacade, PlayerFacade.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(playerPrefab)
                .AsSingle();
        }
    }
}
