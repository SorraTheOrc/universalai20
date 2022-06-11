using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniversalAI
{

public sealed class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
 
    public static List<int> GetSelectedIndexes<T>(T val) where T : IConvertible
    {
        List<int> selectedIndexes = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(T)).Length; i++) {
            int layer = 1 << i;
            if ((Convert.ToInt32(val) & layer) != 0) {
                selectedIndexes.Add(i);
            }
        }
        return selectedIndexes;
    }
    public static List<string> GetSelectedStrings<T>(T val) where T : IConvertible
    {
        List<string> selectedStrings = new List<string>();
        for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++) {
            int layer = 1 << i;
            if ((Convert.ToInt32(val) & layer) != 0) {
                selectedStrings.Add(Enum.GetValues(typeof(T)).GetValue(i).ToString());
            }
        }
        return selectedStrings;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}
#endif
    
}