using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Infrastructure
{
    public class GameStateController : IInitializable
    {
        private readonly InputActionAsset _inputActions;
        private GameState _currentState;

        public event Action<GameState> OnStateChanged;

        public GameStateController(InputActionAsset inputActions)
        {
            _inputActions = inputActions;
        }

        public void Initialize()
        {
            SetState(GameState.Player);
        }

        public void SetState(GameState newState)
        {
            _currentState = newState;
            UpdateInputMaps();
            OnStateChanged?.Invoke(_currentState);
        }

        private void UpdateInputMaps()
        {
            var playerMap = _inputActions.FindActionMap("Player");
            var carMap = _inputActions.FindActionMap("Car");

            if (_currentState == GameState.Player)
            {
                carMap?.Disable();
                playerMap?.Enable();
            }
            else if (_currentState == GameState.Car)
            {
                playerMap?.Disable();
                carMap?.Enable();
            }
        }
    }
}
