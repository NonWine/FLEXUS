using UnityEngine;
using Zenject;

public class CarFacade
{
    private readonly Transform transform;
    private readonly CarStateMachine stateMachine;

    public Transform Transform => transform;
 
    public string CarId { get; private set; }

    public CarFacade(Transform transform, CarStateMachine carStateMachine)
    {
        this.transform = transform;
        stateMachine = carStateMachine;
        carStateMachine.ChangeState<EmptyCarState>();
    }

    public void SetIdentity(string carId)
    {
        CarId = carId;
    }
    
    public class Factory : PlaceholderFactory<string,Transform,CarFacade> { }
}
