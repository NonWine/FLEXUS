using UnityEngine;
using Zenject;

public class CarFacade
{
    private readonly Transform transform;
    private readonly CarStateMachine stateMachine;

    public Transform Transform => transform;

    public CarFacade(Transform transform, CarStateMachine carStateMachine)
    {
        this.transform = transform;
        carStateMachine.ChangeState<CarEmptyState>();
    }
    
    public class Factory : PlaceholderFactory<string,Transform,CarFacade> { }
}