using UnityEngine;
using Unity.Cinemachine;

namespace Infrastructure
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera playerCamera;
        [SerializeField] private CinemachineCamera carCamera;

        public CinemachineCamera PlayerCamera => playerCamera;
        public CinemachineCamera CarCamera => carCamera;
    }
}
