
using UnityEngine;
using UnityEditor;

namespace UniversalAI
{
    [CustomPropertyDrawer(typeof(GroupAttribute))]
    public class GroupDrawer : PropertyDrawer
    {
        private const int k_HeaderPadding = 5;
        private const int k_HorizontalPadding = 16;
        private const int k_VerticalPadding = 8;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color normalGUIColor = GUI.color;
            Color normalContentColor = GUI.contentColor;

            if(property.isExpanded)
                GUI.Box(position, "");

            Rect rect = new Rect(position.x, position.y, position.width, 24);

            if (!EditorGUIUtility.isProSkin)
                GUI.contentColor = new Color(0.45f, 0.45f, 0.45f, 1);

            GUI.color = property.isExpanded ? normalGUIColor : new Color(normalGUIColor.r, normalGUIColor.g, normalGUIColor.b, 0.8f);

            //Draw the group button
            string displayName = property.displayName;
            var attr = (GroupAttribute)attribute;

            if (!property.isExpanded)
            {
                displayName = "Open " + displayName;
            }
            else
            {
                displayName = "Close " + displayName;
            }

            GUIStyle ButtonStyle = EditorGUICustom.StandardButtonStyle;
            if (!EditorGUIUtility.isProSkin)
            {
                ButtonStyle.normal.textColor = Color.black;
                ButtonStyle.hover.textColor = Color.black;
                ButtonStyle.focused.textColor = Color.black;     
            }
           
            if (GUI.Button(EditorGUI.IndentedRect(rect), displayName, ButtonStyle))
                property.isExpanded = !property.isExpanded;

            Color color = GUI.backgroundColor;
            if (color.maxColorComponent > 0.65f)
                color *= 0.97f;

            GUI.backgroundColor = color;

            GUI.contentColor = normalContentColor;
            GUI.color = normalGUIColor;
           
            rect = new Rect(rect.x + k_HorizontalPadding, rect.y + k_VerticalPadding, rect.width - k_HorizontalPadding * 2, EditorGUIUtility.singleLineHeight);

            rect.y = rect.yMax + EditorGUIUtility.standardVerticalSpacing;

            normalGUIColor = GUI.color;
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);

            //Draw the properties 
            if(property.isExpanded)
                DrawChildProperties(property, rect);

            GUI.color = normalGUIColor;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true) + k_HeaderPadding + (property.isExpanded ? k_VerticalPadding : 0);
        }

        private void DrawChildProperties(SerializedProperty property, Rect rect) 
        {
            foreach (SerializedProperty child in property.GetChildren())
            {
                EditorGUI.PropertyField(rect, child, true);
                rect.y = rect.y + EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}