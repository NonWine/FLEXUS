using UnityEngine;

[CreateAssetMenu(fileName = "NewCarData", menuName = "Configs/Car Data")]
public class CarData : ScriptableObject
{
    [Header("Engine & Movement")]
    public float maxMotorTorque = 2500f;
    public float maxSpeed = 120f;
    public float accelerationLerp = 5f;

    [Header("Steering")]
    public float maxSteeringAngle = 35f;
    public float minSteeringAngle = 12f;
    public float steerHelper = 0.7f;

    [Header("Braking")]
    public float brakeTorque = 5000f;
    public float handbrakeTorque = 8000f;

    [Header("Physics & Stability")]
    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0.4f);
    public float normalStiffness = 1.0f;
    public float driftStiffness = 0.4f;

    [Header("Interaction")]
    public float maxExitSpeedKmH = 15f;

    [Header("Input Settings")]
    public string mapName = "Car";
    public string moveActionName = "Move";
    public string handbrakeActionName = "Handbrake";
    public string exitActionName = "Exit";

    [Header("Sub-Configs")]
    public CarSoundSettings soundSettings; // Посилання на звуковий конфіг
}
