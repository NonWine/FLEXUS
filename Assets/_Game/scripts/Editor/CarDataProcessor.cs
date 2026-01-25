using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class CarDataProcessor : OdinAttributeProcessor<CarData>
{
    private const float GREEN_R = 0.2f;
    private const float GREEN_G = 0.7f;
    private const float GREEN_B = 0.3f;

    private const float ORANGE_R = 1f;
    private const float ORANGE_G = 0.5f;
    private const float ORANGE_B = 0f;

    private const float BLUE_R = 0.4f;
    private const float BLUE_G = 0.8f;
    private const float BLUE_B = 1f;

    private const float PURPLE_R = 0.75f;
    private const float PURPLE_G = 0.6f;
    private const float PURPLE_B = 1f;

    private const string MAIN_TAB = "Main";
    private const string GENERAL_GROUP = "General";
    private const string PHYSICS_GROUP = "Physics";
    private const string REFS_GROUP = "References";

    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        if (member.Name == nameof(CarData.CarDataId))
        {
            attributes.Add(new BoxGroupAttribute("Global_Identity", true, false, 0));
            attributes.Add(new ReadOnlyAttribute());
            attributes.Add(new LabelWidthAttribute(70));
        }

        if (IsGeneralMember(member.Name))
        {
            attributes.Add(new TabGroupAttribute(MAIN_TAB, GENERAL_GROUP, SdfIconType.Wrench));
        }

        if (member.Name == nameof(CarData.maxMotorTorque))
        {
            attributes.Add(new ProgressBarAttribute(0, 5000, ORANGE_R, ORANGE_G, ORANGE_B) { Height = 20 });
        }

        if (member.Name == nameof(CarData.maxSpeed))
        {
            attributes.Add(new ProgressBarAttribute(0, 400, ORANGE_R, ORANGE_G, ORANGE_B) { Height = 20 });
        }

        if (member.Name == nameof(CarData.accelerationLerp))
        {
            attributes.Add(new LabelTextAttribute("Accel. Response"));
            attributes.Add(new ProgressBarAttribute(1f, 20f, BLUE_R, BLUE_G, BLUE_B));
        }

        if (member.Name == nameof(CarData.driveType))
        {
            attributes.Add(new EnumToggleButtonsAttribute());
            attributes.Add(new HideLabelAttribute());
        }

        if (member.Name == nameof(CarData.maxSteeringAngle))
        {
            attributes.Add(new SuffixLabelAttribute("deg", true));
            attributes.Add(new ProgressBarAttribute(10f, 60f, GREEN_R, GREEN_G, GREEN_B));
        }

        if (member.Name == nameof(CarData.minSteeringAngle))
        {
            attributes.Add(new SuffixLabelAttribute("deg", true));
            attributes.Add(new ProgressBarAttribute(0f, 20f, GREEN_R, GREEN_G, GREEN_B));
        }

        if (member.Name == nameof(CarData.steerHelper))
        {
            attributes.Add(new LabelTextAttribute("Steer Helper"));
            attributes.Add(new ProgressBarAttribute(0f, 1f, GREEN_R, GREEN_G, GREEN_B));
        }

        if (member.Name == nameof(CarData.brakeTorque) || member.Name == nameof(CarData.handbrakeTorque))
        {
            attributes.Add(new ProgressBarAttribute(0f, 20000f, GREEN_R, GREEN_G, GREEN_B));
        }

        if (member.Name == nameof(CarData.OutlineWidth))
        {
            attributes.Add(new ProgressBarAttribute(0f, 10f, PURPLE_R, PURPLE_G, PURPLE_B));
        }

        if (IsPhysicsMember(member.Name))
        {
            attributes.Add(new TabGroupAttribute(MAIN_TAB, PHYSICS_GROUP, SdfIconType.Box));
        }

        if (member.Name == nameof(CarData.normalStiffness) || member.Name == nameof(CarData.driftStiffness))
        {
            attributes.Add(new RangeAttribute(0, 5));
        }

        if (IsReferenceMember(member.Name))
        {
            attributes.Add(new TabGroupAttribute(MAIN_TAB, REFS_GROUP, SdfIconType.Gear));
            attributes.Add(new InlineEditorAttribute(InlineEditorObjectFieldModes.Boxed));
        }
    }

    private bool IsGeneralMember(string name)
    {
        return name == nameof(CarData.maxMotorTorque) || 
               name == nameof(CarData.maxSpeed) || 
               name == nameof(CarData.accelerationLerp) || 
               name == nameof(CarData.driveType) || 
               name == nameof(CarData.maxSteeringAngle) || 
               name == nameof(CarData.minSteeringAngle) || 
               name == nameof(CarData.steerHelper) || 
               name == nameof(CarData.brakeTorque) || 
               name == nameof(CarData.handbrakeTorque) || 
               name == nameof(CarData.CarOutlineColor) || 
               name == nameof(CarData.OutlineWidth);
    }

    private bool IsPhysicsMember(string name)
    {
        return name == nameof(CarData.centerOfMassOffset) || 
               name == nameof(CarData.normalStiffness) || 
               name == nameof(CarData.driftStiffness) || 
               name == nameof(CarData.maxExitSpeedKmH);
    }

    private bool IsReferenceMember(string name)
    {
        return name == nameof(CarData.inputSettings) || 
               name == nameof(CarData.soundSettings) || 
               name == nameof(CarData.vfxSettings);
    }
}
