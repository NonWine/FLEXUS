#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

[System.Serializable]
public sealed class CarRuntimeEntry
{
     public GameObjectContext Context { get; }
     public string CarDataId { get; }

    [ShowInInspector, ReadOnly, PropertyOrder(-20)]
    public string CarName => Context != null ? Context.name : "<missing>";

    [ShowInInspector, ReadOnly, PropertyOrder(-19)]
    public string CarId => CarDataId;

    [ShowInInspector, InlineProperty, FoldoutGroup("Sound"), HideLabel]
    public CarSoundSettingsData Sound { get; }

    [ShowInInspector, InlineProperty, FoldoutGroup("VFX"), HideLabel]
    public CarVFXSettingsData Vfx { get; }

    public CarRuntimeEntry(
        GameObjectContext context,
        string carDataId,
        CarSoundSettingsData sound,
        CarVFXSettingsData vfx)
    {
        Context = context;
        CarDataId = carDataId;
        Sound = sound;
        Vfx = vfx;
    }

    [Button(ButtonSizes.Small), PropertyOrder(100)]
    public void SelectCar()
    {
        Selection.activeGameObject = Context.gameObject;
        EditorGUIUtility.PingObject(Context.gameObject);
    }
}
#endif
