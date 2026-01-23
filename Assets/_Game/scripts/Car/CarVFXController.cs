using UnityEngine;

public class CarVFXController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarController carController;

    [Header("Visual Effects")]
    [SerializeField] private TrailRenderer[] skidMarks;
    [SerializeField] private ParticleSystem[] tireSmoke;
    [SerializeField] private GameObject brakeLights;

    void Start()
    {
        if (brakeLights != null) brakeLights.SetActive(false);
        ToggleSkidMarks(false);
    }

    void Update()
    {
        if (carController == null) return;

        HandleVisualEffects();
    }

    private void HandleVisualEffects()
    {
        bool isBrakingVisual = carController.IsHandbraking || carController.CurrentBrakeTorque > 10f;
        if (brakeLights != null) brakeLights.SetActive(isBrakingVisual);

        float speed = carController.CurrentSpeedKmH;
        float angularVelY = Mathf.Abs(carController.CarRigidbody.angularVelocity.y);

        bool shouldShowEffects = (carController.IsHandbraking && speed > 5f) || 
                                 (carController.CurrentBrakeTorque > 1000f && speed > 15f) ||
                                 (angularVelY > 1.5f && speed > 20f);

        ToggleSkidMarks(shouldShowEffects);
        ToggleSmoke(shouldShowEffects);
    }

    private void ToggleSkidMarks(bool toggle)
    {
        foreach (var trail in skidMarks)
        {
            if (trail != null) trail.emitting = toggle;
        }
    }

    private void ToggleSmoke(bool toggle)
    {
        foreach (var smoke in tireSmoke)
        {
            if (smoke != null)
            {
                if (toggle && !smoke.isPlaying) smoke.Play();
                else if (!toggle && smoke.isPlaying) smoke.Stop();
            }
        }
    }
}