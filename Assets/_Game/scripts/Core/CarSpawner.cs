using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform carStartPoint;
    [SerializeField, Required] private CarDefinition carDefinition;
    [Inject] private CarFacade.Factory carFactory;
    
    private void Start()
    {
        if (carDefinition == null)
        {
            Debug.LogError("[CarSpawner] CarDefinition is missing");
            return;
        }

        var car = carFactory.Create(carDefinition.CarId, carStartPoint);
        car.Transform.position = carStartPoint.position;
        car.Transform.rotation = carStartPoint.rotation;

    }
}
