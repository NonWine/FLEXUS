using System;
using System.Collections.Generic;
using Zenject;

public class CarStateMachine : ITickable, IInitializable, IDisposable
{
    private IState currentState;
    private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
    private readonly SignalBus signalBus;

    public CarStateMachine(SignalBus signalBus, List<IState> allStates)
    {
        this.signalBus = signalBus;
        foreach (var state in allStates)
        {
            states[state.GetType()] = state;
        }
    }
    
    public void ChangeState<T>() where T : IState
    {
        ChangeState(typeof(T));
    }

    private void ChangeState(Type stateType)
    {

        if (!states.TryGetValue(stateType, out var newState))
        {
            UnityEngine.Debug.LogError($"State {stateType} not registered!");
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
        currentState?.Exit();
        signalBus.TryUnsubscribe<ChangeCarStateSignal>(OnChangeStateSignal);
    }

    private void OnChangeStateSignal(ChangeCarStateSignal signal)
    {
        ChangeState(signal.TargetStateType);
    }
    
    public void Initialize()
    {
        signalBus.Subscribe<ChangeCarStateSignal>(OnChangeStateSignal);
        ChangeState<EmptyCarState>();
    }
}
