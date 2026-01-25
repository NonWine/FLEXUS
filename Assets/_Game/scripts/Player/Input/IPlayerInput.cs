using System;
using UnityEngine;

public interface IPlayerInput
{
    event Action OnInteract;
    Vector2 MoveInput { get; }
    bool IsSprinting { get; }
}