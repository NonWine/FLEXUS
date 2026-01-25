using System;
using UnityEngine;
using Zenject;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform carStartPoint;
    [SerializeField] private CarData carData;
    [Inject] private CarFacade.Factory carFactory;
    
    private void Start()
    {
        var car = carFactory.Create(carData.CarDataId, carStartPoint);
        
        car.Transform.position = carStartPoint.position;
        car.Transform.rotation = carStartPoint.rotation;

    }
}
