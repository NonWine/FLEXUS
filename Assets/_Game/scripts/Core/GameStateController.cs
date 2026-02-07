using System;
using Infrastructure;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

    public class GameStateController : IInitializable , IDisposable
    {
        private readonly InputActionAsset inputActions;
        private readonly SignalBus signalBus;
        private GameState currentState;

        public event Action<GameState> OnStateChanged;

        public GameStateController(InputActionAsset inputActions, SignalBus signalBus)
        {
            this.inputActions = inputActions;
            this.signalBus = signalBus;
        }

        public void Initialize()
        {
            SetState(GameState.Player);
            signalBus.Subscribe<VehicleOccupiedSignal>(OnVehicleOccupied);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            UpdateInputMaps();
            OnStateChanged?.Invoke(currentState);
        }
        
        public void Dispose()
        {
            signalBus.Unsubscribe<VehicleOccupiedSignal>(OnVehicleOccupied);
        }
        
        private void UpdateInputMaps()
        {
            var playerMap = inputActions.FindActionMap("Player");
            var carMap = inputActions.FindActionMap("Car");

            if (currentState == GameState.Player)
            {
                carMap?.Disable();
                playerMap?.Enable();
            }
            else if (currentState == GameState.Car)
            {
                playerMap?.Disable();
                carMap?.Enable();
            }
        }
        
        private void OnVehicleOccupied(VehicleOccupiedSignal vehicleOccupiedSignal)
        {
            SetState(vehicleOccupiedSignal.IsOccupied ? GameState.Car : GameState.Player);
        }


    }
