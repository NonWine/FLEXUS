using UnityEngine;

public class CarStabilizerModule : ICarPhysicsModule
{
    private const float INPUT_THRESHOLD = 0.1f;
    private const float STEER_HELPER_MULTIPLIER = 10f;

    private readonly Rigidbody rb;
    private readonly CarData data;
    private readonly ICarInput input;

    public CarStabilizerModule(Rigidbody rb, CarData data, ICarInput input)
    {
        this.rb = rb;
        this.data = data;
        this.input = input;
    }

    public void Initialize() { }

    public void OnFixedTick()
    {
        if (Mathf.Abs(input.Steer) < INPUT_THRESHOLD)
        {
            Vector3 angularVel = rb.angularVelocity;
            angularVel.y *= (1f - data.steerHelper * Time.fixedDeltaTime * STEER_HELPER_MULTIPLIER);
            rb.angularVelocity = angularVel;
        }
    }
}
