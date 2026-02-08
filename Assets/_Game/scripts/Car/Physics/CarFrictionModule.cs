using UnityEngine;

public class CarFrictionModule : ICarPhysicsModule
{
    private readonly CarData data;
    private readonly ICarInput input;
    private readonly WheelColliders wheels;

    public CarFrictionModule(CarData data, ICarInput input, WheelColliders wheels)
    {
        this.data = data;
        this.input = input;
        this.wheels = wheels;
    }

    public void Initialize()
    {
        SetInitialFriction();
    }

    public void OnFixedTick()
    {
        float currentRearStiffness = input.IsHandbraking ? data.driftStiffness : data.normalStiffness;
        SetWheelStiffness(wheels.rearLeft, currentRearStiffness);
        SetWheelStiffness(wheels.rearRight, currentRearStiffness);
    }

    private void SetInitialFriction()
    {
        SetWheelStiffness(wheels.frontLeft, data.normalStiffness);
        SetWheelStiffness(wheels.frontRight, data.normalStiffness);
        SetWheelStiffness(wheels.rearLeft, data.normalStiffness);
        SetWheelStiffness(wheels.rearRight, data.normalStiffness);
    }

    private void SetWheelStiffness(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }
}
