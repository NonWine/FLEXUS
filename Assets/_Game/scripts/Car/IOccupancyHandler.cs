using UnityEngine;
using System;

public interface IOccupancyHandler
{
    void Enter(GameObject player);
    void Exit(Action<GameObject, Vector3> onExited);
    bool IsOccupied { get; }
}
