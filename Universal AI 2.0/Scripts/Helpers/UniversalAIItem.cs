//Darking Assets

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UniversalAI
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

    public enum ItemType
    {
        HealthKit,
        AmmoBox,
    }
    public class UniversalAIItem : MonoBehaviour
    {
        [HideInInspector] public bool Health;
        [HideInInspector] public bool Ammo;
        
        [Space] 
        public UniversalAIEnums.YesNo ItemCanBeFound = UniversalAIEnums.YesNo.Yes;
        
        [Space]
        public ItemType ItemType = ItemType.HealthKit;
        [Space]

        [Condition("Health", true, 0f)]
        [Range(1f, 999f)]
        public float HealthRefillAmount = 50f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        [Range(1, 100)]
        public float AmmoRefillAmount = 30f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        public UniversalAIShooterWeapon WeaponScript = null;
        [Space]
        
        public bool PlayAnimationOnArrival;
        
        [Condition("PlayAnimationOnArrival", true, 6f)]
        public string AnimationStateName = "Pick Up";

        [Space] 
        [Header("EVENTS")] 
        [Space]
        
        public UnityEvent OnAnimationStart = new UnityEvent();
        [Space]
        
        public UnityEvent OnPickedUp = new UnityEvent();
      
        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (GetComponent<SphereCollider>() == null)
            {
                gameObject.AddComponent<SphereCollider>().isTrigger = true;
            }
            
            if (ItemType == ItemType.AmmoBox)
            {
                Health = false;
                Ammo = true;
            }
            else if (ItemType == ItemType.HealthKit)
            {
                Health = true;
                Ammo = false;
            }
        }

        private UniversalAISystem _system;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<UniversalAINeedsSystem>() != null ||
                other.transform.root.GetComponent<UniversalAINeedsSystem>() != null)
            {
                UniversalAISystem system = other.GetComponent<UniversalAISystem>();

                if (system == null)
                {
                    system = other.transform.root.GetComponent<UniversalAISystem>();
                }
                
                if(system == null)
                    return;

                _system = system;

                if (!PlayAnimationOnArrival)
                {
                    Pickup();
                }
                else
                {
                    _system.Anim.Play(AnimationStateName);
                }
            }
        }
        
        public void Pickup()
        {
            if (ItemType == ItemType.HealthKit)
            {
                _system.UniversalAICommandManager.SetAIHealth(_system.UniversalAICommandManager.GetHealth() + HealthRefillAmount);
                _system.GetComponent<UniversalAINeedsSystem>().Searching = false;
                Destroy(gameObject);       
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UniversalAIItem))]
    public class UniversalAIItemEditor : Editor
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
            EditorGUILayout.LabelField("Universal AI Item", style, GUILayout.ExpandWidth(true));
            
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