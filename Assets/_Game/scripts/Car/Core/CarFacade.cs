using UnityEngine;
using Zenject;

public class CarFacade
{
    private readonly Transform transform;

    public Transform Transform => transform;

    public CarFacade(Transform transform)
    {
        this.transform = transform;
    }
    
    public class Factory : PlaceholderFactory<string,Transform,CarFacade> { }
}