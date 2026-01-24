using UnityEngine;
using System;

public class CarOccupancyHandler : IOccupancyHandler
{
    private const float GROUND_CHECK_DISTANCE = 5f;
    private const float GROUND_OFFSET = 0.05f;

    private readonly CarView view;
    private GameObject currentPlayer;

    public bool IsOccupied => currentPlayer != null;

    public CarOccupancyHandler(CarView view)
    {
        this.view = view;
    }

    public void Enter(GameObject player)
    {
        currentPlayer = player;
        currentPlayer.transform.SetParent(view.transform);
        currentPlayer.transform.localPosition = Vector3.zero;
        currentPlayer.SetActive(false);
    }

    public void Exit(Action<GameObject, Vector3> onExited)
    {
        if (currentPlayer == null) return;

        GameObject player = currentPlayer;
        currentPlayer.transform.SetParent(null);
        
        Vector3 targetPosition = view.ExitPoint.position;
      
        if (Physics.Raycast(view.ExitPoint.position + Vector3.up, Vector3.down, out RaycastHit hit, GROUND_CHECK_DISTANCE))
        {
            targetPosition = hit.point + Vector3.up * GROUND_OFFSET;
        }

        var controller = player.GetComponent<CharacterController>();
        controller.enabled = false;

        Vector3 oldPosition = player.transform.position;
        player.transform.position = targetPosition;
        player.transform.rotation = view.ExitPoint.rotation;
        
        player.SetActive(true);
        controller.enabled = true;

        Vector3 warpDelta = targetPosition - oldPosition;
        currentPlayer = null;

        onExited?.Invoke(player, warpDelta);
    }
}
