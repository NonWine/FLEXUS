using UnityEngine;
using Zenject;

public class CarVisuals : ITickable
{
    private readonly WheelColliders colliders;
    private readonly WheelMeshes meshes;

    public CarVisuals(WheelColliders colliders, WheelMeshes meshes)
    {
        this.colliders = colliders;
        this.meshes = meshes;
    }

    public void Tick()
    {

        UpdateWheel(colliders.frontLeft, meshes.frontLeft);
        UpdateWheel(colliders.frontRight, meshes.frontRight);
        UpdateWheel(colliders.rearLeft, meshes.rearLeft);
        UpdateWheel(colliders.rearRight, meshes.rearRight);
    }

    private void UpdateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
