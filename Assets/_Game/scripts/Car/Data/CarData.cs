using UnityEngine;
using System;
using Sirenix.OdinInspector;

[System.Serializable, InlineProperty, HideLabel]
public class CarData
{

    public float maxMotorTorque = 2500f;
    public float maxSpeed = 120f;
    public float accelerationLerp = 5f;
    public DriveType driveType = DriveType.AWD;
    public float maxSteeringAngle = 35f;
    public float minSteeringAngle = 12f;
    public float steerHelper = 0.7f;
    public float brakeTorque = 5000f;
    public float handbrakeTorque = 8000f;
    public Color32 CarOutlineColor = new Color(0, 255, 0, 255);
    public float OutlineWidth = 3f;

    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0.4f);
    public float normalStiffness = 1.0f;
    public float driftStiffness = 0.4f;
    public float maxExitSpeedKmH = 15f;

    public CarInputSettings inputSettings;

    public CarData()
    {
    }

    public CarData(CarData source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        maxMotorTorque = source.maxMotorTorque;
        maxSpeed = source.maxSpeed;
        accelerationLerp = source.accelerationLerp;
        driveType = source.driveType;
        maxSteeringAngle = source.maxSteeringAngle;
        minSteeringAngle = source.minSteeringAngle;
        steerHelper = source.steerHelper;
        brakeTorque = source.brakeTorque;
        handbrakeTorque = source.handbrakeTorque;
        CarOutlineColor = source.CarOutlineColor;
        OutlineWidth = source.OutlineWidth;
        centerOfMassOffset = source.centerOfMassOffset;
        normalStiffness = source.normalStiffness;
        driftStiffness = source.driftStiffness;
        maxExitSpeedKmH = source.maxExitSpeedKmH;
        inputSettings = source.inputSettings;
    }
    
}
