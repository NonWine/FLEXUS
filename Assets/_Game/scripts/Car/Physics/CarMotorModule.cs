using UnityEngine;

public class CarMotorModule : ICarPhysicsModule, ICarMotor
{
    private const float INPUT_THRESHOLD = 0.1f;
    private const float REVERSE_SPEED_THRESHOLD = -1f;
    private const float FORWARD_SPEED_THRESHOLD = 1f;

    private readonly Rigidbody rb;
    private readonly CarData data;
    private readonly ICarInput input;
    private readonly WheelColliders wheels;

    public float CurrentBrakeTorque { get; private set; }

    public CarMotorModule(Rigidbody rb, CarData data, ICarInput input, WheelColliders wheels)
    {
        this.rb = rb;
        this.data = data;
        this.input = input;
        this.wheels = wheels;
    }

    public void Initialize() { }

    public void OnFixedTick()
    {
        float forwardSpeed = Vector3.Dot(rb.transform.forward, rb.linearVelocity) * 3.6f;
        float targetMotorTorque = 0;
        float targetBrakeTorque = input.Brake * data.brakeTorque;

        if (input.Throttle > INPUT_THRESHOLD)
        {
            if (forwardSpeed < REVERSE_SPEED_THRESHOLD) targetBrakeTorque = data.brakeTorque;
            else if (Mathf.Abs(forwardSpeed) < data.maxSpeed) targetMotorTorque = input.Throttle * data.maxMotorTorque;
        }
        else if (input.Throttle < -INPUT_THRESHOLD)
        {
            if (forwardSpeed > FORWARD_SPEED_THRESHOLD) targetBrakeTorque = data.brakeTorque;
            else if (Mathf.Abs(forwardSpeed) < data.maxSpeed) targetMotorTorque = input.Throttle * data.maxMotorTorque;
        }

        CurrentBrakeTorque = (Mathf.Abs(targetMotorTorque) > INPUT_THRESHOLD) ? 0 : Mathf.Lerp(CurrentBrakeTorque, targetBrakeTorque, Time.fixedDeltaTime * data.accelerationLerp);

        ApplyDriveTorque(targetMotorTorque);
        ApplyBrakeTorque();
    }

    private void ApplyDriveTorque(float totalTorque)
    {
        CarPhysicsUtils.CalculateTorqueDistribution(data.driveType, out float frontShare, out float rearShare);
        wheels.frontLeft.motorTorque = (totalTorque * frontShare) / 2f;
        wheels.frontRight.motorTorque = (totalTorque * frontShare) / 2f;
        wheels.rearLeft.motorTorque = (totalTorque * rearShare) / 2f;
        wheels.rearRight.motorTorque = (totalTorque * rearShare) / 2f;
    }

    private void ApplyBrakeTorque()
    {
        float rearBrake = input.IsHandbraking ? data.handbrakeTorque : CurrentBrakeTorque;
        wheels.frontLeft.brakeTorque = CurrentBrakeTorque;
        wheels.frontRight.brakeTorque = CurrentBrakeTorque;
        wheels.rearLeft.brakeTorque = rearBrake;
        wheels.rearRight.brakeTorque = rearBrake;
    }
}
