using System;
using System.Collections.Generic;
using Zenject;

public class CarStateMachine : ITickable
{
    private ICarState currentState;
    private readonly Dictionary<Type, ICarState> states = new Dictionary<Type, ICarState>();


    public CarStateMachine(List<ICarState> allStates)
    {
        foreach (var state in allStates)
        {
            states[state.GetType()] = state;
        }
    }

    public void ChangeState<T>() where T : ICarState
    {
        if (!states.TryGetValue(typeof(T), out var newState))
        {
            UnityEngine.Debug.LogError($"State {typeof(T)} not registered!");
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Tick()
    {
        currentState?.Tick();
    }
}