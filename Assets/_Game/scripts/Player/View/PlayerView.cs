using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Transform LookAt { get; private set; }
    [field: SerializeField] public Transform InteractionSource { get; private set; }
    [field:SerializeField] public PlayerData PlayerData { get; private set; }
  
    private void Awake()
    {
        if (LookAt == null)
        {
            LookAt = transform;
        }
        
        if (InteractionSource == null)
        {
            InteractionSource = transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (InteractionSource != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(InteractionSource.position, PlayerData.interactionRadius);
        }
    }
}
