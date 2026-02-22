using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CarFactory : IFactory<string, Transform, CarFacade>
{
    private readonly DiContainer _container;
    private readonly Dictionary<string, CarDefinition> _definitions;

    public CarFactory(DiContainer container, List<CarDefinition> definitions)
    {
        _container = container;
        _definitions = new Dictionary<string, CarDefinition>();

        foreach (var definition in definitions.Where(x => x != null))
        {
            if (string.IsNullOrWhiteSpace(definition.CarId))
            {
                definition.RegenerateID();
            }

            if (definition.Prefab == null)
            {
                Debug.LogError($"[CarFactory] Definition '{definition.name}' has no prefab assigned");
                continue;
            }
            
            _definitions.Add(definition.CarId, definition);
        }
    }

    public CarFacade Create(string id, Transform position)
    {
        if (!_definitions.TryGetValue(id, out var definition))
        {
            Debug.LogError($"[CarFactory] Prefab for type '{id}' is missing in Installer!");
            return null;
        }

        GameObject instance = _container.InstantiatePrefab(definition.Prefab.gameObject, position);
        var context = instance.GetComponent<GameObjectContext>();
        var facade = context.Container.Resolve<CarFacade>();
        facade.SetIdentity(id);
        return facade;
    }
}
