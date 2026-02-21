#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zenject;

public class CarDebugWindow : OdinEditorWindow
{
    [MenuItem("Tools/Car Debug")]
    private static void Open()
    {
        GetWindow<CarDebugWindow>("Car Debug");
    }

    [ShowInInspector, ReadOnly]
    private bool IsPlaying => Application.isPlaying;

    [ShowInInspector, ListDrawerSettings(Expanded = true, DraggableItems = false)]
    private readonly List<CarRuntimeEntry> cars = new List<CarRuntimeEntry>();

    [Button(ButtonSizes.Small)]
    private void Refresh()
    {
        Rebuild();
        Repaint();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        Rebuild();
    }

    protected override void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        base.OnDisable();
    }

    protected override void OnImGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to edit runtime car data.", MessageType.Info);
        }

        base.OnImGUI();
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingPlayMode)
        {
            Rebuild();
            Repaint();
        }
    }

    private void Rebuild()
    {
        cars.Clear();

        if (!Application.isPlaying)
        {
            return;
        }

        var contexts = FindObjectsByType<GameObjectContext>(FindObjectsSortMode.None);

        foreach (var context in contexts)
        {
            if (context == null || context.GetComponent<CarInstaller>() == null)
            {
                continue;
            }

            var container = context.Container;
            if (container == null)
            {
                continue;
            }

            var sound = container.TryResolve<CarSoundSettingsData>();
            var vfx = container.TryResolve<CarVFXSettingsData>();

            if (sound == null || vfx == null)
            {
                continue;
            }

            var carData = container.TryResolve<CarData>();
            
            CarRuntimeEntry carRuntimeEntry = new CarRuntimeEntry(context, carData.CarDataId, sound, vfx);
            cars.Add(carRuntimeEntry);
        }
    }
}
#endif
