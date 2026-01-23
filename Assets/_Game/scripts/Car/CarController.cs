using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxMotorTorque = 2500f;
    [SerializeField] private float maxSteeringAngle = 35f;
    [SerializeField] private float minSteeringAngle = 12f;
    [SerializeField] private float brakeTorque = 5000f;
    [SerializeField] private float handbrakeTorque = 8000f;
    
    [Header("Friction Settings")]
    [SerializeField] private float normalStiffness = 1.0f;
    [SerializeField] private float driftStiffness = 0.4f;
    
    [Header("Stability")]
    [SerializeField] private float steerHelper = 0.7f;
    [SerializeField] private float accelerationLerp = 5f;
    [SerializeField] private float maxSpeed = 120f;
    [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0.4f);

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftMesh;
    [SerializeField] private Transform frontRightMesh;
    [SerializeField] private Transform rearLeftMesh;
    [SerializeField] private Transform rearRightMesh;

    private float horizontalInput;
    private float verticalInput;
    private bool isHandbraking;
    private Rigidbody rb;

    private float currentMotorTorque;
    private float currentBrakeTorque;

    public bool IsHandbraking => isHandbraking;
    public float CurrentSpeedKmH => rb.linearVelocity.magnitude * 3.6f;
    public float VerticalInput => verticalInput;
    public float CurrentBrakeTorque => currentBrakeTorque;
    public Rigidbody CarRigidbody => rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMassOffset;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        
        SetWheelStiffness(frontLeftCollider, normalStiffness);
        SetWheelStiffness(frontRightCollider, normalStiffness);
        SetWheelStiffness(rearLeftCollider, normalStiffness);
        SetWheelStiffness(rearRightCollider, normalStiffness);
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleHandbrakeFriction();
        ApplySteerHelper();
    }

    private void GetInput()
    {
        horizontalInput = 0;
        verticalInput = 0;
        isHandbraking = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalInput = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalInput = 1;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalInput = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalInput = -1;
            isHandbraking = Keyboard.current.spaceKey.isPressed;
        }
    }

    private void HandleMotor()
    {
        float speedKmH = CurrentSpeedKmH;
        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity) * 3.6f;

        float targetMotorTorque = 0;
        float targetBrakeTorque = 0;

        if (verticalInput > 0.1f)
        {
            if (forwardSpeed < -1f) targetBrakeTorque = brakeTorque;
            else if (speedKmH < maxSpeed) targetMotorTorque = verticalInput * maxMotorTorque;
        }
        else if (verticalInput < -0.1f)
        {
            if (forwardSpeed > 1f) targetBrakeTorque = brakeTorque;
            else if (speedKmH < maxSpeed) targetMotorTorque = verticalInput * maxMotorTorque;
        }

        currentMotorTorque = targetMotorTorque;

        if (Mathf.Abs(targetMotorTorque) > 0.1f)
        {
            currentBrakeTorque = 0;
        }
        else
        {
            currentBrakeTorque = Mathf.Lerp(currentBrakeTorque, targetBrakeTorque, Time.fixedDeltaTime * accelerationLerp);
        }

        frontLeftCollider.motorTorque = currentMotorTorque * 0.3f;
        frontRightCollider.motorTorque = currentMotorTorque * 0.3f;
        rearLeftCollider.motorTorque = currentMotorTorque * 0.7f;
        rearRightCollider.motorTorque = currentMotorTorque * 0.7f;

        if (isHandbraking)
        {
            ApplyBrakeToWheels(0, handbrakeTorque);
        }
        else
        {
            ApplyBrakeToWheels(currentBrakeTorque, currentBrakeTorque);
        }
    }

    private void ApplyBrakeToWheels(float frontBrake, float rearBrake)
    {
        frontLeftCollider.brakeTorque = frontBrake;
        frontRightCollider.brakeTorque = frontBrake;
        rearLeftCollider.brakeTorque = rearBrake;
        rearRightCollider.brakeTorque = rearBrake;
    }

    private void HandleHandbrakeFriction()
    {
        float currentRearStiffness = isHandbraking ? driftStiffness : normalStiffness;
        SetWheelStiffness(rearLeftCollider, currentRearStiffness);
        SetWheelStiffness(rearRightCollider, currentRearStiffness);
        SetWheelStiffness(frontLeftCollider, normalStiffness);
        SetWheelStiffness(frontRightCollider, normalStiffness);
    }

    private void SetWheelStiffness(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void HandleSteering()
    {
        float speedKmH = CurrentSpeedKmH;
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, speedKmH);
        float dynamicSteerAngle = Mathf.Lerp(maxSteeringAngle, minSteeringAngle, speedFactor);

        float targetSteerAngle = horizontalInput * dynamicSteerAngle;
        frontLeftCollider.steerAngle = targetSteerAngle;
        frontRightCollider.steerAngle = targetSteerAngle;
    }

    private void ApplySteerHelper()
    {
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
            Vector3 angularVel = rb.angularVelocity;
            angularVel.y *= (1f - steerHelper * Time.fixedDeltaTime * 10f);
            rb.angularVelocity = angularVel;
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftCollider, frontLeftMesh);
        UpdateSingleWheel(frontRightCollider, frontRightMesh);
        UpdateSingleWheel(rearLeftCollider, rearLeftMesh);
        UpdateSingleWheel(rearRightCollider, rearRightMesh);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelCollider == null || wheelTransform == null) return;
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}