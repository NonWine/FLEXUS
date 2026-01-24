using UnityEngine;

public class CarTireEffectsVFXModule : ICarVFXModule
{
    private readonly ICarInput input;
    private readonly CarPhysics physics;
    private readonly Rigidbody rb;
    private readonly CarVFXView view;
    private readonly CarVFXSettings settings;

    public CarTireEffectsVFXModule(ICarInput input, CarPhysics physics, Rigidbody rb, CarVFXView view, CarVFXSettings settings)
    {
        this.input = input;
        this.physics = physics;
        this.rb = rb;
        this.view = view;
        this.settings = settings;
    }

    public void Initialize()
    {
        ToggleEffects(false);
    }

    public void Tick()
    {
        float speed = physics.CurrentSpeedKmH;
        float angularVelY = Mathf.Abs(rb.angularVelocity.y);

        bool shouldShowEffects = (input.IsHandbraking && speed > settings.minSpeedForEffects) || 
                                 (physics.CurrentBrakeTorque > settings.brakeTorqueThreshold && speed > settings.skidMinSpeed) ||
                                 (angularVelY > settings.driftAngularVelThreshold && speed > 20f);

        ToggleEffects(shouldShowEffects);
    }

    private void ToggleEffects(bool toggle)
    {
        foreach (var trail in view.skidMarks)
        {
            if (trail != null) trail.emitting = toggle;
        }

        foreach (var smoke in view.tireSmoke)
        {
            if (smoke != null)
            {
                if (toggle && !smoke.isPlaying) smoke.Play();
                else if (!toggle && smoke.isPlaying) smoke.Stop();
            }
        }
    }

    public void Dispose() { }
}
