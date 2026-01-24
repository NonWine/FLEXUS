using System;
using System.Collections.Generic;
using Zenject;

public class CarModuleController<T> : ITickable, IInitializable, IDisposable where T : ICarModule
{
    private readonly List<T> modules;

    public CarModuleController(List<T> modules)
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

