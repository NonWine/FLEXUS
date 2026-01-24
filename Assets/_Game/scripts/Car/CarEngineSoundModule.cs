using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CarEngineSoundModule : ICarSoundModule
{
    private const float IGNITION_DELAY_FACTOR = 700f;
    private const float SPEED_LOAD_DIVIDER = 10f;
    private const float IDLE_VOLUME_MIN = 0.2f;
    private const float IDLE_VOLUME_MAX = 0.6f;
    private const float ENGINE_VOLUME_MIN = 0.0f;
    private const float ENGINE_VOLUME_MAX = 0.8f;
    private const float KMH_TO_MS = 0.27778f;

    private readonly ICarInput input;
    private readonly IVehiclePhysics physics; 
    private readonly CarSoundView view;
    private readonly CarSoundSettings settings;
    private readonly CancellationTokenSource cts = new CancellationTokenSource();

    private float currentPitch;
    private bool isEngineRunning;

    public CarEngineSoundModule(ICarInput input, IVehiclePhysics physics, CarSoundView view, CarSoundSettings settings)
    {
        this.input = input;
        this.physics = physics;
        this.view = view;
        this.settings = settings;
    }

    public void Initialize()
    {
        StartEngineSequenceAsync(cts.Token).Forget();
    }

    private async UniTaskVoid StartEngineSequenceAsync(CancellationToken token)
    {
        try
        {
            view.ignitionSource.Play();
            int delayMs = (int)(view.ignitionSource.clip.length * IGNITION_DELAY_FACTOR);
            await UniTask.Delay(delayMs, cancellationToken: token);
            
            isEngineRunning = true;
            view.idleSource.Play();
            view.engineSource.Play();
        }
        catch (OperationCanceledException) { }
    }

    public void Tick()
    {
        if (!isEngineRunning) return;

        float speedMs = physics.CurrentSpeedKmH * KMH_TO_MS;
        float gasInput = Mathf.Abs(input.Throttle);

        float targetPitch = settings.idlePitch + (speedMs * settings.pitchSpeedMultiplier) + (gasInput * settings.pitchInputMultiplier);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * settings.pitchLerpSpeed);
        float finalPitch = Mathf.Clamp(currentPitch, settings.idlePitch, settings.maxPitch);

        view.idleSource.pitch = finalPitch;
        view.engineSource.pitch = finalPitch;

        float load = Mathf.Clamp01((speedMs / SPEED_LOAD_DIVIDER) + gasInput);
        view.idleSource.volume = Mathf.Lerp(IDLE_VOLUME_MAX, IDLE_VOLUME_MIN, load);
        
        float targetEngineVolume = Mathf.Lerp(ENGINE_VOLUME_MIN, ENGINE_VOLUME_MAX, load);
        view.engineSource.volume = Mathf.MoveTowards(view.engineSource.volume, targetEngineVolume, Time.deltaTime * settings.engineVolumeLerpSpeed);
    }

    public void Dispose()
    {
        cts.Cancel();
        cts.Dispose();
    }
}
