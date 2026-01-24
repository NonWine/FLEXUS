using UnityEngine;

[CreateAssetMenu(fileName = "NewCarSoundSettings", menuName = "Configs/Car Sound Settings")]
public class CarSoundSettings : ScriptableObject
{
    [Header("Engine Sound")]
    public float idlePitch = 0.7f;
    public float maxPitch = 2.2f;
    public float reverseMaxPitch = 1.5f;
    public float pitchSpeedMultiplier = 0.04f;
    public float pitchInputMultiplier = 0.3f;
    public float engineVolumeLerpSpeed = 3f;
    public float pitchLerpSpeed = 5f;

    [Header("Reverse Gear Whine")]
    public float reverseWhineMaxVolume = 0.4f;
    public float reverseWhineFadeSpeed = 3f;

    [Header("Tire Screech")]
    public float screechVolumeSpeed = 8f;
    public float screechMinSpeed = 3f;
    public float screechMinAngularVel = 1.5f;
    public float screechMaxVolume = 0.7f;
}
