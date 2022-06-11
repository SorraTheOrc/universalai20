using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;


namespace UniversalAI
{
   
    public class UniversalAISetupEditor : EditorWindow
    {
        Texture Icon;
        Vector2 scroll;
        public GameObject AIObject;
        public GameObject PlayerObject;
        Editor gameObjectEditor;

        static float secondspassed = 0;
        static double startingseconds = 0;
        static double progress = 0;

        private bool ConfirmationMessaage = false;


        public enum MovementTypeAI
        {
            Rootmotion = 0,
            NavmeshSpeed = 1
        }

        public MovementTypeAI MovementTypeAi = MovementTypeAI.Rootmotion;


        public UniversalAIEnums.HealthBarDisplay healthBarDisplay = UniversalAIEnums.HealthBarDisplay.HealthAndName;

        public UniversalAIEnums.AIConfidence AIConfidence = UniversalAIEnums.AIConfidence.Brave;

        public UniversalAIEnums.AIType AIType = UniversalAIEnums.AIType.Enemy;

        public string PlayerTag = "Player";
        
        public string AITag = "Untagged";

        public UniversalAIEnums.YesNo DrawObjectPreview = UniversalAIEnums.YesNo.No;
        
        public UniversalAIEnums.WanderType AIWanderType = UniversalAIEnums.WanderType.Dynamic;

        public UniversalAIEnums.DeathMethod AIDeathType = UniversalAIEnums.DeathMethod.Animation;

        void OnInspectorUpdate()
        {
            Repaint();
        }


        [MenuItem("Tools/Universal AI/AI Wizards/Setup Manager", false, 200)]
        public static void ShowWindow()
        {
            EditorWindow APS = EditorWindow.GetWindow(typeof(UniversalAISetupEditor), false, "Setup Wizard");
            APS.minSize = new Vector2(420f, 605f);
          
        }

        void OnEnable()
        {
            if (Icon == null) Icon = Resources.Load("LogoBrain") as Texture;

            if (Selection.gameObjects.Length == 1)
            {
                if (Selection.gameObjects[0] != null)
                {
                    AIObject = Selection.gameObjects[0];
                }
            }

            ConfirmationMessaage = false;
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
            EditorGUILayout.LabelField("Universal AI Setup Wizard", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox(
                "The Universal AI Setup Wizard allows you to setup any AI really easily with the given settings!",
                MessageType.Info, true);

            GUILayout.Space(4);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(13);
            scroll = EditorGUILayout.BeginScrollView(scroll);

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);

            if (AIObject == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("Please Assign The AI Object!", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            else if (AIObject.GetComponent<Animator>() == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("AI Object Is Missing Animator Component!",
                    EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            else if (!AIObject.activeInHierarchy)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("Please Assign The AI Object from scene and enable it!",
                    EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            
            AIObject = (GameObject) EditorGUILayout.ObjectField("AI Object", AIObject, typeof(GameObject), true);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI scene gameObject that will be used for the setup!", MessageType.None, true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            if (PlayerTag == String.Empty)
            {
                Color GreenColor = Color.green;
                GreenColor.a = 0.25f;
                GUI.backgroundColor = GreenColor;
                EditorGUILayout.LabelField("Please Assign The Player Object In Order To Detect & Damage Player!", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }

            PlayerTag =  EditorGUILayout.TagField("Player Tag", PlayerTag);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Player Tag  that will be used for the player setup!", MessageType.None, true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);
            
            AITag =  EditorGUILayout.TagField("AI Tag", AITag);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI Tag that will be used for the setup!", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            
            GUILayout.Space(10);
            
            MovementTypeAi = (MovementTypeAI) EditorGUILayout.EnumPopup("AI Movement Type", MovementTypeAi);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI's Movement Type that will be used for the AI movement!", MessageType.None,
                true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            AIConfidence =
                (UniversalAIEnums.AIConfidence) EditorGUILayout.EnumPopup("AI Confidence Type", AIConfidence);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI's Confidence Type that will be used for the general AI confidence!",
                MessageType.None, true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            AIType = (UniversalAIEnums.AIType) EditorGUILayout.EnumPopup("AI Type", AIType);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI's Type that will be used for the general AI behaviour!", MessageType.None,
                true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            AIWanderType = (UniversalAIEnums.WanderType) EditorGUILayout.EnumPopup("AI Wander Type", AIWanderType);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI's Wandering Type that will be used for the AI Patrol Behaviour!",
                MessageType.None, true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            AIDeathType = (UniversalAIEnums.DeathMethod) EditorGUILayout.EnumPopup("AI Death Type", AIDeathType);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The AI's Death Type that will be used for the AI Death Method!", MessageType.None,
                true);
            GUI.backgroundColor = Color.white;
            
            GUILayout.Space(10);

            if (DrawObjectPreview == UniversalAIEnums.YesNo.Yes)
            {
                if (AIObject == null)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("Please Assign The AI Object first!", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }
                else if (!AIObject.activeInHierarchy)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("Please Assign The AI Object from scene and enable it first!",
                        EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }
            }

            DrawObjectPreview =
                (UniversalAIEnums.YesNo) EditorGUILayout.EnumPopup("Draw Object Preview", DrawObjectPreview);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("Draws a object preview below!", MessageType.None, true);
            GUI.backgroundColor = Color.white;

            if (AIObject != null && AIObject.activeInHierarchy && DrawObjectPreview == UniversalAIEnums.YesNo.Yes)
            {
                GUILayout.Space(15);
                GUIStyle bgColor = new GUIStyle();
                bgColor.normal.background = Texture2D.grayTexture;

                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(AIObject);
                else if(gameObjectEditor.target != AIObject)
                {
                    gameObjectEditor = Editor.CreateEditor(AIObject);
                }

                gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(220, 220), bgColor);

                GUILayout.Space(15);
            }

            GUILayout.Space(8);

            if (AIObject == null || AIObject != null && !AIObject.activeInHierarchy)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField(
                    "You must have an scene gameObject applied to the AI Object slot before setting up the AI!",
                    EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            if (AIObject != null && AIObject.GetComponent<Animator>() == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField(
                    "You must have an animator component applied to the AI Object before setting up the AI!",
                    EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }

            EditorGUI.BeginDisabledGroup(AIObject == null || (AIObject != null && !AIObject.activeInHierarchy) || (AIObject != null && AIObject.GetComponent<Animator>() == null));

            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
            if (GUILayout.Button("SETUP AI", HelpButtonStyle, GUILayout.Height(23)))
            {
                if (EditorUtility.DisplayDialog("Universal AI Setup Wizard",
                    "Are you sure you want to Setup The AI?", "Setup", "Cancel"))
                {
                    PrefabAssetType Prefab = PrefabUtility.GetPrefabAssetType(AIObject);

                    if (Prefab != PrefabAssetType.NotAPrefab)
                    {
                        PrefabUtility.UnpackPrefabInstance(AIObject, PrefabUnpackMode.Completely,
                            InteractionMode.AutomatedAction);
                    }

                    SetupAI();
                    startingseconds = EditorApplication.timeSinceStartup;
                }
            }

            GUI.backgroundColor = Color.white;

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            GUILayout.Space(20);
            EditorGUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            GUILayout.Space(30);

            if (secondspassed > 0)
            {
                progress = EditorApplication.timeSinceStartup - startingseconds;

                if (progress < secondspassed)
                {
                    EditorUtility.DisplayProgressBar("Universal AI Setup Wizard", "Setting up the AI...",
                        (float) (progress / secondspassed));
                }
                else
                {
                    EditorUtility.ClearProgressBar();

                    if (ConfirmationMessaage && PlayerPrefs.GetInt("Dont Show Again AI") != 1)
                    {
                        if (EditorUtility.DisplayDialog("Universal AI Setup Wizard",
                            "Your AI has been created. You now need to apply animations to the AI, assign the AI's head transform in the Inspector and adjust the size and position of the created capsule collider.", "Okay",
                            "Okay, Don't Show Again"))
                        {
                            ConfirmationMessaage = false;
                        }
                        else
                        {
                            ConfirmationMessaage = false;
                            PlayerPrefs.SetInt("Dont Show Again AI", 1);
                        }
                    }
                }
            }

        }

       
        private void SetupAI()
        {

            if (!PlayerTag.Equals(String.Empty))
            {
                PlayerObject = GameObject.FindGameObjectWithTag(PlayerTag);
                
                if (PlayerObject != null)
                {
                    if (PlayerObject.GetComponent<UniversalAIPlayerReference>() == null)
                    {
                        PlayerObject.AddComponent<UniversalAIPlayerReference>();
                    }
                }
            }
            
            
            if (AIObject != null && AIObject.GetComponent<UniversalAISystem>() == null &&
                AIObject.GetComponent<Animator>() != null)
            {
                secondspassed = 1.5f;

                if (!AITag.Equals(String.Empty))
                {
                    if(!Resources.Load<UniversalAITags>("Scriptables/Tag Storage").AvailableAITags.Contains(AITag))
                        Resources.Load<UniversalAITags>("Scriptables/Tag Storage").AvailableAITags.Add(AITag);
                    
                    Resources.Load<UniversalAITags>("Scriptables/Tag Storage").ForceSerialization();
                }

                AIObject.AddComponent<NavMeshAgent>();
                AIObject.AddComponent<CapsuleCollider>();

                UniversalAISystem universalAISystem = AIObject.AddComponent<UniversalAISystem>();


                if (MovementTypeAi == MovementTypeAI.Rootmotion)
                {
                    universalAISystem.Settings.Movement.UseRootMotion = UniversalAIEnums.YesNo.Yes;
                }
                else
                {
                    universalAISystem.Settings.Movement.UseRootMotion = UniversalAIEnums.YesNo.No;
                }

                universalAISystem.General.AIConfidence = AIConfidence;

                universalAISystem.General.AIType = AIType;

                universalAISystem.General.wanderType = AIWanderType;

                universalAISystem.General.DeathType = AIDeathType;

                if (AIType != UniversalAIEnums.AIType.Enemy)
                {
                    universalAISystem.TypeSettings = new UniversalAISystem.typeSettings();
                    universalAISystem.TypeSettings.PlayerSettings = new UniversalAISystem.typeSettings.playerSettings();
                    universalAISystem.TypeSettings.PlayerSettings.PlayerTag = PlayerTag;
                }

                if (!PlayerTag.Equals(String.Empty))
                {
                    universalAISystem.PlayerTag = PlayerTag;
                }

                universalAISystem.tag = AITag;
                universalAISystem.GetComponent<NavMeshAgent>().angularSpeed = 2000;
                
                AIObject.AddComponent<UniversalAIAnimatorCreator>();
                AIObject.AddComponent<UniversalAIEvents>();
                AIObject.AddComponent<UniversalAISounds>();
                AIObject.AddComponent<UniversalAICommandManager>().universalAISystem = universalAISystem;

                if (AIObject.GetComponent<AudioSource>() == null)
                {
                    AIObject.AddComponent<AudioSource>().spatialBlend = 1;
                }
                else
                {
                    AIObject.GetComponent<AudioSource>().spatialBlend = 1;
                }

                AIObject.GetComponent<AudioSource>().dopplerLevel = 0;
                AIObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
                AIObject.GetComponent<AudioSource>().minDistance = 1;
                AIObject.GetComponent<AudioSource>().maxDistance = 50;

                AIObject.GetComponent<UniversalAISounds>().Initialize = true;

                ConfirmationMessaage = true;
            }
            else if (AIObject == null)
            {
                EditorUtility.DisplayDialog("Universal AI Setup Wizard - Failed!",
                    "AI Object Slot Is Null! Please assign one and try again.", "Okay");
                ConfirmationMessaage = false;
                return;
            }
            else if (AIObject.GetComponent<UniversalAISystem>() != null)
            {
                EditorUtility.DisplayDialog("Universal AI Setup Wizard - Failed!",
                    "There is already an Universal AI script on this object. Please choose another object without or delete it and try again.",
                    "Okay");
                ConfirmationMessaage = false;
                return;
            }
            else if (AIObject.GetComponent<Animator>() == null)
            {
                EditorUtility.DisplayDialog("Universal AI Setup Wizard - Failed!",
                    "There is no Animator component on your AI Object. Please add one and try again.", "Okay");
                ConfirmationMessaage = false;
                return;
            }
        }
    }
}