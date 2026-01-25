using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerMovement : ITickable , IPlayerMovement
{
    private readonly PlayerView view;
    private readonly IPlayerInput input;
    private readonly PlayerData data;
    private readonly CinemachineCamera camera;
    
    private Vector3 verticalVelocity;
    private float currentRotationVelocity;

    public Vector3 CurrentVelocity => view.CharacterController.velocity;

    public PlayerMovement(PlayerView view, IPlayerInput input, PlayerData data, [Inject(Id = "Player")] CinemachineCamera camera)
    {
        this.view = view;
        this.input = input;
        this.data = data;
        this.camera = camera;
    }

    public void Tick()
    {
        HandleGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = this.input.MoveInput;
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(view.transform.eulerAngles.y, targetAngle, ref currentRotationVelocity, data.rotationSmoothTime);
            view.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float targetSpeed = this.input.IsSprinting ? data.sprintSpeed : data.moveSpeed;
            
            view.CharacterController.Move(moveDir.normalized * (targetSpeed * Time.deltaTime));
        }
    }

    private void HandleGravity()
    {
        if (view.CharacterController.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = data.groundedGravity;
        }

        verticalVelocity.y += data.gravity * Time.deltaTime;
        view.CharacterController.Move(verticalVelocity * Time.deltaTime);
    }
}