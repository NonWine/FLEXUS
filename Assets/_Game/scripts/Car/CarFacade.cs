using UnityEngine;
using Zenject;

public class CarFacade
{
    private readonly CarBrain brain;
    private readonly Transform transform;

    public Transform Transform => transform;

    public CarFacade(CarBrain brain, Transform transform)
    {
        this.brain = brain;
        this.transform = transform;
    }

    public void Interact(GameObject interactor)
    {
        brain.EnterCar(interactor);
    }
    

    public class Factory : PlaceholderFactory<CarFacade> { }
}
