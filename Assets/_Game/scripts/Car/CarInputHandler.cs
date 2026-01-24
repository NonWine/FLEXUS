using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System;

public class CarInputHandler : IInitializable, IDisposable, ICarInput
{
    private readonly InputActionAsset inputActions;
    private readonly CarData data;

    private InputAction moveAction;
    private InputAction handbrakeAction;
    private InputAction exitAction;

    private bool enabled;

    public float Throttle => enabled ? (moveAction?.ReadValue<Vector2>().y ?? 0f) : 0f;
    public float Steer => enabled ? (moveAction?.ReadValue<Vector2>().x ?? 0f) : 0f;
    public float Brake => enabled ? 0f : 1f;
    public bool IsHandbraking => enabled && (handbrakeAction?.IsPressed() ?? false);

    public event Action OnExitPerformed;

    public CarInputHandler(InputActionAsset inputActions, CarData data)
    {
        this.inputActions = inputActions;
        this.data = data;
    }

    public void Initialize()
    {
        var carMap = inputActions.FindActionMap(data.mapName);
        moveAction = carMap.FindAction(data.moveActionName);
        handbrakeAction = carMap.FindAction(data.handbrakeActionName);
        exitAction = carMap.FindAction(data.exitActionName);
        Disable();
    }

    public void Enable()
    {
        if (enabled) return;
        enabled = true;

        exitAction.performed += HandleExit;
    }

    public void Disable()
    {
        if (!enabled)
        {
            exitAction.performed -= HandleExit;
            enabled = false;
            return;
        }

        enabled = false;
        exitAction.performed -= HandleExit;
    }

    public void Dispose()
    {
        Disable();
    }

    private void HandleExit(InputAction.CallbackContext context)
    {
        OnExitPerformed?.Invoke();
    }
}
