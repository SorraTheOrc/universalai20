using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UniversalAI
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [ExecuteAlways]
    public class UniversalAIWaypoints : MonoBehaviour
    {
        [Header("SETTINGS")] [Space(5)] [Tooltip("The Waypoint Type Of The AI.")]
        public float StartRunningDistance = 8f;

        [Space(5)] [Tooltip("The Waypoint Type Of The AI.")]
        public UniversalAIEnums.WaypointType WaypointType = UniversalAIEnums.WaypointType.InOrder;

        [Space(5)] [Reorderable(false)] public WaypointOverrideList Waypoints = new WaypointOverrideList();


        [Serializable]
        public class WaypointOverrideList : ReorderableArray<WaypointOverride>
        {
        }

        [Serializable]
        public class WaypointOverride
        {
            [HideInInspector] public string NameRef = "Waypoint";

            public Vector3 WaypointPosition;

            [Space(5)] [Tooltip("Should AI Wait On Arrival?")]
            public bool WaitOnArrival = true;

            [Space(5)] [Condition("WaitOnArrival", true, 6f)]
            public int MinRandomWaitLength = 4;

            [Condition("WaitOnArrival", true, 6f)] public int MaxRandomWaitLength = 8;

        }
        
       [HideInInspector] public int ListLength; 
       [HideInInspector] public bool Clicking = false;


        private void LateUpdate()
        {

            if (ListLength.Equals(Waypoints.Length) && !Waypoints[Waypoints.Count - 1].WaypointPosition.Equals(Vector3.zero))
            {
                Clicking = false;
            }
            
            if(Application.isPlaying)
                return;

            if (!Waypoints.Length.Equals(ListLength))
            {
                if (ListLength < Waypoints.Length)
                {
                    Waypoints[Waypoints.Length - 1].WaypointPosition = Vector3.zero;
                    Waypoints[Waypoints.Length - 1].WaitOnArrival = false;
                    Clicking = true;
                }
                else if(Clicking)
                {
                    Clicking = false;
                }

                ListLength = Waypoints.Length;
            }

#if UNITY_EDITOR
            if (Clicking)
            {
                Selection.activeGameObject = gameObject;
                SetSceneViewGizmos(true);
            }
#endif
        }

        public void Done()
        {
            if(Application.isPlaying)
                return;
            
#if UNITY_EDITOR

            RaycastHit hit = new RaycastHit();
            if (Camera.main == null)
            {
                Debug.LogError("There must be at least 1 camera in the scene for creating waypoints!");
                return;
            }

            Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Waypoints[Waypoints.Length - 1].WaypointPosition = hit.point;
                Clicking = false;
                SetSceneViewGizmos(false);
            }
#endif

        }

        public static void SetSceneViewGizmos(bool gizmosOn)
        {
            if(Application.isPlaying)
                return;
            
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            sv.drawGizmos = gizmosOn;
#endif
        }
        public static bool GetSceneViewGizmosEnabled()
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            return sv.drawGizmos;
#else
        return false;
#endif
        }
        
    }
#if UNITY_EDITOR
        [CustomEditor(typeof(UniversalAIWaypoints))]
        public class UniversalAIWaypointsEditor : Editor
        {
            private static readonly string[] _dontIncludeMe = new string[] {"m_Script"};

            public override void OnInspectorGUI()
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
                style.fontSize = 13;
                EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style,
                    GUILayout.ExpandWidth(true), GUILayout.Height(43));
                EditorGUILayout.LabelField("Universal AI Waypoints", style, GUILayout.ExpandWidth(true));

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                
                if (((UniversalAIWaypoints) target).Clicking && !Application.isPlaying)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.HelpBox(
                        "Please click on somewhere in your scene to create waypoint!",
                        MessageType.Info);
                    GUI.backgroundColor = Color.white;

                    GUILayout.Space(3);
                    GUILayout.Space(10f);
                }
                
                serializedObject.Update();

                DrawPropertiesExcluding(serializedObject, _dontIncludeMe);

                serializedObject.ApplyModifiedProperties();
            }

            private void OnSceneGUI()
            {
                if(Application.isPlaying)
                    return;
                
                if (((UniversalAIWaypoints) target).Clicking)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    if (Event.current.type.Equals(EventType.MouseDown) && Event.current.button.Equals(0))
                    {
                        ((UniversalAIWaypoints)target).Done();
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                
            }
        }

#endif
    }