using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniversalAI
{
    
#if UNITY_EDITOR
    using UnityEditor;
#endif

    
    [RequireComponent(typeof(UniversalAIDamageableObject))]
public class UniversalAIDismemberment : MonoBehaviour
{
    [Space]
    [Header("SETTINGS")]
    [Space]
    
    [Range(1f, 100f)] public float Durability = 100f;
    [Space] [Space] [Help("The Durability Amount Before The Limb Dismemberment !", HelpBoxMessageType.Info)]

    public List<GameObject> ObjectsToEnable = new List<GameObject>();
    [Space] [Space] [Help("The Objects To Enable On Dismemberment ! (Blood, Limb Prefab) ", HelpBoxMessageType.Info)]
    
    public List<GameObject> ObjectsToDisable = new List<GameObject>();

    [Space] [Space] [Help("The Objects To Disable On Dismemberment ! (Limb Mesh) ", HelpBoxMessageType.Info)]

    public bool NoMesh = false;
    [Space] [Space] [Help("Use this if you don't have seperated mesh ! (Disables Bone)", HelpBoxMessageType.Info)]
    
    public List<UniversalAIDismemberment> ChildLimbs = new List<UniversalAIDismemberment>();
    [Space] [Space] [Help("The Limbs That Will Also Fall When This Limb Dismembers !", HelpBoxMessageType.Info)]

    public UniversalAIEnums.YesNo KillAIOnDismemberment = UniversalAIEnums.YesNo.No;

    [Space] 
    [Header("EVENTS")] 
    [Space] 
    
    public UnityEvent OnTakeDamageEvent = new UnityEvent();
    [Space] [Space] [Help("The Take Damage Event !", HelpBoxMessageType.Info)]
    
    public UnityEvent OnDismembermentEvent = new UnityEvent();
    [Space] [Space] [Help("The Dismemberment Done Event !", HelpBoxMessageType.Info)]
    
    [ReadOnly]
    public UniversalAIDamageableObject DamageableObject;

    private void OnValidate()
    {
        if(Application.isPlaying)
            return;

        if (DamageableObject == null)
        {
            DamageableObject = GetComponent<UniversalAIDamageableObject>();
        }
    }

    private void Awake()
    {
        if (DamageableObject == null)
        {
            DamageableObject = GetComponent<UniversalAIDamageableObject>();
        }
       
        DamageableObject.PrivateDamageEvent.AddListener(TakeDamage);
    }

    public void TakeDamage(float damage)
    {

        if(Durability <= 0)
            return;

        Durability -= damage;

        OnTakeDamageEvent.Invoke();
        
        if (Durability <= 0)
        {
            Durability = 0;

            foreach (var enable in ObjectsToEnable)
            {
                enable.SetActive(true);
                if(enable.GetComponent<ParticleSystem>() == null)
                    enable.transform.SetParent(null);
            }
            
            foreach (var disable in ObjectsToDisable)
            {
                disable.SetActive(false);
            }

            if (NoMesh)
            {
                transform.localScale = Vector3.zero;
            }
            
            foreach (var childLimb in ChildLimbs)
            {
                childLimb.TakeDamage(9999);
            }

            if (ChildLimbs.Count <= 0 && transform.childCount > 0)
            {
                transform.localScale = Vector3.zero;
            }

            if (KillAIOnDismemberment == UniversalAIEnums.YesNo.Yes)
            {
                if (GetComponent<UniversalAIDamageableObject>() != null)
                {
                    GetComponent<UniversalAIDamageableObject>().TakeDamage(9999, AttackerType.Other, null);
                }
            }
            
            OnDismembermentEvent.Invoke();
        }
    }
}
#if UNITY_EDITOR
    
[CustomEditor(typeof(UniversalAIDismemberment))]
public class UniversalAIDismembermentEditor : Editor
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
        EditorGUILayout.LabelField("Universal AI Dismemberment", style, GUILayout.ExpandWidth(true));
            
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
