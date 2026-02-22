using UnityEngine;
using Zenject;

[CreateAssetMenu(
    fileName = "CarCompositeConfigInstaller",
    menuName = "Installers/CarCompositeConfigInstaller")]
public class CarCompositeConfigInstaller : ScriptableObjectInstaller<CarCompositeConfigInstaller>
{
    [SerializeField] private CarDataInstaller carDataInstaller;
    [SerializeField] private CarSoundSettingsInstaller carSoundSettingsInstaller;
    [SerializeField] private CarVFXSettingsInstaller carVfxSettingsInstaller;

    public override void InstallBindings()
    {
        InstallChild(carDataInstaller);
        InstallChild(carSoundSettingsInstaller);
        InstallChild(carVfxSettingsInstaller);
    }

    private void InstallChild(ScriptableObjectInstallerBase installer)
    {
        if (installer == null) return;

        Container.Inject(installer);
        installer.InstallBindings();
    }
}