using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class CarFactoryInstaller : MonoInstaller
    {
        [SerializeField] private List<CarDefinition> carDefinitions;

        public override void InstallBindings()
        {
            Container.BindInstance(carDefinitions).AsSingle().WhenInjectedInto<CarFactory>();

            Container.BindFactory<string, Transform, CarFacade, CarFacade.Factory>()
                .FromFactory<CarFactory>();
        }
    }
}
 