using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;


namespace UniversalAI
{
   
    public class UniversalAIIntegrationsEditor : EditorWindow
    {
        Texture Icon;
        Vector2 scroll;

        public UniversalAIEnums.YesNo EnableHQFPS = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnableSTP = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnableNeofps = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnableInvector = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnableMmfps = UniversalAIEnums.YesNo.No;
        
        // public UniversalAIEnums.YesNo EnableUSK = UniversalAIEnums.YesNo.No;
        // private bool USKEnabled = false;
        
        public UniversalAIEnums.YesNo EnableHorrorFPS = UniversalAIEnums.YesNo.No;
        private bool HorrorFPSEnabled = false;
        
        // public UniversalAIEnums.YesNo EnablePuppet = UniversalAIEnums.YesNo.No;
        // private bool PuppetmasterEnabled = false;
        
        public UniversalAIEnums.YesNo EnablePlaymaker = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnablePathPro = UniversalAIEnums.YesNo.No;
        private bool PathProEnabled = false;
        
        public UniversalAIEnums.YesNo EnableEasySave = UniversalAIEnums.YesNo.No;
        private bool EasySaveEnabled = false;
        
        public UniversalAIEnums.YesNo EnableOpsive = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.YesNo EnableGKC = UniversalAIEnums.YesNo.No;
        private bool GKCEnabled = false;
        
        private string CurrentIntegration = "Null";

        public UniversalAISystem AISystem;
        void OnInspectorUpdate()
        {
            Repaint();
        }

        [MenuItem("Tools/Universal AI/AI Wizards/Integration Manager", false, 200)]
        public static void ShowWindow()
        {
            EditorWindow APS = EditorWindow.GetWindow(typeof(UniversalAIIntegrationsEditor), false, "Integration Manager");
            APS.minSize = new Vector2(440f, 605f);
        }

        void OnEnable()
        {
            if (Icon == null) Icon = Resources.Load("LogoBrain") as Texture;
    

#if UniversalAI_Integration_USK
            USKEnabled = true;
            EnableUSK = UniversalAIEnums.YesNo.Yes;
#endif
            
#if UniversalAI_Integration_HQFPS
            EnableHQFPS = UniversalAIEnums.YesNo.Yes;
#endif
            
#if UniversalAI_Integration_STP
            EnableSTP = UniversalAIEnums.YesNo.Yes;
#endif
            
#if UniversalAI_Integration_HORRORFPS
            EnableHorrorFPS = UniversalAIEnums.YesNo.Yes;
            HorrorFPSEnabled = true;
#endif

#if UniversalAI_Integration_NEOFPS
            EnableNeofps = UniversalAIEnums.YesNo.Yes;
#endif

#if UniversalAI_Integration_GKC
            EnableGKC = UniversalAIEnums.YesNo.Yes;
            GKCEnabled = true;
#endif

#if UniversalAI_Integration_INVECTOR
            EnableInvector = UniversalAIEnums.YesNo.Yes;
#endif

#if UniversalAI_Integration_OPSIVE
            EnableInvector = UniversalAIEnums.YesNo.Yes;
#endif

#if UniversalAI_Integration_PathfindingPro
            PathProEnabled = true;
            EnablePathPro = UniversalAIEnums.YesNo.Yes;
#endif

// #if UniversalAI_Integration_Puppetmaster
//             PuppetmasterEnabled = true;
//             EnablePuppet = UniversalAIEnums.YesNo.Yes;
// #endif

#if UniversalAI_Integration_PLAYMAKER
            EnablePlaymaker = UniversalAIEnums.YesNo.Yes;
#endif

#if UniversalAI_Integration_MMFPSE
            EnableMmfps = UniversalAIEnums.YesNo.Yes;
#endif

        }
        
       

        void OnGUI()
        {
            GUILayout.Space(15);

           
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box");
            GUILayout.FlexibleSpace();
            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
            EditorGUILayout.LabelField(new GUIContent(Icon), style, GUILayout.ExpandWidth(true),
                GUILayout.Height(32));
            EditorGUILayout.LabelField("Universal AI Integrations Editor", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox(
                "The Universal AI Integrations Editor allows you to Add any AI Integration really easily with the given settings!",
                MessageType.Info, true);

            GUILayout.Space(4);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Space(10);

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
         
            scroll = EditorGUILayout.BeginScrollView(scroll);

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Controller Integrations", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);

            // EnableSoundDetection = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use Sound Detection", EnableSoundDetection);
            // GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            // EditorGUILayout.HelpBox("This will automatically enable the sound detection option for the controller.", MessageType.None, true);
            // GUI.backgroundColor = Color.white;
            // EditorGUI.EndDisabledGroup();
            //
            // GUILayout.Space(10);
            
#if !HQ_FPS_TEMPLATE
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'HQ FPS Template' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableHQFPS = UniversalAIEnums.YesNo.No;
#endif
            EnableHQFPS = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'HQ FPS' Integration", EnableHQFPS);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'HQ FPS Template' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            
#if !SURVIVAL_TEMPLATE_PRO
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'Survival Template PRO' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableSTP = UniversalAIEnums.YesNo.No;
#endif
            EnableSTP = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'STP' Integration", EnableSTP);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Survival Template PRO' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            
#if !INVECTOR_MELEE && !INVECTOR_SHOOTER
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'Invector Melee Or Shooter' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableInvector = UniversalAIEnums.YesNo.No;
#endif
            EnableInvector = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'Invector' Integration", EnableInvector);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Invector Melee / Shooter' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            
#if !NEOFPS
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'NEO FPS' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableNeofps = UniversalAIEnums.YesNo.No;
#endif
            EnableNeofps = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'NEO FPS' Integration", EnableNeofps);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Neo FPS' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();
            
            GUILayout.Space(10);

#if !INTEGRATION_FPV2NEWER
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'MMFPSE' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableMmfps = UniversalAIEnums.YesNo.No;
#endif
            EnableMmfps = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'MMFPSE' Integration", EnableMmfps);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'MMFPSE' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            
#if !FIRST_PERSON_CONTROLLER && !THIRD_PERSON_CONTROLLER
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("Any 'Opsive' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnableOpsive = UniversalAIEnums.YesNo.No;
#endif
            
            EnableOpsive = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'Opsive' Integration", EnableOpsive);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Opsive' assets integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();
            
            GUILayout.Space(10);
            
            EnableGKC = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'GKC' Integration", EnableGKC);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Game Kit Controller' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            if (EnableGKC == UniversalAIEnums.YesNo.Yes)
            {
                if (!GKCEnabled)
                {
                    if (EditorUtility.DisplayDialog("Are You Sure? ",
                            "Are You Sure To Continue? Please don't forget to import the 'Game Controller Kit' asset from package manager first!",
                            "Go Ahead!", "Cancel"))
                    {
                        GKCEnabled = true;
                    }
                    else
                    {
                        EnableGKC = UniversalAIEnums.YesNo.No;
                    }   
                }
            }
            
            GUILayout.Space(10);
            
            EnableHorrorFPS = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'HorrorFps' Integration", EnableHorrorFPS);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Horror FPS Kit' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();
            
            if (EnableHorrorFPS == UniversalAIEnums.YesNo.Yes)
            {
                if (!HorrorFPSEnabled)
                {
                    if (EditorUtility.DisplayDialog("Are You Sure? ",
                        "Are You Sure To Continue? Please don't forget to import the 'Horror FPS Kit' asset from package manager first!",
                        "Go Ahead!", "Cancel"))
                    {
                        HorrorFPSEnabled = true;
                    }
                    else
                    {
                        EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                    }   
                }
            }
            
            // GUILayout.Space(10);
            //
            // EnableUSK = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("Use 'USK' Integration", EnableUSK);
            // GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            // EditorGUILayout.HelpBox("This will enable the 'Universal Shooter Kit' asset integration for the AIs.", MessageType.None, true);
            // GUI.backgroundColor = Color.white;
            //
            // if (EnableUSK == UniversalAIEnums.YesNo.Yes)
            // {
            //     if (!USKEnabled)
            //     {
            //         if (EditorUtility.DisplayDialog("Are You Sure? ",
            //             "Are You Sure To Continue? Please don't forget to import the 'Universal Shooter Kit' asset from package manager first!",
            //             "Go Ahead!", "Cancel"))
            //         {
            //             USKEnabled = true;
            //         }
            //         else
            //         {
            //             EnableUSK = UniversalAIEnums.YesNo.No;
            //         }   
            //     }
            // }


            GUILayout.Space(17);
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Misc Integrations", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);
            
#if !PLAYMAKER
            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
            EditorGUILayout.LabelField("'Playmaker' asset isn't imported in this project.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            
            EditorGUI.BeginDisabledGroup(true);

            EnablePlaymaker = UniversalAIEnums.YesNo.No;
#endif
            
            EnablePlaymaker = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("'Playmaker' Integration", EnablePlaymaker);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Playmaker' asset integration!", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
            
            EnableEasySave = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("'Easy Save 3' Integration", EnableEasySave);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'Easy Save 3' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            
            if (EnableEasySave == UniversalAIEnums.YesNo.Yes)
            {
                if (!EasySaveEnabled)
                {
                    if (EditorUtility.DisplayDialog("Are You Sure? ",
                        "Are You Sure To Continue? Please don't forget to import the 'Easy Save' asset from package manager first!",
                        "Go Ahead!", "Cancel"))
                    {
                        EasySaveEnabled = true;
                        AddRemoveDefine("UniversalAI_Integration_EasySave", true);
                    }
                    else
                    {
                        EnableEasySave = UniversalAIEnums.YesNo.No;
                    }   
                }
            }
            
            GUILayout.Space(10);
            
            EnablePathPro = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("'A* Pathfinding' Integration", EnablePathPro);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("This will enable the 'A* Pathfinding Project Pro' asset integration for the AIs.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            
            if (EnablePathPro == UniversalAIEnums.YesNo.Yes)
            {
                if (!PathProEnabled)
                {
                    if (EditorUtility.DisplayDialog("Are You Sure? ",
                        "Are You Sure To Continue? Please don't forget to import the 'A* Pathfinding Project Pro' asset from package manager first!",
                        "Go Ahead!", "Cancel"))
                    {
                        PathProEnabled = true;
                        AddRemoveDefine("UniversalAI_Integration_PathfindingPro", true);
                    }
                    else
                    {
                        EnablePathPro = UniversalAIEnums.YesNo.No;
                    }   
                }
            }

            // GUILayout.Space(10);
            //
            // EnablePuppet = (UniversalAIEnums.YesNo)  EditorGUILayout.EnumPopup("'PuppetMaster' Integration", EnablePuppet);
            // GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            // EditorGUILayout.HelpBox("This will enable the 'Puppet Master' asset integration for the AIs.", MessageType.None, true);
            // GUI.backgroundColor = Color.white;
            //
            // if (EnablePuppet == UniversalAIEnums.YesNo.Yes)
            // {
            //     if (!PuppetmasterEnabled)
            //     {
            //         if (EditorUtility.DisplayDialog("Are You Sure? ",
            //             "Are You Sure To Continue? Please don't forget to import the 'Puppet Master' asset from package manager first!",
            //             "Go Ahead!", "Cancel"))
            //         {
            //             PuppetmasterEnabled = true;
            //             AddRemoveDefine("UniversalAI_Integration_Puppetmaster", true);
            //         }
            //         else
            //         {
            //             EnablePuppet = UniversalAIEnums.YesNo.No;
            //         }   
            //     }
            // }

            EditorGUILayout.EndVertical();
            GUILayout.Space(20);
            EditorGUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            GUILayout.Space(30);
            
            CheckForIntegrations();
            
            
            // if (EnableUSK == UniversalAIEnums.YesNo.No)
            // {
            //     USKEnabled = false;
            // }

            // if (EnablePuppet == UniversalAIEnums.YesNo.No && PuppetmasterEnabled)
            // {
            //     PuppetmasterEnabled = false;
            // }

            if (EnablePathPro == UniversalAIEnums.YesNo.No && PathProEnabled)
            {
                AddRemoveDefine("UniversalAI_Integration_PathfindingPro", false);
                PathProEnabled = false;
            }
            
            if (EnablePlaymaker == UniversalAIEnums.YesNo.Yes)
            {
                AddRemoveDefine("UniversalAI_Integration_PLAYMAKER", true);
            }
            else
            {
                AddRemoveDefine("UniversalAI_Integration_PLAYMAKER", false);
            }
            
            if (EnableEasySave == UniversalAIEnums.YesNo.No && EasySaveEnabled)
            {
                AddRemoveDefine("UniversalAI_Integration_EasySave", false);
                PathProEnabled = false;
            }
            
            if (EnableHorrorFPS == UniversalAIEnums.YesNo.No && HorrorFPSEnabled)
            {
                HorrorFPSEnabled = false;
            }
            
            if (EnableGKC == UniversalAIEnums.YesNo.No && GKCEnabled)
            {
                GKCEnabled = false;
            }

        }

        
        private void AddRemoveDefine(string DefineName, bool add)
        {
        
            List<string> Symbols = new List<string>();
            Symbols.Add(DefineName);
      
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
        
            if(add)
                allDefines.AddRange(Symbols.Except(allDefines));

            if (!add && allDefines.Contains(DefineName))
            {
                allDefines.Remove(DefineName);
            }
        
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }

        private string OldIntegration = "Null";
        
        
        private void CheckForIntegrations()
        {
            if (CurrentIntegration.Equals("Null"))
            {
                if (EnableHQFPS == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "HQFPS";
                }
                
                if (EnableSTP == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "STP";
                }
                
                if (EnableInvector == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "INVECTOR";
                }
                
                if (EnableNeofps == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "NEOFPS";
                }
                
                if (EnableMmfps == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "MMFPSE";
                }
                
                if (EnableOpsive == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "OPSIVE";
                }
                
                if (EnableGKC == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "GKC";
                }

                // if (EnableUSK == UniversalAIEnums.YesNo.Yes)
                // {
                //     CurrentIntegration = "USK";
                // }
                
                if (EnableHorrorFPS == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "HORRORFPS";
                }
                
                return;
            }
            else if(EnableSTP == UniversalAIEnums.YesNo.No && EnableHorrorFPS == UniversalAIEnums.YesNo.No && EnableInvector == UniversalAIEnums.YesNo.No && /* EnableUSK == UniversalAIEnums.YesNo.No && */ EnableOpsive == UniversalAIEnums.YesNo.No && EnableHQFPS == UniversalAIEnums.YesNo.No && EnableGKC == UniversalAIEnums.YesNo.No && EnableNeofps == UniversalAIEnums.YesNo.No && EnableMmfps == UniversalAIEnums.YesNo.No)
            {
                AddRemoveDefine("UniversalAI_Integration_" + OldIntegration, false);

                CurrentIntegration = "Null";

                OldIntegration = CurrentIntegration;
                return;
            }
               
            
            if (!CurrentIntegration.Equals("HQFPS"))
            {
                if (EnableHQFPS == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "HQFPS";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("STP"))
            {
                if (EnableSTP == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "STP";
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("HORRORFPS"))
            {
                if (EnableHorrorFPS == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "HORRORFPS";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("INVECTOR"))
            {
                if (EnableInvector == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "INVECTOR";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            // if (!CurrentIntegration.Equals("USK"))
            // {
            //     if (EnableUSK == UniversalAIEnums.YesNo.Yes)
            //     {
            //         CurrentIntegration = "USK";
            //         EnableOpsive = UniversalAIEnums.YesNo.No;
            //         EnableGKC = UniversalAIEnums.YesNo.No;
            //         EnableNeofps = UniversalAIEnums.YesNo.No;
            //         EnableMmfps = UniversalAIEnums.YesNo.No;
            //         EnableHQFPS = UniversalAIEnums.YesNo.No;
            //         EnableInvector = UniversalAIEnums.YesNo.No;
            //         EnableHorrorFPS = UniversalAIEnums.YesNo.No;
            //     }
            // }
            
            if (!CurrentIntegration.Equals("Opsive"))
            {
                if (EnableOpsive == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "Opsive";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    // EnableUSK = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("GKC"))
            {
                if (EnableGKC == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "GKC";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    // EnableUSK = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("NEOFPS"))
            {
                if (EnableNeofps == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "NEOFPS";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableMmfps = UniversalAIEnums.YesNo.No;
                    //  EnableUSK = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }
            
            if (!CurrentIntegration.Equals("MMFPSE"))
            {
                if (EnableMmfps == UniversalAIEnums.YesNo.Yes)
                {
                    CurrentIntegration = "MMFPSE";
                    EnableSTP = UniversalAIEnums.YesNo.No;
                    EnableOpsive = UniversalAIEnums.YesNo.No;
                    EnableGKC = UniversalAIEnums.YesNo.No;
                    EnableNeofps = UniversalAIEnums.YesNo.No;
                    EnableHQFPS = UniversalAIEnums.YesNo.No;
                    //   EnableUSK = UniversalAIEnums.YesNo.No;
                    EnableInvector = UniversalAIEnums.YesNo.No;
                    EnableHorrorFPS = UniversalAIEnums.YesNo.No;
                }
            }

           

            if (!OldIntegration.Equals(CurrentIntegration))
            {
                AddRemoveDefine("UniversalAI_Integration_" + OldIntegration, false);
                
                AddRemoveDefine("UniversalAI_Integration_" + CurrentIntegration, true);

                OldIntegration = CurrentIntegration;
            }

        }
    }
}