using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    [RequireComponent(typeof(BoxCollider))]
public class UniversalAIActionZone : MonoBehaviour
{
    [Space]
    [Header("GENERAL SETTINGS")]
    [Space]
    [Help("Please Check The Tutorial If You Didn't Understand Somewhere !", HelpBoxMessageType.BigInfo)]
    [Space]
    
    public UniversalAIEnums.YesNo ActionZoneEnabled = UniversalAIEnums.YesNo.Yes;
    [Space] [Space] [Help("Is This Action Zone Usable ?", HelpBoxMessageType.Info)]

    public float ActionStartDelay = 0f;
    [Space] [Space] [Help("The Starting Delay In Seconds Before The Action !", HelpBoxMessageType.Info)]
    
    public string ActionAnimatorStateName = String.Empty;
    [Space] [Space] [Help("The Action Animation State's Name In The Animator, Check The Tutorial For Details !", HelpBoxMessageType.Info)]

    public bool ExitAfterAnimationEnds = true;

    [Condition("ExitAfterAnimationEnds", false, 6f)]
    public float ExitActionDelay = 1f;
    
    [Space]
    
    [Condition("ExitAfterAnimationEnds", false, 6f)] [Tooltip("The Action Animation's Bool To Exit The Action !")]
    public string ActionBoolParameterName = String.Empty;

    private UniversalAISystem CurrentAISystem;
    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>().isKinematic = true;
        }

        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        UniversalAISystem system = other.GetComponent<UniversalAISystem>();
        
        if(CurrentAISystem != null || ActionZoneEnabled == UniversalAIEnums.YesNo.No)
            return;
     
        
        if (system == null)
        {
            system = other.transform.root.GetComponent<UniversalAISystem>();
        }
        
        if(system == null)
            return;

        CurrentAISystem = system;
        
        Invoke("StartAction", ActionStartDelay);
        
    }

    private void OnValidate()
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }
    }

    private BoxCollider _collider;
    private void OnDrawGizmos()
    {
        Color color = Color.yellow;
        color.a = 0.45f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, _collider.size);
    }

    public void StartAction()
    {
        if(CurrentAISystem == null)
            return;
        
        if (!ExitAfterAnimationEnds)
        {
            if (ActionBoolParameterName != String.Empty)
            {
                CurrentAISystem.Anim.SetBool(ActionBoolParameterName, true);
                Invoke("ExitAction", ExitActionDelay);
            }
            else
            {
                CurrentAISystem.Anim.SetBool(ActionBoolParameterName, false);
            }
        }
        else
        {
            CurrentAISystem.Anim.Play(ActionAnimatorStateName);
        }
        
        InvokeRepeating("CheckEnd", 0.3f, 0.1f);
    }

    public void CheckEnd()
    {
        if (CurrentAISystem == null)
        {
            CancelInvoke("CheckEnd");
        }
        else if(!CurrentAISystem.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
        {
            CancelInvoke("CheckEnd");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UniversalAISystem system = other.GetComponent<UniversalAISystem>();
        
        if (system != null)
        {
            if(system.Equals(CurrentAISystem))
                CurrentAISystem = null;
        }
    }

    public void ExitAction()
    {
        CurrentAISystem.Anim.SetBool(ActionBoolParameterName, false);
    }
}

#if UNITY_EDITOR
    
[CustomEditor(typeof(UniversalAIActionZone))]
public class UniversalAIActionsEditor : Editor
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
        EditorGUILayout.LabelField("Universal AI Action Zone", style, GUILayout.ExpandWidth(true));
            
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