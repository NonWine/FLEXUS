using System;

[System.Serializable]
public class CarVFXSettingsData
{
    public float minSpeedForEffects;
    public float driftAngularVelThreshold;
    public float brakeTorqueThreshold;
    public float visualBrakeThreshold;
    public float skidMinSpeed;

    public CarVFXSettingsData()
    {
    }

    public CarVFXSettingsData(CarVFXSettingsData source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        minSpeedForEffects = source.minSpeedForEffects;
        driftAngularVelThreshold = source.driftAngularVelThreshold;
        brakeTorqueThreshold = source.brakeTorqueThreshold;
        visualBrakeThreshold = source.visualBrakeThreshold;
        skidMinSpeed = source.skidMinSpeed;
    }
}
