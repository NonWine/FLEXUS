using UnityEngine;
using Zenject;
using Unity.Cinemachine;

namespace Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;

        [Inject] private PlayerFacade.Factory _playerFactory;
        

        private void Start()
        {
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            var player = _playerFactory.Create();
            
            player.Transform.position = spawnPoint.position;
            player.Transform.rotation = spawnPoint.rotation;
        }
    }
}
