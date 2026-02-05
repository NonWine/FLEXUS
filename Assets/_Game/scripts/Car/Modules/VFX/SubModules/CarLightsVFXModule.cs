using UnityEngine;

public class CarLightsVFXModule : ICarVFXModule
{
    private readonly ICarInput input;
    private readonly CarVFXView view;
    private readonly CarVFXSettings settings;

    public CarLightsVFXModule(ICarInput input, CarVFXView view, CarVFXSettings settings)
    {
        this.input = input;
        this.view = view;
        this.settings = settings;
    }

    public void Initialize()
    {
        view.brakeLights.SetActive(false);
    }

    public void Tick() 
    {
        view.brakeLights.SetActive(input.IsHandbraking);
    }

    public void Dispose() { }
}
