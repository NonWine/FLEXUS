using UnityEngine;
using Unity.Cinemachine;
using System;

public class CarView : MonoBehaviour, IInteractable
{
    [field:SerializeField]  public Transform ExitPoint { get; private set; }
    [field:SerializeField]  public Rigidbody Rigidbody { get; private set; }
    [field:SerializeField]  public WheelColliders Wheels { get; private set; }
    [field:SerializeField]  public WheelMeshes Meshes { get; private set; }
    [field: SerializeField] public Outline Outline { get; private set; }
    
    public event Action<GameObject> OnInteractedEvent;
    public event Action OnShowUxEvent;
    public event Action OnHideUxEvent;

    
    public void Interact(GameObject interactor)
    {
        OnInteractedEvent?.Invoke(interactor);
    }

    public void ShowUx()
    {
        OnShowUxEvent?.Invoke();
    }
    
    public void HideUx()
    {
        OnHideUxEvent?.Invoke();
    }
}
