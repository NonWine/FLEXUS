using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class CarFactoryInstaller : MonoInstaller
    {
        [SerializeField] private GameObject carPrefab;
        
        public override void InstallBindings()
        {
            Container.BindFactory<CarFacade, CarFacade.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(carPrefab)
                .AsSingle();
        }
    }
}