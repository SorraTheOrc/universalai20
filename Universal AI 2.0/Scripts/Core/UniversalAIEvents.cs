    
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UniversalAI
{

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [Serializable]
    public class TheBoolEvent : UnityEvent<bool> {}
    
    [Serializable]
    public class TheFloatEvent : UnityEvent<float> {}
    public class UniversalAIEvents : MonoBehaviour
    {
        [Space] 
        [Help("You Need This Script To Use The AI Events.",HelpBoxMessageType.BigInfo)] 
        [Space]
        [Header("GENERAL EVENTS")] 
        [Space]

        public UnityEvent OnReady = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Setup Finishes.",HelpBoxMessageType.Info)]

        public TheBoolEvent OnFreeze = new TheBoolEvent();
        [Space] [Space] [Help("Plays After The AI Freezes Or UnFreezes.",HelpBoxMessageType.Info)]
        
        [Space]
        [Header("DETECTION EVENTS")] 
        [Space]
        
        public UnityEvent OnTargetVisible = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Detects A Target.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnTargetNotVisible = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Lost Target Vision.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnTargetLost = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Lost A Target.",HelpBoxMessageType.Info)]
        
        [Space]
        [Header("COMBAT EVENTS")] 
        [Space]

        public TheFloatEvent OnDealDamage = new TheFloatEvent();
        [Space] [Space] [Help("Plays After The AI Damages A Target.",HelpBoxMessageType.Info)]
        
        public TheFloatEvent OnTakeDamage = new TheFloatEvent();
        [Space] [Space] [Help("Plays After The AI Takes Damage.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnBlocked = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Blocks Attack.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnGetHit = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Gets Hit.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnAttack = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Attacks.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnDeath = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Dies.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnRevived = new UnityEvent();
        
        [Space]
        [Header("WEAPON EVENTS")] 
        [Space]
        
        public UnityEvent OnDraw = new UnityEvent();
        [Space] [Space] [Help("Plays After The AI Equips An Weapon.",HelpBoxMessageType.Info)]
        
        public UnityEvent OnHolster = new UnityEvent();
    }
    
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(UniversalAIEvents))]
    public class UniversalAIEventsEditor : Editor
    {
        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.fontSize = 13;
            EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
            EditorGUILayout.LabelField("Universal AI Events", style, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}