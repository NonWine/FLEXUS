using UnityEngine;

public class CarTireSoundModule : ICarSoundModule
{
    private const float SCREECH_SPEED_THRESHOLD = 10f;

    private readonly ICarInput input;
    private readonly IVehiclePhysics vehiclePhysics;
    private readonly CarSoundView view;
    private readonly CarSoundSettingsData settings;

    public CarTireSoundModule(ICarInput input, IVehiclePhysics vehiclePhysics, CarSoundView view, CarSoundSettingsData settings)
    {
        this.input = input;
        this.vehiclePhysics = vehiclePhysics;
        this.view = view;
        this.settings = settings;
    }

    public void Initialize() { }

    public void Tick()
    {
        bool isDrifting = input.IsHandbraking;
        float speed = vehiclePhysics.CurrentSpeedKmH;
        float angularVelY = vehiclePhysics.AngularVelocityY;
        
        if ((isDrifting && speed > settings.screechMinSpeed) || (angularVelY > settings.screechMinAngularVel && speed > SCREECH_SPEED_THRESHOLD))
        {
            view.tireScreechSource.volume = Mathf.MoveTowards(view.tireScreechSource.volume, settings.screechMaxVolume, Time.deltaTime * settings.screechVolumeSpeed);
        }
        else
        {
            view.tireScreechSource.volume = Mathf.MoveTowards(view.tireScreechSource.volume, 0f, Time.deltaTime * settings.screechVolumeSpeed);
        }
    }

    public void Dispose() { }
}
