using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CarDefinition", menuName = "Configs/Car/Car Definition")]
public class CarDefinition : ScriptableObject
{
    [SerializeField, ReadOnly] private string carId;
    [SerializeField] private CarView prefab;
    
    public string CarId => carId;
    public CarView Prefab => prefab;
    
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(carId))
        {
            RegenerateID();
        }
    }

    public void RegenerateID()
    {
        carId = Guid.NewGuid().ToString();
    }
}
