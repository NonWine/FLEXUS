using System;
using System.Collections.Generic;
using Zenject;

public class CarSoundController : ITickable, IInitializable, IDisposable
{
    private readonly List<ICarSoundModule> modules;

    public CarSoundController(List<ICarSoundModule> modules)
    {
        this.modules = modules;
    }

    public void Initialize()
    {
        foreach (var module in modules)
        {
            module.Initialize();
        }
    }

    public void Tick()
    {
        foreach (var module in modules)
        {
            module.Tick();
        }
    }

    public void Dispose()
    {
        foreach (var module in modules)
        {
            module.Dispose();
        }
    }
}
