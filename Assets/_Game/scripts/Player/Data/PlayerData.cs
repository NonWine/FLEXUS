using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Configs/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    [Header("Interaction")]
    public float interactionRadius = 2f;
    public LayerMask interactionLayer;

    [Header("Configs")]
    public PlayerInputSettings inputSettings; 
    
    [Header("Animation")]
    public float animationSmoothTime = 0.1f;
    public string speedParameterName = "Speed";
}
