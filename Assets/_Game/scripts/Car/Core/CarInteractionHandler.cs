using System;
using UnityEngine;
using Zenject;

public class CarInteractionHandler : IInitializable, IDisposable
{
    private readonly CarView view;
    private readonly ICarInputHandler input;
    private readonly SignalBus signalBus;

    public CarInteractionHandler(CarView view, ICarInputHandler input, SignalBus signalBus)
    {
        this.view = view;
        this.input = input;
        this.signalBus = signalBus;
    }

    public void Initialize()
    {
        view.OnInteractedEvent += HandleEnter;
        input.OnExitPerformed += HandleExit;
    }

    public void Dispose()
    {
        view.OnInteractedEvent -= HandleEnter;
        input.OnExitPerformed -= HandleExit;
    }

    private void HandleEnter(GameObject interactor)
    {
        signalBus.Fire(new CarInteractionSignal { Interactor = interactor });
    }

    private void HandleExit()
    {
        signalBus.Fire(new CarExitRequestSignal());
    }
}
