using UnityEngine;

public class CarTireSoundModule : ICarSoundModule
{
    private const float SCREECH_SPEED_THRESHOLD = 10f;

    private readonly ICarInput input;
    private readonly Rigidbody rb;
    private readonly CarSoundView view;
    private readonly CarSoundSettings settings;

    public CarTireSoundModule(ICarInput input, Rigidbody rb, CarSoundView view, CarSoundSettings settings)
    {
        this.input = input;
        this.rb = rb;
        this.view = view;
        this.settings = settings;
    }

    public void Initialize() { }

    public void Tick()
    {
        bool isDrifting = input.IsHandbraking;
        float speed = rb.linearVelocity.magnitude;
        float angularVelY = Mathf.Abs(rb.angularVelocity.y);
        
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
