using System;
using UnityEngine;
using Zenject;

public class PlayerInteraction : IInitializable, IDisposable
{
    private readonly PlayerView view;
    private readonly IPlayerInput input;
    private readonly IInteractionScanner scanner;

    public PlayerInteraction(PlayerView view, IPlayerInput input, IInteractionScanner scanner)
    {
        this.view = view;
        this.input = input;
        this.scanner = scanner;
    }

    public void Initialize()
    {
        input.OnInteract += TryInteract;
    }

    public void Dispose()
    {
        input.OnInteract -= TryInteract;
    }

    private void TryInteract()
    {
        var target = scanner.CurrentInteractable;
        if (target != null)
        {
            target.Interact(view.gameObject);
        }
    }
}
