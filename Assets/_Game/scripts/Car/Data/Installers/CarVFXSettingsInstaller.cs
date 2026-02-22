using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarVFXSettingsInstaller", menuName = "Installers/CarVFXSettingsInstaller")]
public class CarVFXSettingsInstaller : ScriptableObjectInstaller<CarVFXSettingsInstaller>
{
    public CarVFXSettingsData CarVFXSettingsTemplate;

    public override void InstallBindings()
    {
        var vfxData = CarVFXSettingsTemplate != null
            ? new CarVFXSettingsData(CarVFXSettingsTemplate)
            : new CarVFXSettingsData();

        Container.BindInstance(vfxData);
    }
}
