using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CaSoundSettingsInstaller", menuName = "Installers/CaSoundSettingsInstaller")]
public class CarSoundSettingsInstaller : ScriptableObjectInstaller<CarSoundSettingsInstaller>
{
    [HorizontalGroup("left")]
    public CarSoundSettingsData Data;
    [HorizontalGroup("right")]
    public CarVFXSettingsData CarVFXSettingsData;
    
    public override void InstallBindings()
    {
        var soundData = Data != null ? new CarSoundSettingsData(Data) : new CarSoundSettingsData();
        var vfxData = CarVFXSettingsData != null ? new CarVFXSettingsData(CarVFXSettingsData) : new CarVFXSettingsData();

        Container.BindInstance(soundData);
        Container.BindInstance(vfxData);
    }
    
}
