using UnityEngine;
using Zenject;

public class PlayerAnimation : ITickable
{
    private readonly PlayerView view;
    private readonly IPlayerMovement movement;
    private readonly PlayerData data;
    private readonly int speedHash;

    public PlayerAnimation(PlayerView view, IPlayerMovement movement, PlayerData data)
    {
        this.view = view;
        this.movement = movement;
        this.data = data;
        speedHash = Animator.StringToHash(this.data.speedParameterName);
    }

    public void Tick()
    {

        Vector3 horizontalVelocity = movement.CurrentVelocity;
        horizontalVelocity.y = 0;
        float speed = horizontalVelocity.magnitude;

        float normalizedSpeed = Mathf.Clamp01(speed / data.sprintSpeed);

        view.Animator.SetFloat(speedHash, normalizedSpeed, data.animationSmoothTime, Time.deltaTime);
    }
}
