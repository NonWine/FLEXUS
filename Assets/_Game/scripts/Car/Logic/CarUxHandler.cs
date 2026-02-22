using System;
using Zenject;

public class CarUxHandler : IInitializable, IDisposable
{
    private readonly CarView view;
    private readonly CarData data;
    private readonly SignalBus signalBus;


    public CarUxHandler(CarView view, CarData data, SignalBus signalBus)
    {
        this.view = view;
        this.data = data;
        this.signalBus = signalBus;
    }

    public void Initialize()
    {
        view.OnShowUxEvent += ShowOutline;
        view.OnHideUxEvent += HideOutline;
        signalBus.Subscribe<VehicleOccupiedSignal>(OnVehicleOccupiedChanged);
    }

    public void Dispose()
    {
        view.OnShowUxEvent -= ShowOutline;
        view.OnHideUxEvent -= HideOutline;
        signalBus.Unsubscribe<VehicleOccupiedSignal>(OnVehicleOccupiedChanged);
    }

    private void OnVehicleOccupiedChanged(VehicleOccupiedSignal signal)
    {
        if (signal.IsOccupied)
        {
            HideOutline();
        }
    }


    private void ShowOutline()
    {
        if (view.Outline != null)
        {
            view.Outline.OutlineColor = data.CarOutlineColor;
            view.Outline.OutlineWidth = data.OutlineWidth;
        }
    }

    private void HideOutline()
    {
        if (view.Outline != null)
        {
            view.Outline.OutlineWidth = 0f;
        }
    }
}
