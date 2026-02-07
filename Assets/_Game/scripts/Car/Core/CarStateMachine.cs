using System;
using System.Collections.Generic;
using Zenject;

public class CarStateMachine : ITickable , IInitializable , IDisposable
{
    private ICarState currentState;
    private readonly Dictionary<Type, ICarState> states = new Dictionary<Type, ICarState>();
    private readonly SignalBus signalBus;

    public CarStateMachine(SignalBus signalBus, List<ICarState> allStates)
    {
        this.signalBus = signalBus;
        foreach (var state in allStates)
        {
            states[state.GetType()] = state;
        }
    }

    public void ChangeState(Type state)
    {
        if (!states.TryGetValue(state, out var newState))
        {
            UnityEngine.Debug.LogError($"State {state} not registered!");
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

    public void Dispose()
    {
        signalBus.TryUnsubscribe<ChangeCarStateSignal>(OnChangeStateSignal);
    }

    private void OnChangeStateSignal(ChangeCarStateSignal signal)
    {
        ChangeState(signal.TargetStateType);
    }
    
    public void Initialize()
    {
        signalBus.Subscribe<ChangeCarStateSignal>(OnChangeStateSignal);
        
        ChangeState(typeof(CarEmptyState));
    }
}