using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string sprintActionName = "Sprint";

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private CharacterController _characterController;
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private Transform _mainCameraTransform;

    private Vector3 _velocity;
    private float _currentRotationVelocity;
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }

        var playerActionMap = inputActions.FindActionMap("Player"); 
        _moveAction = playerActionMap.FindAction(moveActionName);
        _sprintAction = playerActionMap.FindAction(sprintActionName);
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _sprintAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _sprintAction.Disable();
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        bool isSprinting = _sprintAction.IsPressed();

        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentRotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
            _characterController.Move(moveDir.normalized * (currentSpeed * Time.deltaTime));

            float speedPercent = isSprinting ? 1f : 0.5f;
            animator.SetFloat(SpeedHash, speedPercent, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(SpeedHash, 0f, 0.1f, Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; 
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
