using UnityEngine;
using Unity.Cinemachine;
using System;

public class CarView : MonoBehaviour, IInteractable
{
    [field:SerializeField] public CarData CarData { get; private set; }
    [field:SerializeField]  public Transform ExitPoint { get; private set; }
    [field:SerializeField]  public Rigidbody Rigidbody { get; private set; }
    [field:SerializeField]  public WheelColliders Wheels { get; private set; }
    [field:SerializeField]  public WheelMeshes Meshes { get; private set; }

    public event Action<GameObject> OnInteracted;
    
    public void Interact(GameObject interactor)
    {
        OnInteracted?.Invoke(interactor);
    }
}
