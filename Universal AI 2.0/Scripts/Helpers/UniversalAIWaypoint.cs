using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    [ExecuteAlways]
public class UniversalAIWaypoint : MonoBehaviour
{
     [Space]
     public UniversalAIEnums.YesNo EnableGizmos = UniversalAIEnums.YesNo.Yes;
     [Space]
     
     public UniversalAIEnums.YesNo AlignWithFloor = UniversalAIEnums.YesNo.Yes;

     [Space]
     [Header("EVENTS")] 
     [Space] 
     
     public UnityEvent OnReachedWaypoint = new UnityEvent();
     [Space]
     
     public UnityEvent OnMovingToWaypoint = new UnityEvent();

     [HideInInspector] public Transform NextWaypoint;

     private void LateUpdate()
     {
         if (AlignWithFloor == UniversalAIEnums.YesNo.Yes)
         {
             RaycastHit hit;

             if (Physics.Raycast(transform.position, Vector3.down, out hit))
             {
                 if(hit.transform.gameObject == null)
                     return;
                 
                 if(hit.transform.gameObject == gameObject)
                     return;
                 
               
                 transform.position = new Vector3 (transform.position.x, (transform.position.y - hit.distance) + 0.1f, transform.position.z);
             }
         }
     }

     private void OnDrawGizmos()
    {
        if(EnableGizmos == UniversalAIEnums.YesNo.No)
            return;
        
        if (NextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, NextWaypoint.transform.position);
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);   
    }

     private void Start() // To disable the script
     {
     }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(UniversalAIWaypoint))]
public class UniversalAIWaypointEditor : Editor
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
        EditorGUILayout.LabelField("Universal AI Waypoint", style, GUILayout.ExpandWidth(true));
            
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