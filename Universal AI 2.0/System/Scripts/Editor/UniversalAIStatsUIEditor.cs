using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;


namespace UniversalAI
{
   
    public class UniversalAIStatsUIEditor : EditorWindow
    {
        Texture Icon;
        

        public UniversalAIEnums.HealthBarDisplay HealthBarDisplay;

        public string AIName = "AI";

        public Color HealthBarColor = Color.red;
        
        public Color EnemyNameColor = Color.red;
        
        public Color FriendlyNameColor = Color.green;
     
        [MenuItem("Tools/Universal AI/AI Wizards/Stats UI Manager", false, 200)]
        public static void ShowWindow()
        {
            
            EditorWindow APS = EditorWindow.GetWindow(typeof(UniversalAIStatsUIEditor), false, "Stats UI Manager");
            APS.minSize = new Vector2(430f, 465f);
            APS.maxSize = new Vector2(430f, 465f);
        }

        void OnEnable()
        {
            if(Application.isPlaying)
                return;
            if (Icon == null) Icon = Resources.Load("LogoBrain") as Texture;
        
        }
        private UniversalAIHealthBar OldSelectedobject;
        private UniversalAIHealthBar Selectedobject;
       

        private void InitializeValues(UniversalAIHealthBar healthBar)
        {
            if(Application.isPlaying)
                return;
            HealthBarDisplay = healthBar.HealthBarDisplay;
            AIName = healthBar.AIName;
            HealthBarColor = healthBar.HealthBarColor;
            EnemyNameColor = healthBar.EnemyNameColor;
            FriendlyNameColor = healthBar.FriendlyNameColor;
        }
        
        void OnGUI()
        {

            if(Application.isPlaying)
                return;
            
           
            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box");
            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};
            EditorGUILayout.LabelField(new GUIContent(Icon), style, GUILayout.ExpandWidth(true),
                GUILayout.Height(32));
            EditorGUILayout.LabelField("Universal AI Stats UI Editor", style, GUILayout.ExpandWidth(true));
            
            if (Selection.gameObjects.Length != 1)
            {
                Selectedobject = null;
               
                OldSelectedobject = null;
                
                HealthBarDisplay = UniversalAIEnums.HealthBarDisplay.HealthAndName;

                AIName = "AI";

                HealthBarColor = Color.red;
        
                EnemyNameColor = Color.red;
        
                FriendlyNameColor = Color.green;
                
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("Please Select ONE 'Universal AI Stats UI' GameObject To Edit Stats UI First Before Starting!", MessageType.Error, true);
                GUI.backgroundColor = Color.white;
            
                EditorGUI.BeginDisabledGroup(true);
            }
            else
            {
                Selectedobject = Selection.gameObjects[0].GetComponent<UniversalAIHealthBar>();
               

                if (Selectedobject != null)
                {
                    if (OldSelectedobject == null)
                    {
                        OldSelectedobject = Selectedobject;
                        InitializeValues(Selectedobject);
                    }
                    else if(OldSelectedobject != Selectedobject)
                    {
                        OldSelectedobject = Selectedobject;
                        InitializeValues(Selectedobject);
                    }
                }
            }
            

            if (Selectedobject == null)
            {
                OldSelectedobject = null;
                
                HealthBarDisplay = UniversalAIEnums.HealthBarDisplay.HealthAndName;

                AIName = "AI";

                HealthBarColor = Color.red;
        
                EnemyNameColor = Color.red;
        
                FriendlyNameColor = Color.green;
                if (Selection.gameObjects.Length == 1)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("Please Select ONE 'Universal AI Stats UI' GameObject To Edit Stats UI First Before Starting!", MessageType.Error, true);
                    GUI.backgroundColor = Color.white;
            
                    EditorGUI.BeginDisabledGroup(true);
                }
               
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "The Universal AI Stats UI Editor allows you to adjust or create the AI Stats UI really easily with the given settings!",
                    MessageType.Info, true);
            }
            
          

            GUILayout.Space(4);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("UI Settings", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);


            HealthBarDisplay = (UniversalAIEnums.HealthBarDisplay)  EditorGUILayout.EnumPopup("Stats UI Display Type", HealthBarDisplay);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("How will the 'Stats UI' Display ?", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            
            GUILayout.Space(10);

            if (HealthBarDisplay != UniversalAIEnums.HealthBarDisplay.OnlyName)
            {
                HealthBarColor = EditorGUILayout.ColorField("Stats UI Health Bar Color", HealthBarColor);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("What Should Be The 'Health Bar' Color ?", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                
                GUILayout.Space(10);
                
            }

            if (HealthBarDisplay != UniversalAIEnums.HealthBarDisplay.OnlyHealth)
            {
                AIName = EditorGUILayout.TextField("Stats UI AI Name", AIName);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("How will the 'Stats UI' Display The AI's Name ?", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                
                GUILayout.Space(10);
                
                EnemyNameColor = EditorGUILayout.ColorField("Stats UI Enemy Color", EnemyNameColor);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("What Should Be The Enemy's 'Name UI' Color ?", MessageType.None, true);
                GUI.backgroundColor = Color.white;

                GUILayout.Space(10);
                
                FriendlyNameColor = EditorGUILayout.ColorField("Stats UI Friendly Color", FriendlyNameColor);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("What Should Be The Friendly's 'Name UI' Color ?", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                
                GUILayout.Space(10);
            }
            
            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
            
                if (GUILayout.Button("APPLY", HelpButtonStyle, GUILayout.Height(23)))
                {
                    if (EditorUtility.DisplayDialog("Stats UI Manager",
                        "Are you sure?", "Yes", "Cancel"))
                    {
                        if (Selectedobject == null)
                        {
                            Debug.LogError("Please Select An 'Stats UI' GameObject And Try Again!");
                            return;
                        }
                        
                      
                        Selectedobject.HealthBarDisplay = HealthBarDisplay;
                        Selectedobject.AIName = AIName;
                        Debug.Log("test");
                        Selectedobject.HealthBarColor = HealthBarColor;
                        Selectedobject.EnemyNameColor = EnemyNameColor;
                        Selectedobject.FriendlyNameColor = FriendlyNameColor;

                        Selectedobject = null;
                        OldSelectedobject = null;
                    }
                }


            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndVertical();
           
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            GUILayout.Space(30);
            
           
        }
        
    }
}