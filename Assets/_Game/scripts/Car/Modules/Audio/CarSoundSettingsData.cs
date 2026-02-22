using System;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable, InlineProperty, HideLabel]
public class CarSoundSettingsData 
{
    [Header("Engine Sound")]
    public float idlePitch = 0.7f;
    public float maxPitch = 2.2f;
    public float pitchSpeedMultiplier = 0.04f;
    public float pitchInputMultiplier = 0.3f;
    public float engineVolumeLerpSpeed = 3f;
    public float pitchLerpSpeed = 5f;

    [Header("Tire Screech")]
    public float screechVolumeSpeed = 8f;
    public float screechMinSpeed = 3f;
    public float screechMinAngularVel = 1.5f;
    public float screechMaxVolume = 0.7f;

    public CarSoundSettingsData()
    {
    }

    public CarSoundSettingsData(CarSoundSettingsData source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        idlePitch = source.idlePitch;
        maxPitch = source.maxPitch;
        pitchSpeedMultiplier = source.pitchSpeedMultiplier;
        pitchInputMultiplier = source.pitchInputMultiplier;
        engineVolumeLerpSpeed = source.engineVolumeLerpSpeed;
        pitchLerpSpeed = source.pitchLerpSpeed;
        screechVolumeSpeed = source.screechVolumeSpeed;
        screechMinSpeed = source.screechMinSpeed;
        screechMinAngularVel = source.screechMinAngularVel;
        screechMaxVolume = source.screechMaxVolume;
    }
}
