using System;
using System.Collections.Generic;
using Zenject;

public class CarSoundController : CarModuleController<ICarSoundModule>
{
    public CarSoundController(List<ICarSoundModule> modules) : base(modules)
    {
    }
}

