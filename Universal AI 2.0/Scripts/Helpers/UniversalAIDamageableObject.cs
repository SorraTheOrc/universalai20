
namespace UniversalAI
{
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
   

#if UNITY_EDITOR
using UnityEditor;
#endif

    public class UniversalAIDamageableObject : MonoBehaviour, UniversalAIDamageable
    {

        [HideInInspector] public TheFloatEvent PrivateDamageEvent = new TheFloatEvent();
        
        [Space] 
        
        public UniversalAIEnums.YesNo CanReceiveDamage = UniversalAIEnums.YesNo.Yes;
        [Space] [Space] [Help("Is Taking Damage Enabled For This Damageable object ?",HelpBoxMessageType.Info)]
        
        public AttackerTypeCheck CanReceiveDamageFrom = AttackerTypeCheck.All;
        [Space] [Space] [Help("Which Attacker Types Can Damage This Damageable Object ?",HelpBoxMessageType.Info)]
        
        public float DamageMultiplier = 1f;
        [Space] [Space] [Help("Damage Multiplier For This Damageable Object !",HelpBoxMessageType.Info)]
        
        public UniversalAIEnums.YesNo KillAIInstant = UniversalAIEnums.YesNo.No;
        [Space] [Space] [Help("Is This Body Part Will Kill The AI When Damaged ?",HelpBoxMessageType.Info)]
        
        [ReadOnly] public UniversalAISystem UniversalAISystem;

        private void OnValidate()
        {
            if (UniversalAISystem == null)
            {

#if AI_Puppet_Integration
                
                if(transform.root == null)
                    return;

                UniversalAISystem = transform.root.GetComponentInChildren<UniversalAISystem>();
               
#else
      
 if(transform.parent == null)
                    return;
                
                Transform parentt = transform.parent;

                if(parentt == null)
                    return;
                
                while (parentt.GetComponent<UniversalAISystem>() == null)
                {
                    if(parentt == null)
                        break;
                    
                    parentt = parentt.parent;
                    
                    if(parentt.parent == null)
                        break;
                }
                
                if(parentt == null)
                    return;
                UniversalAISystem = parentt.GetComponent<UniversalAISystem>();

#endif
            }
        }

        bool Convertor(AttackerType type1, AttackerTypeCheck type2)
        {
            if (type2 == AttackerTypeCheck.All)
                return true;
            
            if (type1 == AttackerType.AI && type2 == AttackerTypeCheck.AI)
                return true;
            
            if (type1 == AttackerType.Other && type2 == AttackerTypeCheck.Other)
                return true;
            
            if (type1 == AttackerType.Player && type2 == AttackerTypeCheck.Player)
                return true;


            return false;
        }
        public void TakeDamage(float damageamount, AttackerType attackerType, GameObject Attacker, bool BlockSuccess = true)
        {
           
            if (Convertor(attackerType, CanReceiveDamageFrom) && CanReceiveDamage == UniversalAIEnums.YesNo.Yes)
            {
                if (KillAIInstant == UniversalAIEnums.YesNo.Yes)
                {
                    UniversalAISystem.TakeDamage(9999, attackerType, Attacker);
                }
                else
                {
                    UniversalAISystem.TakeDamage(damageamount * DamageMultiplier, attackerType, Attacker); 
                    PrivateDamageEvent.Invoke(damageamount * DamageMultiplier);
                }
            }
        }
    }
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(UniversalAIDamageableObject))]
    public class UniversalAIDamageableEditor : Editor
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
            EditorGUILayout.LabelField("Generic AI Damageable", style, GUILayout.ExpandWidth(true));
            
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