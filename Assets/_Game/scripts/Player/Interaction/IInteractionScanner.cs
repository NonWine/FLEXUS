using System;

public interface IInteractionScanner
{
    IInteractable CurrentInteractable { get; }
    event Action<IInteractable> OnInteractableChanged;
}
