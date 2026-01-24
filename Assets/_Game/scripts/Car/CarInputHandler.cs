using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System;

public class CarInputHandler : CarInputBase, IInitializable, IDisposable, ICarInputHandler
{
    private InputAction exitAction;

    public event Action OnExitPerformed;

    public CarInputHandler(InputActionAsset inputActions, CarData data) : base(inputActions, data)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        
        var carMap = inputActions.FindActionMap(data.mapName);
        exitAction = carMap.FindAction(data.exitActionName);
        Disable();
    }

    public void Enable()
    {
        if (isEnabled) return;
        isEnabled = true;

        exitAction.performed += HandleExit;
    }

    public void Disable()
    {
        if (!isEnabled)
        {
            exitAction.performed -= HandleExit;
            isEnabled = false;
            return;
        }

        isEnabled = false;
        if (exitAction != null)
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
