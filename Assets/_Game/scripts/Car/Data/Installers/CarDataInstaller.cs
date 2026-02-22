using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarDataInstaller", menuName = "Installers/CarDataInstaller")]
public class CarDataInstaller : ScriptableObjectInstaller<CarDataInstaller>
{
    public CarData CarDataTemplate;

    public override void InstallBindings()
    {
        var carData = CarDataTemplate != null ? new CarData(CarDataTemplate) : new CarData();
        Container.BindInstance(carData);
    }
}
