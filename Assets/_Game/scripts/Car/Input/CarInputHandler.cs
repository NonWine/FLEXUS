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
        var carMap = inputActions.FindActionMap(data.inputSettings.mapName);
        exitAction = carMap.FindAction(data.inputSettings.exitActionName);
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
