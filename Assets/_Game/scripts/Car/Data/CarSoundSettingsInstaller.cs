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
        Container.BindInstance(Data);
        Container.BindInstance(CarVFXSettingsData);
    }
    
}
