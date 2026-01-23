using System.Collections;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarController carController;
    [SerializeField] private Rigidbody carRigidbody;

    [Header("Engine Sound Settings")]
    [SerializeField] private AudioSource ignitionSource; 
    [SerializeField] private AudioSource idleSource;      
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private float idlePitch = 0.7f;
    [SerializeField] private float maxPitch = 2.2f;
    [SerializeField] private float reverseMaxPitch = 1.5f;
    [SerializeField] private float pitchSpeedMultiplier = 0.04f;
    [SerializeField] private float pitchInputMultiplier = 0.3f;

    [Header("Reverse Gear Whine")]
    [SerializeField] private AudioSource reverseWhineSource;
    [SerializeField] private float reverseWhineMaxVolume = 0.4f;

    [Header("Tire Screech Settings")]
    [SerializeField] private AudioSource tireScreechSource;
    [SerializeField] private float screechVolumeSpeed = 8f;

    private float currentPitch;
    private bool isEngineRunning = false;

    void Start()
    {
        if (ignitionSource != null)
        {
            StartCoroutine(StartEngineSequence());
        }
        else
        {
            isEngineRunning = true;
            if (idleSource != null) idleSource.Play();
            if (engineSource != null) engineSource.Play();
        }
    }

    private IEnumerator StartEngineSequence()
    {
        ignitionSource.Play();
        
        yield return new WaitForSeconds(ignitionSource.clip.length * 0.7f);
        
        isEngineRunning = true;
        
        if (idleSource != null) 
        {
            idleSource.volume = 0;
            idleSource.Play();
        }
        if (engineSource != null)
        {
            engineSource.volume = 0;
            engineSource.Play();
        }
    }

    void Update()
    {
        if (carController == null || carRigidbody == null || !isEngineRunning) return;

        float forwardSpeed = Vector3.Dot(transform.forward, carRigidbody.linearVelocity);
        bool isReversing = forwardSpeed < -0.1f;

        HandleEngineSound(isReversing);
        HandleReverseWhine(isReversing, Mathf.Abs(forwardSpeed));
        HandleTireScreech();
    }

    private void HandleEngineSound(bool isReversing)
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        float gasInput = Mathf.Abs(carController.VerticalInput);

        float effectiveMaxPitch = isReversing ? reverseMaxPitch : maxPitch;
        float targetPitch = idlePitch + (speed * pitchSpeedMultiplier) + (gasInput * pitchInputMultiplier);
        
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * 5f);
        float finalPitch = Mathf.Clamp(currentPitch, idlePitch, effectiveMaxPitch);

        if (idleSource != null) idleSource.pitch = finalPitch;
        engineSource.pitch = finalPitch;

        float load = Mathf.Clamp01((speed / 10f) + gasInput);
        
        if (idleSource != null)
        {
            idleSource.volume = Mathf.Lerp(0.6f, 0.2f, load);
        }

        float targetEngineVolume = Mathf.Lerp(0.0f, 0.8f, load);
        engineSource.volume = Mathf.MoveTowards(engineSource.volume, targetEngineVolume, Time.deltaTime * 3f);
    }

    private void HandleReverseWhine(bool isReversing, float speed)
    {
        if (reverseWhineSource == null) return;

        if (isReversing)
        {
            reverseWhineSource.volume = Mathf.Lerp(0, reverseWhineMaxVolume, speed / 10f);
            reverseWhineSource.pitch = Mathf.Lerp(0.8f, 1.8f, speed / 10f);
        }
        else
        {
            reverseWhineSource.volume = Mathf.MoveTowards(reverseWhineSource.volume, 0f, Time.deltaTime * 3f);
        }
    }

    private void HandleTireScreech()
    {
        bool isDrifting = carController.IsHandbraking;
        float speed = carRigidbody.linearVelocity.magnitude;
        float angularVelY = Mathf.Abs(carRigidbody.angularVelocity.y);
        
        if ((isDrifting && speed > 3f) || (angularVelY > 1.5f && speed > 10f))
        {
            tireScreechSource.volume = Mathf.MoveTowards(tireScreechSource.volume, 0.7f, Time.deltaTime * screechVolumeSpeed);
        }
        else
        {
            tireScreechSource.volume = Mathf.MoveTowards(tireScreechSource.volume, 0f, Time.deltaTime * screechVolumeSpeed);
        }
    }
}