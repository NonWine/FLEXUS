using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputHandler : IInitializable, IDisposable , IPlayerInput
{
    private readonly InputActionAsset inputActions;
    private readonly PlayerData data;

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction interactAction;

    private bool isActive = true;

    public Vector2 MoveInput => isActive ? (moveAction?.ReadValue<Vector2>() ?? Vector2.zero) : Vector2.zero;
    public bool IsSprinting => isActive && (sprintAction?.IsPressed() ?? false);
    
    public event Action OnInteract;

    public PlayerInputHandler(InputActionAsset inputActions, PlayerData data)
    {
        this.inputActions = inputActions;
        this.data = data;
    }

    public void Initialize()
    {
        var map = inputActions.FindActionMap(data.mapName);
        moveAction = map.FindAction(data.moveActionName);
        sprintAction = map.FindAction(data.sprintActionName);
        interactAction = map.FindAction(data.interactActionName);
        interactAction.performed += HandleInteract;
        map.Enable();
    }


    public void Dispose()
    {
        interactAction.performed -= HandleInteract;
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (isActive) OnInteract?.Invoke();
    }
}
