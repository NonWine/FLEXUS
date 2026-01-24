using System;
using System.Collections.Generic;
using Zenject;

public class CarVFXController : ITickable, IInitializable, IDisposable
{
    private readonly List<ICarVFXModule> modules;

    public CarVFXController(List<ICarVFXModule> modules)
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
