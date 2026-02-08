using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CarPhysics : IFixedTickable, IInitializable, IVehiclePhysics
{
    private const float MS_TO_KMH = 3.6f;

    private readonly List<ICarPhysicsModule> modules;
    private readonly ICarMotor motor;
    private readonly Rigidbody rigidbody;
    private readonly CarData carData;

    public float CurrentSpeedKmH => rigidbody.linearVelocity.magnitude * MS_TO_KMH;
    public float CurrentBrakeTorque => motor.CurrentBrakeTorque;
    public float AngularVelocityY => Mathf.Abs(rigidbody.angularVelocity.y); 

    public CarPhysics(Rigidbody rb, CarData carData, List<ICarPhysicsModule> modules, ICarMotor motor)
    {
        this.carData = carData;
        this.rigidbody = rb;
        this.modules = modules;
        this.motor = motor;
        
        rigidbody.centerOfMass = carData.centerOfMassOffset;
    }

    public void Initialize()
    {
        foreach (var module in modules)
        {
            module.Initialize();
        }
    }

    public void FixedTick()
    {
        foreach (var module in modules)
        {
            module.OnFixedTick();
        }
    }
}
