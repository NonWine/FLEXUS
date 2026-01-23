using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Infrastructure;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour, IInteractable
{
    private const float MS_TO_KMH = 3.6f;
    private const int ACTIVE_CAMERA_PRIORITY = 100;
    private const int INACTIVE_CAMERA_PRIORITY = 0;
    private const float GROUND_CHECK_DISTANCE = 5f;
    private const float GROUND_OFFSET = 0.05f;
    private const float STEER_HELPER_MULTIPLIER = 10f;
    private const float FRONT_WHEEL_TORQUE_SHARE = 0.3f;
    private const float REAR_WHEEL_TORQUE_SHARE = 0.7f;

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

    [Header("Interaction & Camera")]
    [SerializeField] private CinemachineCamera carCamera;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float maxExitSpeedKmH = 15f;

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

    private GameStateController _gameStateController;
    private InputAction _moveAction;
    private InputAction _handbrakeAction;
    private InputAction _exitAction;
    private GameObject _currentPlayer;

    public bool IsHandbraking => isHandbraking;
    public float CurrentSpeedKmH => rb != null ? rb.linearVelocity.magnitude * MS_TO_KMH : 0f;
    public float VerticalInput => verticalInput;
    public float CurrentBrakeTorque => currentBrakeTorque;
    public Rigidbody CarRigidbody => rb;
    public string InteractionPrompt => "Press E to Drive";

    [Inject]
    public void Construct(GameStateController gameStateController, InputActionAsset inputActions)
    {
        _gameStateController = gameStateController;
        var carMap = inputActions.FindActionMap("Car");
        if (carMap != null)
        {
            _moveAction = carMap.FindAction("Move");
            _handbrakeAction = carMap.FindAction("Handbrake");
            _exitAction = carMap.FindAction("Exit");
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMassOffset;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        SetWheelStiffness(frontLeftCollider, normalStiffness);
        SetWheelStiffness(frontRightCollider, normalStiffness);
        SetWheelStiffness(rearLeftCollider, normalStiffness);
        SetWheelStiffness(rearRightCollider, normalStiffness);

        if (carCamera != null) carCamera.Priority = INACTIVE_CAMERA_PRIORITY;
        enabled = false;
    }

    public void Interact(GameObject interactor)
    {
        EnterCar(interactor);
    }

    private void EnterCar(GameObject interactor)
    {
        _currentPlayer = interactor;
        
        if (_currentPlayer != null)
        {
            _currentPlayer.transform.SetParent(transform);
            _currentPlayer.transform.localPosition = Vector3.zero;
            _currentPlayer.SetActive(false);
        }

        if (carCamera != null) carCamera.Priority = ACTIVE_CAMERA_PRIORITY;
        
        enabled = true;
        _gameStateController.SetState(GameState.Car);
        
        if (_exitAction != null)
            _exitAction.performed += OnExitPerformed;
    }

    private void OnExitPerformed(InputAction.CallbackContext context)
    {
        if (CurrentSpeedKmH > maxExitSpeedKmH)
        {
            return;
        }
        ExitCar();
    }

    private void ExitCar()
    {
        if (_exitAction != null)
            _exitAction.performed -= OnExitPerformed;
        
        if (_currentPlayer != null)
        {
            _currentPlayer.transform.SetParent(null);
            
            Vector3 targetPosition = exitPoint.position;
            if (Physics.Raycast(exitPoint.position + Vector3.up, Vector3.down, out RaycastHit hit, GROUND_CHECK_DISTANCE))
            {
                targetPosition = hit.point + Vector3.up * GROUND_OFFSET;
            }

            var controller = _currentPlayer.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            Vector3 oldPosition = _currentPlayer.transform.position;
            _currentPlayer.transform.position = targetPosition;
            _currentPlayer.transform.rotation = exitPoint.rotation;

            var playerVcam = _currentPlayer.GetComponentInChildren<CinemachineCamera>();
            if (playerVcam != null)
            {
                playerVcam.OnTargetObjectWarped(_currentPlayer.transform, targetPosition - oldPosition);
            }
            
            _currentPlayer.SetActive(true);
            
            if (controller != null) controller.enabled = true;
        }

        if (carCamera != null) carCamera.Priority = INACTIVE_CAMERA_PRIORITY;
        enabled = false;
        
        horizontalInput = 0;
        verticalInput = 0;
        isHandbraking = false;
        ApplyBrakeToWheels(brakeTorque, brakeTorque);
        
        _gameStateController.SetState(GameState.Player);
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
        if (_moveAction == null || _handbrakeAction == null) return;

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;
        isHandbraking = _handbrakeAction.IsPressed();
    }

    private void HandleMotor()
    {
        if (rb == null) return;

        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity) * MS_TO_KMH;
        float targetMotorTorque = 0;
        float targetBrakeTorque = 0;

        if (verticalInput > 0.1f)
        {
            if (forwardSpeed < -1f) targetBrakeTorque = brakeTorque;
            else if (Mathf.Abs(forwardSpeed) < maxSpeed) targetMotorTorque = verticalInput * maxMotorTorque;
        }
        else if (verticalInput < -0.1f)
        {
            if (forwardSpeed > 1f) targetBrakeTorque = brakeTorque;
            else if (Mathf.Abs(forwardSpeed) < maxSpeed) targetMotorTorque = verticalInput * maxMotorTorque;
        }

        currentMotorTorque = targetMotorTorque;
        currentBrakeTorque = (Mathf.Abs(targetMotorTorque) > 0.1f) ? 0 : Mathf.Lerp(currentBrakeTorque, targetBrakeTorque, Time.fixedDeltaTime * accelerationLerp);

        if (frontLeftCollider) frontLeftCollider.motorTorque = currentMotorTorque * FRONT_WHEEL_TORQUE_SHARE;
        if (frontRightCollider) frontRightCollider.motorTorque = currentMotorTorque * FRONT_WHEEL_TORQUE_SHARE;
        if (rearLeftCollider) rearLeftCollider.motorTorque = currentMotorTorque * REAR_WHEEL_TORQUE_SHARE;
        if (rearRightCollider) rearRightCollider.motorTorque = currentMotorTorque * REAR_WHEEL_TORQUE_SHARE;

        ApplyBrakeToWheels(isHandbraking ? 0 : currentBrakeTorque, isHandbraking ? handbrakeTorque : currentBrakeTorque);
    }

    private void ApplyBrakeToWheels(float frontBrake, float rearBrake)
    {
        if (frontLeftCollider) frontLeftCollider.brakeTorque = frontBrake;
        if (frontRightCollider) frontRightCollider.brakeTorque = frontBrake;
        if (rearLeftCollider) rearLeftCollider.brakeTorque = rearBrake;
        if (rearRightCollider) rearRightCollider.brakeTorque = rearBrake;
    }

    private void HandleHandbrakeFriction()
    {
        float currentRearStiffness = isHandbraking ? driftStiffness : normalStiffness;
        if (rearLeftCollider) SetWheelStiffness(rearLeftCollider, currentRearStiffness);
        if (rearRightCollider) SetWheelStiffness(rearRightCollider, currentRearStiffness);
    }

    private void SetWheelStiffness(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = stiffness;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void HandleSteering()
    {
        if (rb == null) return;
        float speedKmH = rb.linearVelocity.magnitude * MS_TO_KMH;
        float dynamicSteerAngle = Mathf.Lerp(maxSteeringAngle, minSteeringAngle, speedKmH / maxSpeed);
        float targetSteerAngle = horizontalInput * dynamicSteerAngle;
        if (frontLeftCollider) frontLeftCollider.steerAngle = targetSteerAngle;
        if (frontRightCollider) frontRightCollider.steerAngle = targetSteerAngle;
    }

    private void ApplySteerHelper()
    {
        if (rb == null) return;
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
            Vector3 angularVel = rb.angularVelocity;
            angularVel.y *= (1f - steerHelper * Time.fixedDeltaTime * STEER_HELPER_MULTIPLIER);
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
        Vector3 pos; Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
