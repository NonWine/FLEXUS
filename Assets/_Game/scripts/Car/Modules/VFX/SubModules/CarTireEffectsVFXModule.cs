using UnityEngine;

public class CarTireEffectsVFXModule : ICarVFXModule
{
    private readonly ICarInput input;
    private readonly IVehiclePhysics physics;
    private readonly CarVFXView view;
    private readonly CarVFXSettings settings;

    public CarTireEffectsVFXModule(ICarInput input, IVehiclePhysics physics, CarVFXView view, CarVFXSettings settings)
    {
        this.input = input;
        this.physics = physics;
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
        float angularVelY = physics.AngularVelocityY;

        bool shouldShowEffects = (speed > settings.minSpeedForEffects) || 
                                 (physics.CurrentBrakeTorque > settings.brakeTorqueThreshold && speed > settings.skidMinSpeed) ||
                                 (angularVelY > settings.driftAngularVelThreshold && speed > 20f);
        
        ToggleEffects(shouldShowEffects && input.IsHandbraking);
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
