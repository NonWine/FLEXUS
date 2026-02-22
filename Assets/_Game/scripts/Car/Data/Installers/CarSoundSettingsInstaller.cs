using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarSoundSettingsInstaller", menuName = "Installers/CarSoundSettingsInstaller")]
public class CarSoundSettingsInstaller : ScriptableObjectInstaller<CarSoundSettingsInstaller>
{
    public CarSoundSettingsData CarSoundSettingsTemplate;

    public override void InstallBindings()
    {
        var soundData = CarSoundSettingsTemplate != null
            ? new CarSoundSettingsData(CarSoundSettingsTemplate)
            : new CarSoundSettingsData();

        Container.BindInstance(soundData);
    }
}