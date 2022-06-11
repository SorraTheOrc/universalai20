#if UniversalAI_Integration_EasySave

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniversalAI
{


#if UNITY_EDITOR
using UnityEditor;
#endif

public class UniversalAIEasySave : MonoBehaviour
{
    #region Settings

    [Group("Open ")] public generalSaveSettings GeneralSettings;

    [Serializable]
    public class generalSaveSettings
    {
        [Header("GENERAL SETTINGS")]
        [Space]
        
        public string SaveFileName = "AISaves.es3";
        [Space] [Space] [Help("The Save File's Name !", HelpBoxMessageType.Info)]

        public string SaveKey = "SaveAI";
        [Space] [Space] [Help("The 'UNIQUE' Save Key, To Load Saves (Make This Unique) !", HelpBoxMessageType.Info)]

        public UniversalAIEnums.YesNo LoadOnStart = UniversalAIEnums.YesNo.Yes;
        [Space] [Space] [Help("Will It Automatically Load On Start ?", HelpBoxMessageType.Info)]
        
        public UniversalAIEnums.YesNo SaveOnQuit = UniversalAIEnums.YesNo.Yes;
        [Space] [Space] [Help("Will It Automatically Save On Quit ?", HelpBoxMessageType.Info)]
        
        public UniversalAIEnums.YesNo EncrpytSaveFile = UniversalAIEnums.YesNo.Yes;
        [Space] [Space] [Help("Will It Encrypt The Save File ?", HelpBoxMessageType.Info)]
        
        public UniversalAIEnums.YesNo DebugLoadEvents = UniversalAIEnums.YesNo.No;
        [Space] [Space] [Help("Will It Debug The Loaded Values Too ?", HelpBoxMessageType.Info)]
        
        [ReadOnly]
        public UniversalAISystem AISystem;
    }

    #endregion
    
    #region Variables To Save

    [Group("Open ")] public savedVariables VariablesToSave;

    [Serializable]
    public class savedVariables
    {
        [Header("VARIABLES")] 
        [Space] 
        
        public UniversalAIEnums.YesNo SaveHealth = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SavePosition = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveRotation = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveCurrentTarget = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveCurrentDestination = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveAlertedState = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveAIConfidence = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveFrozenState = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveIgnoredTargets = UniversalAIEnums.YesNo.Yes;
        public UniversalAIEnums.YesNo SaveNonIgnoredTargets = UniversalAIEnums.YesNo.Yes;
    }

    #endregion

    private void OnValidate()
    {
        if(Application.isPlaying)
            return;
        
        if (GeneralSettings == null)
        {
            GeneralSettings = new generalSaveSettings();
        }

        if (GeneralSettings.AISystem == null)
        {
            if (GetComponent<UniversalAISystem>() != null)
            {
                GeneralSettings.AISystem = GetComponent<UniversalAISystem>();
            }
        }
    }

    private ES3Settings _settings;
    public void Save()
    {
        if (GeneralSettings.EncrpytSaveFile == UniversalAIEnums.YesNo.Yes)
        {
            _settings = new ES3Settings(ES3.EncryptionType.AES, "SavedAIPassword");
        }
        else
        {
            _settings = new ES3Settings(ES3.EncryptionType.None, "SavedAIPassword");
        }
        
        if (VariablesToSave.SaveHealth == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Health: " + GeneralSettings.AISystem.Health);
            }
            ES3.Save("AIHealth " + GeneralSettings.SaveKey, GeneralSettings.AISystem.Health, GeneralSettings.SaveFileName, _settings);
        }


        if (VariablesToSave.SavePosition == UniversalAIEnums.YesNo.Yes ||
            VariablesToSave.SaveRotation == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                if(VariablesToSave.SavePosition == UniversalAIEnums.YesNo.Yes)
                    Debug.Log("Saved Transform Pos: " + this.transform.position);
                
                if(VariablesToSave.SaveRotation == UniversalAIEnums.YesNo.Yes)
                    Debug.Log("Saved Transform Rot: " + this.transform.rotation);
            }
             
            ES3.Save("AITransform " + GeneralSettings.SaveKey, this.transform, GeneralSettings.SaveFileName, _settings);
        }


        if (VariablesToSave.SaveCurrentTarget == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Target: " + GeneralSettings.AISystem.Target);
            }
            
            ES3.Save("AITarget " + GeneralSettings.SaveKey, GeneralSettings.AISystem.Target, GeneralSettings.SaveFileName, _settings);
        }


        if (VariablesToSave.SaveCurrentDestination == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Destination: " + GeneralSettings.AISystem.WanderDestination);
            }
            
            ES3.Save("AIDestination " + GeneralSettings.SaveKey, GeneralSettings.AISystem.WanderDestination, GeneralSettings.SaveFileName, _settings);
        }


        if (VariablesToSave.SaveAlertedState == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Alerted State: " + GeneralSettings.AISystem.Alerted);
            }
            
            ES3.Save("AIAlerted " + GeneralSettings.SaveKey, GeneralSettings.AISystem.Alerted, GeneralSettings.SaveFileName, _settings);
        }
        
        if (VariablesToSave.SaveAIConfidence == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved AI Confidence: " + GeneralSettings.AISystem.General.AIConfidence);
            }
            
            ES3.Save("AIConfidence " + GeneralSettings.SaveKey, GeneralSettings.AISystem.General.AIConfidence, GeneralSettings.SaveFileName, _settings);
        }
        
        if (VariablesToSave.SaveFrozenState == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Frozen State: " + GeneralSettings.AISystem.Frozen);
            }
            
            ES3.Save("AIFrozen " + GeneralSettings.SaveKey, GeneralSettings.AISystem.Frozen, GeneralSettings.SaveFileName, _settings);
        }
        
        if (VariablesToSave.SaveIgnoredTargets == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Ignored Targets Length: " + GeneralSettings.AISystem.Detection.DetectionSettings.IgnoredObjects.Count);
            }
            
            ES3.Save("AIIgnoredTargets " + GeneralSettings.SaveKey,  GeneralSettings.AISystem.Detection.DetectionSettings.IgnoredObjects, GeneralSettings.SaveFileName, _settings);
        }

        if (VariablesToSave.SaveNonIgnoredTargets == UniversalAIEnums.YesNo.Yes)
        {
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Saved Non Ignored Targets Length: " + GeneralSettings.AISystem.Detection.DetectionSettings.DetectObjects.Count);
            }
            
            ES3.Save("AINonIgnoredTargets " + GeneralSettings.SaveKey,  GeneralSettings.AISystem.Detection.DetectionSettings.DetectObjects, GeneralSettings.SaveFileName, _settings);
            
            ES3.Save("AIPlayerEnemy " + GeneralSettings.SaveKey,  GeneralSettings.AISystem.Detection.Factions.CanDetectPlayer, GeneralSettings.SaveFileName, _settings);
        }
    }

    private void Start()
    {
        //Dont Delete
    }

    public void Load()
    {

        if (GeneralSettings.EncrpytSaveFile == UniversalAIEnums.YesNo.Yes)
        {
            _settings = new ES3Settings(ES3.EncryptionType.AES, "SavedAIPassword");
        }
        else
        {
            _settings = new ES3Settings(ES3.EncryptionType.None, "SavedAIPassword");
        }
        
        if (ES3.KeyExists("AIHealth " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveHealth == UniversalAIEnums.YesNo.Yes)
        {
            float LoadedHealth = ES3.Load<float>("AIHealth " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Health: " + LoadedHealth);
            
            GeneralSettings.AISystem.UniversalAICommandManager.SetAIHealth(LoadedHealth);
        }
        
        if (ES3.KeyExists("AITransform " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && (VariablesToSave.SavePosition == UniversalAIEnums.YesNo.Yes || VariablesToSave.SaveRotation == UniversalAIEnums.YesNo.Yes))
        {
            Transform LoadedTransform = ES3.Load<Transform>("AITransform " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);


            if (VariablesToSave.SavePosition == UniversalAIEnums.YesNo.Yes)
            {
                transform.position.Set(LoadedTransform.position.x, LoadedTransform.position.y, LoadedTransform.position.z);
                
                if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                    Debug.Log("Transform Pos: " + LoadedTransform.position.x + " " + LoadedTransform.position.y + " " + LoadedTransform.position.z);
            }

            if (VariablesToSave.SaveRotation == UniversalAIEnums.YesNo.Yes)
            {
                transform.localEulerAngles.Set(LoadedTransform.rotation.x, LoadedTransform.position.y, LoadedTransform.position.z);
                
                if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                    Debug.Log("Transform Rot: " + LoadedTransform.rotation.x + " " + LoadedTransform.rotation.y + " " + LoadedTransform.rotation.z);
            }
        }
        
        if (ES3.KeyExists("AIConfidence " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveAIConfidence == UniversalAIEnums.YesNo.Yes)
        {
            UniversalAIEnums.AIConfidence LoadedAIConfidence = ES3.Load<UniversalAIEnums.AIConfidence>("AIConfidence " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Confidence: " + LoadedAIConfidence);

            GeneralSettings.AISystem.General.AIConfidence = LoadedAIConfidence;
        }
        
        if (ES3.KeyExists("AIFrozen " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveFrozenState == UniversalAIEnums.YesNo.Yes)
        {
            bool LoadedFrozen = ES3.Load<bool>("AIFrozen " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Frozen: " + LoadedFrozen);
            
            GeneralSettings.AISystem.FreezeAIManuel(LoadedFrozen);
        }
        
        if (ES3.KeyExists("AIIgnoredTargets " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveIgnoredTargets == UniversalAIEnums.YesNo.Yes)
        {
            List<GameObject> LoadedIgnoredTargets = ES3.Load<List<GameObject>>("AIIgnoredTargets " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Loaded Ignored Target Length: " + LoadedIgnoredTargets.Count);

            GeneralSettings.AISystem.Detection.DetectionSettings.IgnoredObjects = LoadedIgnoredTargets;
        }
        
        if (ES3.KeyExists("AINonIgnoredTargets " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveNonIgnoredTargets == UniversalAIEnums.YesNo.Yes)
        {
            List<GameObject> LoadedNonIgnoredTargets = ES3.Load<List<GameObject>>("AINonIgnoredTargets " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            UniversalAIEnums.YesNo PlayerEnemy = ES3.Load<UniversalAIEnums.YesNo>("AIPlayerEnemy " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Loaded Non Ignored Target Length: " + LoadedNonIgnoredTargets.Count);

            GeneralSettings.AISystem.Detection.DetectionSettings.DetectObjects = LoadedNonIgnoredTargets;
            GeneralSettings.AISystem.Detection.Factions.CanDetectPlayer = PlayerEnemy;
        }
        
        if (ES3.KeyExists("AIDestination " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveCurrentDestination == UniversalAIEnums.YesNo.Yes)
        {
            Vector3 LoadedDestination = ES3.Load<Vector3>("AIDestination " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Destination: " + LoadedDestination);
            
            GeneralSettings.AISystem.UniversalAICommandManager.SetDestination(LoadedDestination);
        }
        
        if (ES3.KeyExists("AIAlerted " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveAlertedState == UniversalAIEnums.YesNo.Yes)
        {
            bool LoadedAlerted = ES3.Load<bool>("AIAlerted " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Alerted: " + LoadedAlerted);
            
            GeneralSettings.AISystem.Alerted = LoadedAlerted;

            if (LoadedAlerted)
            {
                if(!GeneralSettings.AISystem.DontGoCombat)
                    GeneralSettings.AISystem.Anim.SetBool("Combating", true);
            }
        }

        
        if (ES3.KeyExists("AITarget " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName) && VariablesToSave.SaveCurrentTarget == UniversalAIEnums.YesNo.Yes)
        {
            GameObject LoadedTarget = ES3.Load<GameObject>("AITarget " + GeneralSettings.SaveKey, GeneralSettings.SaveFileName, _settings);
            
            if(LoadedTarget == null)
                return;
            
            if (GeneralSettings.DebugLoadEvents == UniversalAIEnums.YesNo.Yes)
                Debug.Log("Target: " + LoadedTarget);

            GeneralSettings.AISystem.UniversalAICommandManager.SetTarget(LoadedTarget);
        }

        if (GeneralSettings.AISystem.UniversalAICommandManager.GetHealth() <= 0)
        {
            GeneralSettings.AISystem.UniversalAICommandManager.KillAI();
        }
    }

    private void OnApplicationQuit()
    {
        if(GeneralSettings.SaveOnQuit == UniversalAIEnums.YesNo.Yes)
            Save();
    }
}

#if UNITY_EDITOR
    
[CustomEditor(typeof(UniversalAIEasySave))]
public class UniversalAIEasySaveEditor : Editor
{
    public UniversalAIEasySave Savesystem;

    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};

    private void OnEnable()
    {
        Savesystem = (UniversalAIEasySave)target;
    }
        
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
        style.fontSize = 13;
        EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
        EditorGUILayout.LabelField("Universal AI Easy Save", style, GUILayout.ExpandWidth(true));
            
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
            
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
        
        GUILayout.Space(20);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("DELETE SAVE FILE",GUILayout.Width(240),GUILayout.Height(23.5f)))
        {
            if (File.Exists(Application.persistentDataPath + "/" + Savesystem.GeneralSettings.SaveFileName))
            {
               File.Delete(Application.persistentDataPath + "/" + Savesystem.GeneralSettings.SaveFileName);
               Debug.Log("Deleted The Save File!");
            }
            else
            {
                Debug.LogError("File Cannot Be Found!");
            }
        } 
        
        if (GUILayout.Button("OPEN SAVE FILE",GUILayout.Width(240),GUILayout.Height(23.5f)))
        {
            if (File.Exists(Application.persistentDataPath + "/" + Savesystem.GeneralSettings.SaveFileName))
            {
                Application.OpenURL(Application.persistentDataPath);
            }
            else
            {
                Debug.LogError("File Cannot Be Found!");
            }
        } 
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}

#endif
    
}


#endif