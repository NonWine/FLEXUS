using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private string interactActionName = "Interact";
    [SerializeField] private LayerMask interactableLayer;
    
    private InputAction _interactAction;
    private InputActionAsset _inputActions;
    
    private readonly List<IInteractable> _availableInteractables = new List<IInteractable>();
    private IInteractable _closestInteractable;

    [Inject]
    public void Construct(InputActionAsset inputActions)
    {
        _inputActions = inputActions;
        var map = _inputActions.FindActionMap("Player");
        _interactAction = map?.FindAction(interactActionName);
    }

    private void OnEnable() => _interactAction.performed += OnInteract;
    private void OnDisable() => _interactAction.performed -= OnInteract;

    private void Update() => UpdateClosestInteractable();

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_closestInteractable != null)
        {
            _closestInteractable.Interact(gameObject);
        }
    }

    private void UpdateClosestInteractable()
    {
        if (_availableInteractables.Count == 0)
        {
            _closestInteractable = null;
            return;
        }

        IInteractable bestTarget = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3 currentPos = transform.position;

        for (int i = _availableInteractables.Count - 1; i >= 0; i--)
        {
            var interactable = _availableInteractables[i];
            if (interactable is MonoBehaviour mono && mono != null)
            {
                float distSqr = (mono.transform.position - currentPos).sqrMagnitude;
                if (distSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distSqr;
                    bestTarget = interactable;
                }
            }
            else
            {
                _availableInteractables.RemoveAt(i);
            }
        }
        _closestInteractable = bestTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            var interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null && !_availableInteractables.Contains(interactable))
            {
                _availableInteractables.Add(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            var interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                _availableInteractables.Remove(interactable);
            }
        }
    }
}
