using UnityEngine;
using Zenject;

public class PlayerFacade
{

    private Transform transform;
    
    public Transform Transform => transform;

    [Inject]
    public void Construct(Transform transform)
    {
        this.transform = transform;
    }
    

    public class Factory : PlaceholderFactory<PlayerFacade> { }
}
