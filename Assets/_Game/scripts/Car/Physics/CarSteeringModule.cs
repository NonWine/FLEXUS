using UnityEngine;

public class CarSteeringModule : ICarPhysicsModule
{
    private readonly CarData data;
    private readonly ICarInput input;
    private readonly WheelColliders wheels;
    private readonly Rigidbody rb;

    public CarSteeringModule(Rigidbody rb, CarData data, ICarInput input, WheelColliders wheels)
    {
        this.rb = rb;
        this.data = data;
        this.input = input;
        this.wheels = wheels;
    }

    public void Initialize() { }

    public void OnFixedTick()
    {
        float currentSpeedKmH = rb.linearVelocity.magnitude * 3.6f;
        float speedFactor = currentSpeedKmH / data.maxSpeed;
        float dynamicSteerAngle = Mathf.Lerp(data.maxSteeringAngle, data.minSteeringAngle, speedFactor);
        float targetSteerAngle = input.Steer * dynamicSteerAngle;
        
        wheels.frontLeft.steerAngle = targetSteerAngle;
        wheels.frontRight.steerAngle = targetSteerAngle;
    }
}
