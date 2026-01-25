using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Configs/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float sprintSpeed = 8f;
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.8f;
    public float groundedGravity = -2f;

    [Header("Interaction")]
    public float interactionRadius = 2f;
    public LayerMask interactionLayer;

    [Header("Input Settings")]
    public string mapName = "Player";
    public string moveActionName = "Move";
    public string sprintActionName = "Sprint";
    public string interactActionName = "Interact";
    
    [Header("Animation")]
    public float animationSmoothTime = 0.1f;
    public string speedParameterName = "Speed";
}
