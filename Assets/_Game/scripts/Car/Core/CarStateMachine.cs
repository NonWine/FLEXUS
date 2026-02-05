using System;
using System.Collections.Generic;
using Zenject;

public class CarStateMachine : ITickable
{
    private readonly DiContainer container;
    private ICarState currentState;
    private readonly Dictionary<Type, ICarState> states = new Dictionary<Type, ICarState>();

    public CarStateMachine(DiContainer container)
    {
        this.container = container;
    }

    public void RegisterState<T>() where T : ICarState
    {
        states[typeof(T)] = container.Resolve<T>();
    }

    public void ChangeState<T>() where T : ICarState
    {
        currentState?.Exit();
        currentState = states[typeof(T)];
        currentState.Enter();
    }

    public void Tick()
    {
        currentState?.Tick();
    }
}
