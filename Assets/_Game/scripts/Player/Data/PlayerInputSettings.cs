using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlayerInput", menuName = "Configs/Input/Player Input Settings")]
public class PlayerInputSettings : BaseInputSettings
{
    public string sprintActionName = "Sprint";
    public string interactActionName = "Interact";

    public PlayerInputSettings()
    {
        mapName = "Player";
    }
}
