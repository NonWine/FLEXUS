using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CarVFXController : CarModuleController<ICarVFXModule>
{
    public CarVFXController(List<ICarVFXModule> modules) : base(modules)
    {
    }
}
