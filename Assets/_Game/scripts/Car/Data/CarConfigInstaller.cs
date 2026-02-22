using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarConfigInstaller", menuName = "Installers/CarConfigInstaller")]
public class CarConfigInstaller : ScriptableObjectInstaller<CarConfigInstaller>
{
    [HorizontalGroup("car")]
    public CarData CarDataTemplate;

    [HorizontalGroup("left")]
    public CarSoundSettingsData Data;
    [HorizontalGroup("right")]
    public CarVFXSettingsData CarVFXSettingsData;

    public override void InstallBindings()
    {
        var carData = CarDataTemplate != null ? new CarData(CarDataTemplate) : new CarData();
        var soundData = Data != null ? new CarSoundSettingsData(Data) : new CarSoundSettingsData();
        var vfxData = CarVFXSettingsData != null
            ? new CarVFXSettingsData(CarVFXSettingsData)
            : new CarVFXSettingsData();

        Container.BindInstance(carData);
        Container.BindInstance(soundData);
        Container.BindInstance(vfxData);
    }
}
