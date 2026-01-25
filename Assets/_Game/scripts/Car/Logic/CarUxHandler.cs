using System;
using UnityEngine;
using Zenject;

public class CarUxHandler : IInitializable, IDisposable
{
    private readonly CarView view;
    private readonly IOccupancyHandler occupancy;

    public CarUxHandler(CarView view, IOccupancyHandler occupancy)
    {
        this.view = view;
        this.occupancy = occupancy;
    }

    public void Initialize()
    {
        view.OnShowUxEvent += ShowUx;
        view.OnHideUxEvent += HideUx;
        HideUx();
    }

    public void Dispose()
    {
        view.OnShowUxEvent -= ShowUx;
        view.OnHideUxEvent -= HideUx;
    }

    private void ShowUx()
    {
        view.Outline.OutlineColor = view.CarData.CarOutlineColor;
        view.Outline.OutlineWidth = view.CarData.OutlineWidht;
    }

    private void HideUx()
    {
        view.Outline.OutlineWidth = 0f;
    }
}
