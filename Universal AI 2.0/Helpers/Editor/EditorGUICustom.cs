
using System;
using UnityEditor;
using UnityEngine;

namespace UniversalAI
{
    public static class EditorGUICustom
    {
        public static event Action OneSecondPassed;

        private static double m_OneSecondTimer;

        public static GUIStyle TitleLabel { get; private set; }
        public static GUIStyle ShortTitleLabel { get; private set; }
        public static GUIStyle MiddleLeftBoldMiniLabel { get; private set; }
        public static GUIStyle BottomLeftBoldMiniLabel { get; private set; }
        public static GUIStyle CenteredBigLabel { get; private set; }
        public static GUIStyle MiniGreyLabel { get; private set; }
        public static GUIStyle BoldMiniGreyLabel { get; private set; }

        public static GUIStyle FoldOutStyle { get; private set; }
        public static GUIStyle StandardButtonStyle { get; private set; }
        public static GUIStyle LargeButtonStyle { get; private set; }
        private static GUIStyle m_SeparatorStyle;

        public static readonly Color HighlightColor = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f, 1f) : new Color(0.35f, 0.35f, 0.35f);
        public static readonly Color HeaderColor = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 0.8f, 1f) : new Color(0.4f, 0.4f, 0.4f);
        public static readonly Color SeparatorColor = EditorGUIUtility.isProSkin ? new Color(0.16f, 0.16f, 0.16f) : new Color(0.5f, 0.5f, 0.5f);

        
        static EditorGUICustom()
        {
            EditorApplication.update += OnEditorUpdate;
           
            m_SeparatorStyle = new GUIStyle();
            m_SeparatorStyle.normal.background = EditorGUIUtility.whiteTexture;
            m_SeparatorStyle.stretchWidth = true;
            m_SeparatorStyle.margin = new RectOffset(0, 0, 7, 7);

            TitleLabel = new GUIStyle(EditorStyles.boldLabel);
            TitleLabel.fontSize = 12;
            TitleLabel.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1,1,1,0.8f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
            TitleLabel.alignment = TextAnchor.UpperCenter;

            ShortTitleLabel = new GUIStyle(EditorStyles.boldLabel);
            ShortTitleLabel.fontSize = 16;
            ShortTitleLabel.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.8f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
            ShortTitleLabel.alignment = TextAnchor.UpperLeft;

            MiddleLeftBoldMiniLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            MiddleLeftBoldMiniLabel.fontSize = 12;
            MiddleLeftBoldMiniLabel.normal.textColor = Color.white;
            MiddleLeftBoldMiniLabel.alignment = TextAnchor.MiddleLeft;

            BottomLeftBoldMiniLabel = new GUIStyle(MiddleLeftBoldMiniLabel);
            BottomLeftBoldMiniLabel.alignment = TextAnchor.LowerLeft;

            CenteredBigLabel = new GUIStyle(MiddleLeftBoldMiniLabel);
            CenteredBigLabel.fontSize = 13;
            CenteredBigLabel.alignment = TextAnchor.MiddleCenter;
            CenteredBigLabel.fontStyle = FontStyle.Normal;

            MiniGreyLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            MiniGreyLabel.alignment = TextAnchor.MiddleLeft;

            BoldMiniGreyLabel = new GUIStyle(MiniGreyLabel);
            BoldMiniGreyLabel.normal.textColor = new Color(0.65f,0.65f,0.65f, 1f);
            BoldMiniGreyLabel.fontStyle = FontStyle.Bold;
            BoldMiniGreyLabel.alignment = TextAnchor.UpperRight;

            FoldOutStyle = new GUIStyle(EditorStyles.foldout);
            FoldOutStyle.fontStyle = FontStyle.BoldAndItalic;
            FoldOutStyle.fontSize = 13;

            StandardButtonStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).button;
            StandardButtonStyle.fontStyle = FontStyle.Normal;
            StandardButtonStyle.alignment = TextAnchor.MiddleCenter;
            StandardButtonStyle.padding = new RectOffset(5, 0, 0, 0);
            StandardButtonStyle.fontSize = 13;
            StandardButtonStyle.normal.textColor = new Color(1f, 1f, 1f, 0.85f);

            LargeButtonStyle = new GUIStyle(StandardButtonStyle);
            LargeButtonStyle.padding.top = 6;
            LargeButtonStyle.padding.bottom = 6;

            HighlightColor = new Color(0.7f, 0.7f, 0.7f, 1f);
            HeaderColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        }

        private static void OnEditorUpdate()
        {
            if(EditorApplication.timeSinceStartup > m_OneSecondTimer + 1f)
            {
                if(OneSecondPassed != null)
                    OneSecondPassed();

                m_OneSecondTimer = EditorApplication.timeSinceStartup;
            }
        }

        public static void Separator(Color rgb, float thickness = 1)
        {
            Rect position = GUILayoutUtility.GetRect(GUIContent.none, m_SeparatorStyle, GUILayout.Height(thickness));

            if(Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = rgb;
                m_SeparatorStyle.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        public static void Separator(float thickness, GUIStyle splitterStyle)
        {
            Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));

            if(Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = SeparatorColor;
                splitterStyle.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        public static void Separator(float thickness = 1)
        {
            Separator(thickness, m_SeparatorStyle);
        }

        public static void Separator(Rect position, Color color)
        {
            if(Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = color;
                m_SeparatorStyle.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        public static void Separator(Rect position)
        {
            Separator(position, SeparatorColor);
        }

        public static void EnumPopupNonAlloc(Rect rect, SerializedProperty property, ref string[] names)
        {
            property.enumValueIndex = EditorGUI.Popup(rect, property.enumValueIndex, names);
        }

        public static int IndexOfString(string str, string[] allStrings)
        {
            for(int i = 0;i < allStrings.Length;i++)
            {
                if(allStrings[i] == str)
                    return i;
            }

            return 0;
        }

        public static string StringAtIndex(int i, string[] allStrings)
        {
            return allStrings.Length > i ? allStrings[i] : "";
        }

        public static void DrawSubProperties(SerializedProperty property)
        {
            foreach (SerializedProperty p in property)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {

                }
                else
                {
                    EditorGUILayout.PropertyField(p, true);
                }
            }
        }
    }
}