using UnityEngine;

[CreateAssetMenu(fileName = "NewCarVFXSettings", menuName = "Configs/Car/Car VFX Settings")]
public class CarVFXSettings : ScriptableObject
{
    public float minSpeedForEffects;
    public float driftAngularVelThreshold;
    public float brakeTorqueThreshold;
    public float visualBrakeThreshold;
    public float skidMinSpeed;
}
