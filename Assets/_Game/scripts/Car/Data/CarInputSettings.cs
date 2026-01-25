using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCarInput", menuName = "Configs/Input/Car Input Settings")]
public class CarInputSettings : BaseInputSettings
{
    public string handbrakeActionName = "Handbrake";
    public string exitActionName = "Exit";

    public CarInputSettings()
    {
        mapName = "Car";
    }
}
