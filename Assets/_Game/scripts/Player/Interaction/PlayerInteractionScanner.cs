using System;
using UnityEngine;
using Zenject;

public class PlayerInteractionScanner : ITickable, IInteractionScanner
{
    private readonly PlayerView view;
    private readonly PlayerData data;
    
    private readonly Collider[] hitColliders = new Collider[5];
    private IInteractable currentInteractable;

    public IInteractable CurrentInteractable => currentInteractable;
    public event Action<IInteractable> OnInteractableChanged;

    public PlayerInteractionScanner(PlayerView view, PlayerData data)
    {
        this.view = view;
        this.data = data;
    }

    public void Tick()
    {
        Scan();
    }

    private void Scan()
    {
        int hits = Physics.OverlapSphereNonAlloc(
            view.InteractionSource.position, 
            data.interactionRadius, 
            hitColliders, 
            data.interactionLayer
        );

        IInteractable bestTarget = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3 currentPos = view.InteractionSource.position;

        for (int i = 0; i < hits; i++)
        {
            var interactable = hitColliders[i].GetComponentInParent<IInteractable>();
            if (interactable != null && interactable is MonoBehaviour mono)
            {
                float distSqr = (mono.transform.position - currentPos).sqrMagnitude;
                if (distSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distSqr;
                    bestTarget = interactable;
                }
            }
        }

        if (currentInteractable != bestTarget)
        {
            currentInteractable?.HideUx();
            currentInteractable = bestTarget;
            currentInteractable?.ShowUx();
            OnInteractableChanged?.Invoke(currentInteractable);
        }
    }
}
