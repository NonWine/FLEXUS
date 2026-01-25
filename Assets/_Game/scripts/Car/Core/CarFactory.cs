using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CarFactory : IFactory<string, Transform, CarFacade>
{
    private readonly DiContainer _container;
    private readonly Dictionary<string, GameObject> _prefabs;

    public CarFactory(DiContainer container, List<CarView> presets)
    {
        _container = container;
        _prefabs = presets.ToDictionary(x => x.CarData.CarDataId, x => x.gameObject);
    }

    public CarFacade Create(string id, Transform position)
    {
        if (!_prefabs.TryGetValue(id, out var prefab))
        {
            Debug.LogError($"[CarFactory] Prefab for type '{id}' is missing in Installer!");
            return null;
        }
        
        GameObject instance = _container.InstantiatePrefab(prefab, position);
        var context = instance.GetComponent<GameObjectContext>();
        return context.Container.Resolve<CarFacade>();
    }
}


