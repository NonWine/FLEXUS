using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameStateControllerTests
{
    [Test]
    public void Initialize_SetsPlayerState_EnablesPlayerMapAndDisablesCarMap()
    {
        var inputActions = CreateInputActions(out var playerMap, out var carMap);
        var signalBus = CreateSignalBus();
        var controller = new GameStateController(inputActions, signalBus);
        var receivedStates = new List<GameState>();
        controller.OnStateChanged += receivedStates.Add;

        try
        {
            controller.Initialize();

            Assert.That(playerMap.enabled, Is.True);
            Assert.That(carMap.enabled, Is.False);
            Assert.That(receivedStates, Is.EqualTo(new[] { GameState.Player }));
        }
        finally
        {
            controller.Dispose();
            Object.DestroyImmediate(inputActions);
        }
    }

    [Test]
    public void SetState_WhenCarSelected_EnablesCarMapAndDisablesPlayerMap()
    {
        var inputActions = CreateInputActions(out var playerMap, out var carMap);
        var signalBus = CreateSignalBus();
        var controller = new GameStateController(inputActions, signalBus);
        var receivedStates = new List<GameState>();
        controller.OnStateChanged += receivedStates.Add;

        try
        {
            controller.SetState(GameState.Car);

            Assert.That(playerMap.enabled, Is.False);
            Assert.That(carMap.enabled, Is.True);
            Assert.That(receivedStates, Is.EqualTo(new[] { GameState.Car }));
        }
        finally
        {
            Object.DestroyImmediate(inputActions);
        }
    }

    [Test]
    public void Initialize_WhenVehicleOccupiedSignalIsFiredWithTrue_SwitchesToCarState()
    {
        var inputActions = CreateInputActions(out var playerMap, out var carMap);
        var signalBus = CreateSignalBus();
        var controller = new GameStateController(inputActions, signalBus);
        var receivedStates = new List<GameState>();
        controller.OnStateChanged += receivedStates.Add;

        try
        {
            controller.Initialize();

            signalBus.Fire(new VehicleOccupiedSignal
            {
                IsOccupied = true
            });

            Assert.That(playerMap.enabled, Is.False);
            Assert.That(carMap.enabled, Is.True);
            Assert.That(receivedStates, Is.EqualTo(new[] { GameState.Player, GameState.Car }));
        }
        finally
        {
            controller.Dispose();
            Object.DestroyImmediate(inputActions);
        }
    }

    [Test]
    public void Initialize_WhenVehicleOccupiedSignalIsFiredWithFalse_SwitchesToPlayerState()
    {
        var inputActions = CreateInputActions(out var playerMap, out var carMap);
        var signalBus = CreateSignalBus();
        var controller = new GameStateController(inputActions, signalBus);
        var receivedStates = new List<GameState>();
        controller.OnStateChanged += receivedStates.Add;

        try
        {
            controller.Initialize();
            controller.SetState(GameState.Car);

            signalBus.Fire(new VehicleOccupiedSignal
            {
                IsOccupied = false
            });

            Assert.That(playerMap.enabled, Is.True);
            Assert.That(carMap.enabled, Is.False);
            Assert.That(receivedStates, Is.EqualTo(new[] { GameState.Player, GameState.Car, GameState.Player }));
        }
        finally
        {
            controller.Dispose();
            Object.DestroyImmediate(inputActions);
        }
    }

    [Test]
    public void Dispose_UnsubscribesFromVehicleOccupiedSignal()
    {
        var inputActions = CreateInputActions(out var playerMap, out var carMap);
        var signalBus = CreateSignalBus();
        var controller = new GameStateController(inputActions, signalBus);
        var receivedStates = new List<GameState>();
        controller.OnStateChanged += receivedStates.Add;

        try
        {
            controller.Initialize();
            controller.SetState(GameState.Car);
            controller.Dispose();

            signalBus.Fire(new VehicleOccupiedSignal
            {
                IsOccupied = false
            });

            Assert.That(playerMap.enabled, Is.False);
            Assert.That(carMap.enabled, Is.True);
            Assert.That(receivedStates, Is.EqualTo(new[] { GameState.Player, GameState.Car }));
        }
        finally
        {
            Object.DestroyImmediate(inputActions);
        }
    }

    private static InputActionAsset CreateInputActions(out InputActionMap playerMap, out InputActionMap carMap)
    {
        var inputActions = ScriptableObject.CreateInstance<InputActionAsset>();

        playerMap = new InputActionMap("Player");
        playerMap.AddAction("Move");
        inputActions.AddActionMap(playerMap);

        carMap = new InputActionMap("Car");
        carMap.AddAction("Drive");
        inputActions.AddActionMap(carMap);

        return inputActions;
    }

    private static SignalBus CreateSignalBus()
    {
        var container = new DiContainer();
        SignalBusInstaller.Install(container);
        container.DeclareSignal<VehicleOccupiedSignal>();
        return container.Resolve<SignalBus>();
    }
}
