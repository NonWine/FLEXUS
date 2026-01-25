using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject interactor);
    void ShowUx();
    void HideUx();
}
