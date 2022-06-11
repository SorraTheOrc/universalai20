//Darking Assets

using System;
using UnityEngine;
using UnityEngine.AI;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    [CustomEditor(typeof(UniversalAISystem))]
    public class UniversalAISystemEditor : Editor
    {
        public UniversalAISystem AISystem;

        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};

        private void OnEnable()
        {
            AISystem = (UniversalAISystem)target;
        }

        private void OnValidate()
        {
            OnInspectorGUI();
        }

        private bool IgnoreNavmeshRadius = false;
#if !UniversalAI_Integration_PathfindingPro
        private bool IgnoreColliderRadius = false;
#endif
        public override void OnInspectorGUI()
        {
            AISystem.OnValidate();

            try
            {
                EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.fontSize = 13;
            EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
            EditorGUILayout.LabelField("Universal AI System", style, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
           
                //Check For Errors

            #region Error Checker

            AISystem.Reasons = String.Empty;
            AISystem.StartWithError = false;
            
#if !UniversalAI_Integration_PathfindingPro
        NavMeshHit hit;
#endif

#if !UniversalAI_Integration_PathfindingPro
            IgnoreColliderRadius = AISystem.IgnoreColliderRadius;
#endif
            IgnoreNavmeshRadius = AISystem.IgnoreNavmeshRadius;
            
              
                if (AISystem.UniversalAISounds == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'UniversalAISounds' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'UniversalAISounds' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if (AISystem.UniversalAIEvents == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'UniversalAIEvents' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'UniversalAIEvents' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if (AISystem.UniversalAICommandManager == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'UniversalAICommandManager' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'UniversalAICommandManager' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
#if UniversalAI_Integration_PathfindingPro
                else if (AISystem.NavPro == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'AI Path' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'AI Path' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
#else
                else if (AISystem.Nav == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'NavmeshAgent' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'NavmeshAgent' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
#endif
                else if (AISystem.Anim == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Animator' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'Animator' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if (AISystem.Anim.runtimeAnimatorController == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The AI animations aren't set up correctly, please make sure you applied the animations inside 'Animator Creator' component!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    
                    GUILayout.Space(3);
                        
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
 
                    if (GUILayout.Button("Open Documentation", GUILayout.Width(150),GUILayout.Height(25)))
                    {
                       Application.OpenURL("https://aidocs.darkingassets.com/get-started/create-your-first-ai/set-up-ai-animations");
                    }
                
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The AI animations aren't set up correctly, please make sure you applied the animations inside 'Animator Creator' component, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
//                 else if (AISystem.MainCollider == null)
//                 {
// #if UniversalAI_Integration_PathfindingPro
//                     EditorGUILayout.Space();
//                     GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
//                     EditorGUILayout.HelpBox("The 'Character Controller' component of the AI is null, please add one!", MessageType.Error);
//                     GUI.backgroundColor = Color.white;
//                     EditorGUILayout.Space();
//
//                     AISystem.Reasons =
//                         "The 'Character Controller' component of the AI is null, please add one!, disabling the AI: ' " +
//                         AISystem.gameObject.name + " ' !";
//                     AISystem.StartWithError = true;
// #else
//                     EditorGUILayout.Space();
//                     GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
//                     EditorGUILayout.HelpBox("The 'Collider' component of the AI is null, please add one!", MessageType.Error);
//                     GUI.backgroundColor = Color.white;
//                     EditorGUILayout.Space();
//
//                     AISystem.Reasons =
//                         "The 'Collider' component of the AI is null, please add one!, disabling the AI: ' " +
//                         AISystem.gameObject.name + " ' !";
//                     AISystem.StartWithError = true;
// #endif
//                 }
#if !UniversalAI_Integration_PathfindingPro
                else if (!NavMesh.SamplePosition(AISystem.transform.position, out hit, 1000.0f, NavMesh.AllAreas))
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The current scene doesn't have a valid 'Navmesh Area' baked, please follow the documentation to bake it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    
                    GUILayout.Space(3);
                        
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
 
                    if (GUILayout.Button("Open Documentation", GUILayout.Width(150),GUILayout.Height(25)))
                    {
                        Application.OpenURL("https://aidocs.darkingassets.com/get-started/installation-and-set-up#set-up-your-scene");
                    }
                
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The current scene doesn't have a valid 'Navmesh Area' baked, please follow the documentation to bake it, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
#endif
                else if (AISystem.General.wanderType == UniversalAIEnums.WanderType.Waypoint && AISystem.GetComponent<UniversalAIWaypoints>() == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'UniversalAIWaypoints' component of the AI is null, please add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "The 'UniversalAIWaypoints' component of the AI is null, please add one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if (AISystem.General.wanderType == UniversalAIEnums.WanderType.Waypoint && AISystem.GetComponent<UniversalAIWaypoints>() != null && AISystem.GetComponent<UniversalAIWaypoints>().Waypoints.Length < 2)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("There is currently not enough waypoints for the AI, please navigate to 'Waypoint System' script on the AI to create one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    AISystem.Reasons =
                        "There is currently not enough waypoints for the AI, please navigate to 'Waypoint System' script on the AI to create one!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if (AISystem.General.AIConfidence == UniversalAIEnums.AIConfidence.Coward && AISystem.General.FleeDistance <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The Flee Distance Is Lower Than 0, Please Increase It!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The Flee Distance Is Lower Than 0, Please Increase It!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if(AISystem.Detection.DetectionSettings.DetectionLayers == LayerMask.GetMask("Nothing"))
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Detection Layer' Is Null! Please Navigate to: 'Detection / Detection Settings' to fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'Detection Layer' Is Null!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if(AISystem.Detection.DetectionSettings.HeadTransform == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Head Transform' Is Null! Please Navigate to: 'Detection / Detection Settings' to add one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'Head Transform' Is Null!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if(AISystem.InverseKinematics.UseLookAtIK == UniversalAIEnums.YesNo.Yes && AISystem.InverseKinematics.LookAtLayers == LayerMask.GetMask("Nothing"))
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'LookAt Layer' Is Null! Please Navigate to: 'InverseKinematics / Look At IK' to fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'LookAt Layer' Is Null!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                else if(AISystem.InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && AISystem.InverseKinematics.humanBones.Count <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Human Bones' Are Empty! Please Navigate to: 'InverseKinematics / Aim At IK' to add bones!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'Human Bones' Are Empty!, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                // else if(AISystem.Settings.Attack.AttackDistance <= AISystem.Settings.Movement.TooCloseDistance)
                // {
                //     EditorGUILayout.Space();
                //     GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                //     EditorGUILayout.HelpBox("The 'Attack Distance' Is Lower Than The 'Too Close Distance', AI Won't Work Properly!", MessageType.Error);
                //     GUI.backgroundColor = Color.white;
                //     EditorGUILayout.Space();
                //     
                //     AISystem.Reasons =
                //         "The 'Attack Distance' Is Lower Than The 'Too Close Distance', AI Won't Work Properly, disabling the AI: ' " +
                //         AISystem.gameObject.name + " ' !";
                //     AISystem.StartWithError = true;
                // }
                // else if(AISystem.Settings.Attack.MaxDamageDistance <= AISystem.Settings.Attack.AttackDistance)
                // {
                //     EditorGUILayout.Space();
                //     GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                //     EditorGUILayout.HelpBox("The 'Max Damage Distance' Is Lower Than The 'Attack Distance', AI Won't Work Properly!", MessageType.Error);
                //     GUI.backgroundColor = Color.white;
                //     EditorGUILayout.Space();
                //     
                //     AISystem.Reasons =
                //         "The 'Max Damage Distance' Is Lower Than The 'Attack Distance', AI Won't Work Properly, disabling the AI: ' " +
                //         AISystem.gameObject.name + " ' !";
                //     AISystem.StartWithError = true;
                // }
                else if(AISystem.Detection.DetectionSettings.DetectionDistance <= AISystem.Settings.Attack.AttackDistance)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Detection Distance' Is Lower Than The 'Attack Distance', AI Won't Work Properly!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'Detection Distance' Is Lower Than The 'Attack Distance', AI Won't Work Properly, disabling the AI: ' " +
                        AISystem.gameObject.name + " ' !";
                    AISystem.StartWithError = true;
                }
                // else if(AISystem.Detection.DetectionSettings.DetectionDistance <= AISystem.Settings.Attack.MaxDamageDistance)
                // {
                //     EditorGUILayout.Space();
                //     GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                //     EditorGUILayout.HelpBox("The 'Max Damage Distance' Is Lower Than The 'Attack Distance', AI Won't Work Properly!", MessageType.Error);
                //     GUI.backgroundColor = Color.white;
                //     EditorGUILayout.Space();
                //     
                //     AISystem.Reasons =
                //         "The 'Detection Distance' Is Lower Than The 'Max Damage Distance', AI Won't Work Properly, disabling the AI: ' " +
                //         AISystem.gameObject.name + " ' !";
                //     AISystem.StartWithError = true;
                // }
                else if(AISystem.TagStorage != null && !AISystem.TagStorage.AvailableAITags.Contains(AISystem.gameObject.tag))
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'AI Object's Tag' is not an AI tag, AI will be undetectable!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons =
                        "The 'AI Object's Tag' is not an AI tag, AI will be undetectable.";
                    AISystem.StartWithError = false;
                }
#if !UniversalAI_Integration_PathfindingPro
                else if(AISystem.Nav.radius.Equals(0.5f) && AISystem.Nav.height.Equals(2f) && !IgnoreNavmeshRadius)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("Make sure to adjust the 'NavmeshAgent' radius & height to make it fit to your AI object!", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                    {
                        IgnoreNavmeshRadius = true;
                        AISystem.IgnoreNavmeshRadius = true;
                    }
                    
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons = String.Empty;
                    AISystem.StartWithError = false;
                }
#else
 else if(AISystem.NavPro.radius.Equals(0.5f) && AISystem.NavPro.height.Equals(2f) && !IgnoreNavmeshRadius)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("Make sure to adjust the 'AI Path' radius & height to make it fit to your AI object!", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                    {
                        IgnoreNavmeshRadius = true;
                    }
                    
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    EditorGUILayout.Space();
                    
                    AISystem.Reasons = String.Empty;
                    AISystem.StartWithError = false;
                }
#endif
#if !UniversalAI_Integration_PathfindingPro
                else if(AISystem.MainCollider != null && !IgnoreColliderRadius)
                {
                    if (AISystem.MainCollider is CapsuleCollider capsuleCollider)
                    {
                        if (capsuleCollider != null)
                        {
                            if (capsuleCollider.radius.Equals(0.5f) && capsuleCollider.height.Equals(1f))
                            {
                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                                EditorGUILayout.HelpBox("Make sure to adjust the 'Collider' radius & height to make it fit to your AI object!", MessageType.Warning);
                                GUI.backgroundColor = Color.white;
                    
                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                    
                                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                                {
                                    IgnoreColliderRadius = true;
                                    AISystem.IgnoreColliderRadius = true;
                                }
                    
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                    
                                EditorGUILayout.Space();
                    
                                AISystem.Reasons = String.Empty;
                                AISystem.StartWithError = false;
                            }
                        }
                    }
                    
                    if (AISystem.MainCollider is BoxCollider boxCollider)
                    {
                        if (boxCollider != null)
                        {
                            if (boxCollider.size.Equals(Vector3.one))
                            {
                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                                EditorGUILayout.HelpBox("Make sure to adjust the 'Collider' size to make it fit to your AI object!", MessageType.Warning);
                                GUI.backgroundColor = Color.white;
                    
                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                    
                                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                                {
                                    IgnoreColliderRadius = true;
                                    AISystem.IgnoreColliderRadius = true;
                                }
                    
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                    
                                EditorGUILayout.Space();
                    
                                AISystem.Reasons = String.Empty;
                                AISystem.StartWithError = false;
                            }
                        }
                    }
                    
                    if (AISystem.MainCollider is SphereCollider sphereCollider)
                    {
                        if (sphereCollider != null)
                        {
                            if (sphereCollider.radius.Equals(0.5f))
                            {
                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                                EditorGUILayout.HelpBox("Make sure to adjust the 'Collider' radius to make it fit to your AI object!", MessageType.Warning);
                                GUI.backgroundColor = Color.white;
                    
                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();
                    
                                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                                {
                                    IgnoreColliderRadius = true;
                                    AISystem.IgnoreColliderRadius = true;
                                }
                    
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                    
                                EditorGUILayout.Space();
                    
                                AISystem.Reasons = String.Empty;
                                AISystem.StartWithError = false;
                            }
                        }
                    }
                }
#endif
                else
                {
                    AISystem.Reasons = String.Empty;
                    AISystem.StartWithError = false;
                }
              
                #endregion
                
                serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();
            
            
            
            if(!Application.isPlaying)
                return;
            
            GUILayout.Space(20);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            
            
            if (AISystem.Health > 0)
            {
                if (GUILayout.Button("KILL AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                    if(!Application.isPlaying)
                        return;
                    
                    AISystem.Die();
                }
            }
            else if(AISystem.General.DeathType != UniversalAIEnums.DeathMethod.Ragdoll && !AISystem.IsWeapon)
            {
                if (GUILayout.Button("REVIVE AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                    if(!Application.isPlaying)
                        return;
            
                    AISystem.enabled = true;
                    AISystem.Revive();
                }
            }
            else if(AISystem.General.DeathType == UniversalAIEnums.DeathMethod.Ragdoll)
            {
                if (GUILayout.Button("CANT REVIVE RAGDOLL AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                  
                }
            }
            else if(AISystem.IsWeapon)
            {
                if (GUILayout.Button("CANT REVIVE WEAPON AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                  
                }
            }
            
            if (!AISystem.Frozen)
            {
                if (GUILayout.Button("FREEZE AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                    AISystem.FreezeAIManuel(true);
                } 
            }
            else
            {
                if (GUILayout.Button("UNFREEZE AI",GUILayout.Width(240),GUILayout.Height(20)))
                {
                    AISystem.FreezeAIManuel(false);
                } 
            }

            GUILayout.EndHorizontal();
            }
            catch
            {
               
            }
        }
    }
    
}