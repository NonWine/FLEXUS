using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputBase : ICarInput
{
    protected readonly InputActionAsset inputActions;
    protected readonly CarData data;

    protected InputAction moveAction;
    protected InputAction handbrakeAction;

    protected bool isEnabled;

    public float Throttle => isEnabled ? (moveAction?.ReadValue<Vector2>().y ?? 0f) : 0f;
    public float Steer => isEnabled ? (moveAction?.ReadValue<Vector2>().x ?? 0f) : 0f;
    public float Brake => isEnabled ? 0f : 1f;
    public bool IsHandbraking => isEnabled && (handbrakeAction?.IsPressed() ?? false);

    public CarInputBase(InputActionAsset inputActions, CarData data)
    {
        this.inputActions = inputActions;
        this.data = data;
    }

    public virtual void Initialize()
    {
        var carMap = inputActions.FindActionMap(data.mapName);
        moveAction = carMap.FindAction(data.moveActionName);
        handbrakeAction = carMap.FindAction(data.handbrakeActionName);
    }
}
