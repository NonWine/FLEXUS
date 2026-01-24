using UnityEngine;
using Zenject;

public class CarPhysics : IFixedTickable
{
    private const float MS_TO_KMH = 3.6f;
    private const float INPUT_THRESHOLD = 0.1f;
    private const float REVERSE_SPEED_THRESHOLD = -1f;
    private const float FORWARD_SPEED_THRESHOLD = 1f;
    private const float STEER_HELPER_MULTIPLIER = 10f;
    private const float FRONT_TORQUE_SHARE = 0.3f;
    private const float REAR_TORQUE_SHARE = 0.7f;

    private readonly Rigidbody rb;
    private readonly CarData data;
    private readonly ICarInput input;
    private readonly WheelColliders wheels;
 
    public float CurrentSpeedKmH => rb.linearVelocity.magnitude * MS_TO_KMH;
    public float CurrentBrakeTorque { get; private set; }
    public Rigidbody Rigidbody => rb;

    public CarPhysics(Rigidbody rb, CarData data, ICarInput input, WheelColliders wheels)
    {
        this.rb = rb;
        this.data = data;
        this.input = input;
        this.wheels = wheels;
        
        this.rb.centerOfMass = this.data.centerOfMassOffset;
        SetInitialFriction();
    }

    private void SetInitialFriction()
    {
        SetWheelStiffness(wheels.frontLeft, data.normalStiffness);
        SetWheelStiffness(wheels.frontRight, data.normalStiffness);
        SetWheelStiffness(wheels.rearLeft, data.normalStiffness);
        SetWheelStiffness(wheels.rearRight, data.normalStiffness);
    }

    public void FixedTick()
    {
        HandleMotor();
        HandleSteering();
        HandleHandbrakeFriction();
        ApplySteerHelper();
    }

    private void HandleMotor()
    {
        float forwardSpeed = Vector3.Dot(rb.transform.forward, rb.linearVelocity) * MS_TO_KMH;
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

        wheels.frontLeft.motorTorque = targetMotorTorque * FRONT_TORQUE_SHARE;
        wheels.frontRight.motorTorque = targetMotorTorque * FRONT_TORQUE_SHARE;
        wheels.rearLeft.motorTorque = targetMotorTorque * REAR_TORQUE_SHARE;
        wheels.rearRight.motorTorque = targetMotorTorque * REAR_TORQUE_SHARE;

        float rearBrake = input.IsHandbraking ? data.handbrakeTorque : CurrentBrakeTorque;
        wheels.frontLeft.brakeTorque = CurrentBrakeTorque;
        wheels.frontRight.brakeTorque = CurrentBrakeTorque;
        wheels.rearLeft.brakeTorque = rearBrake;
        wheels.rearRight.brakeTorque = rearBrake;
    }

    private void HandleHandbrakeFriction()
    {
        float currentRearStiffness = input.IsHandbraking ? data.driftStiffness : data.normalStiffness;
        SetWheelStiffness(wheels.rearLeft, currentRearStiffness);
        SetWheelStiffness(wheels.rearRight, currentRearStiffness);
    }

    private void SetWheelStiffness(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void HandleSteering()
    {
        float speedFactor = CurrentSpeedKmH / data.maxSpeed;
        float dynamicSteerAngle = Mathf.Lerp(data.maxSteeringAngle, data.minSteeringAngle, speedFactor);
        float targetSteerAngle = input.Steer * dynamicSteerAngle;
        
        wheels.frontLeft.steerAngle = targetSteerAngle;
        wheels.frontRight.steerAngle = targetSteerAngle;
    }

    private void ApplySteerHelper()
    {
        if (Mathf.Abs(input.Steer) < INPUT_THRESHOLD)
        {
            Vector3 angularVel = rb.angularVelocity;
            angularVel.y *= (1f - data.steerHelper * Time.fixedDeltaTime * STEER_HELPER_MULTIPLIER);
            rb.angularVelocity = angularVel;
        }
    }
}
