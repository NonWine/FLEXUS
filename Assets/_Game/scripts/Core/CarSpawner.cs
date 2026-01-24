using System;
using UnityEngine;
using Zenject;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform carStartPoint;
    [Inject] private CarFacade.Factory carFactory;
    
    
    private void Start()
    {
        var car = carFactory.Create();
        car.Transform.position = carStartPoint.position;
        
    }
}
