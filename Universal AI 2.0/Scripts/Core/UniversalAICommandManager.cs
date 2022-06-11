using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    public enum AttackState
    {
        Aggressive,
        Passive,
    }
    
    public enum CompanionBehaviour
    {
        Follow,
        Stay,
    }
    
    public enum PetBehaviour
    {
        Follow,
        Stay,
    }
public class UniversalAICommandManager : MonoBehaviour
{
    [Serializable]
    public class companionCommands
    {
        [HideInInspector] public UniversalAISystem universalAISystem;
        
        /// <summary>
        /// Changes The Companion AI's Attack State (Aggressive, Passive)
        /// </summary>
        public void ChangeAttackState(AttackState newAttackState)
        {
            universalAISystem.TypeSettings.CompanionSettings.AttackState = newAttackState;
        }
        
        /// <summary>
        /// Changes The Companion AI's Behaviour (Follow, Stay)
        /// </summary>
        public void ChangeCompanionBehaviour(CompanionBehaviour newBehaviour)
        {
            universalAISystem.TypeSettings.CompanionSettings.companionBehaviour = newBehaviour;
        }
        
    }
    
    [Serializable]
    public class petCommands
    {
        [HideInInspector] public UniversalAISystem universalAISystem;
        
        /// <summary>
        /// Changes The Pet AI's Behaviour (Follow, Stay)
        /// </summary>
        public void ChangePetBehaviour(PetBehaviour newBehaviour)
        {
            universalAISystem.TypeSettings.PetSettings.PetBehaviour = newBehaviour;
        }
        
    }
    
    [HideInInspector] public UniversalAISystem universalAISystem;
    [HideInInspector] public companionCommands CompanionCommands;
    [HideInInspector] public petCommands PetCommands;
    private void Awake()
    {
        if (universalAISystem == null)
        {
            universalAISystem = gameObject.GetComponent<UniversalAISystem>();
        }

        CompanionCommands = new companionCommands();
        CompanionCommands.universalAISystem = universalAISystem;
        
        PetCommands = new petCommands();
        PetCommands.universalAISystem = universalAISystem;

    }
    

    /// <summary>
    /// Adds A GameObject To The Ignored List For Detection
    /// </summary>
    public void AddIgnoredTarget(GameObject Target)
    {
        if (Target != null)
        {
            if(!universalAISystem.Detection.DetectionSettings.IgnoredObjects.Contains(Target))
                universalAISystem.Detection.DetectionSettings.IgnoredObjects.Add(Target);
        }
    }
    
    /// <summary>
    /// Removes The GameObject From The Ignored List For Detection
    /// </summary>
    public void RemoveIgnoredTarget(GameObject Target)
    {
        if (Target != null)
        {
            if(universalAISystem.Detection.DetectionSettings.IgnoredObjects.Contains(Target))
                universalAISystem.Detection.DetectionSettings.IgnoredObjects.Remove(Target);
        }
    }
    
    /// <summary>
    /// Removes Every GameObject From The Ignored List For Detection
    /// </summary>
    public void ClearAllIgnoredTargets()
    {
        universalAISystem.Detection.DetectionSettings.IgnoredObjects.Clear();
    }
    
    /// <summary>
    /// Returns The Current Target GameObject Of The AI / Will Return Null If There Is Currently No Target
    /// </summary>
    public GameObject GetTarget()
    {
        return universalAISystem.Target;
    }
    
    /// <summary>
    /// Sets The Temporary Target Of The AI
    /// </summary>
    public void SetTarget(GameObject newTarget)
    {
        universalAISystem.SearchSettedTarget = true;
        universalAISystem.Target = newTarget;
    }

    /// <summary>
    /// Sets The Temporary Destination For The AI (If The AI Isn't Wandering, AI Will Go To The Destination Whenever It Wanders Again)
    /// </summary>
    public void SetDestination(Vector3 newDestination)
    {
#if !UniversalAI_Integration_PathfindingPro
        universalAISystem.Nav.ResetPath();
#else
        universalAISystem.NavPro.SetPath(null);
#endif
        universalAISystem.OvverideWandering = true;
        universalAISystem.OvverideWanderingPos = newDestination;
    }
    
    /// <summary>
    /// Changes The AI's Current Detection Type
    /// </summary>
    public void ChangeDetectionType(UniversalAIEnums.DetectionType detectionType)
    {
        universalAISystem.Detection.DetectionSettings.DetectionType = detectionType;
    }
   
    /// <summary>
    /// Changes The AI's Current Faction
    /// </summary>
    public void ChangeFaction(UniversalAIEnums.Factions factions)
    {
        universalAISystem.Detection.Factions.Factions = factions;
    }
    
    /// <summary>
    /// Returns The AI's Current Detection Type
    /// </summary>
    public UniversalAIEnums.DetectionType GetDetectionType()
    {
        return universalAISystem.Detection.DetectionSettings.DetectionType;
    }

    /// <summary>
    /// Returns The AI's Current Faction
    /// </summary>
    public UniversalAIEnums.Factions GetFaction()
    {
        return universalAISystem.Detection.Factions.Factions;
    }
    
    /// <summary>
    /// Kills The AI With A Delay Or Instantly
    /// </summary>
    public void KillAI(float delay = 0f)
    {
       Invoke("Kill", delay);
    }

    private void Kill()
    {
        universalAISystem.Die();
    }
    
    /// <summary>
    /// Freezes The AI Delayed Or Instantly
    /// </summary>
    public void FreezeAI(float delay = 0f)
    {
        Invoke("Freeze", delay);
    }

    private void Freeze()
    {
        universalAISystem.FreezeAIManuel(true);
    }

    /// <summary>
    /// Stops The AI Scripts, So User Can Add Custom Behaviours
    /// </summary>
    public void StopAIBehaviour()
    {
        universalAISystem.StopAIBehaviour = true;
        universalAISystem.AdjustNavmesh(true);
        universalAISystem.PlayAnimation(UniversalAIEnums.AnimationType.Idle);
    }
    
    /// <summary>
    /// Starts The AI Scripts Again
    /// </summary>
    public void StartAIBehaviour()
    {
        universalAISystem.StopAIBehaviour = false;
    }
    
    /// <summary>
    /// Unfreezes The AI Delayed Or Instantly
    /// </summary>
    public void UnFreezeAI(float delay = 0f)
    {
        Invoke("UnFreeze", delay);
    }

    private void UnFreeze()
    {
        universalAISystem.FreezeAIManuel(false);
    }

    /// <summary>
    /// Revives The AI
    /// </summary>
    public void ReviveAI()
    {
        universalAISystem.Revive();
    }
    
    /// <summary>
    /// Sets The AI Health
    /// </summary>
    public void SetAIHealth(float newHealth)
    {
        universalAISystem.Health = newHealth;
    }
    
    /// <summary>
    /// Refills The AI Health Instantly
    /// </summary>
    public void RefillAIHealth()
    {
        universalAISystem.Health = universalAISystem.Stats.MaxHealth;
    }
    
    /// <summary>
    /// Returns The Current AI Health
    /// </summary>
    public float GetHealth()
    {
       return universalAISystem.Health;
    }
    
    /// <summary>
    /// Changes The AI Type
    /// </summary>
    public void ChangeAIType(UniversalAIEnums.AIType AIType)
    {
        universalAISystem.General.AIType = AIType;
    }
    
    /// <summary>
    /// Changes The AI Confidence
    /// </summary>
    public void ChangeAIConfidence(UniversalAIEnums.AIConfidence AIConfidence)
    {
        universalAISystem.General.AIConfidence = AIConfidence;
    }
    
    /// <summary>
    /// Changes The AI's Wandering Type
    /// </summary>
    public void ChangeAIWanderType(UniversalAIEnums.WanderType wanderType)
    { 
        universalAISystem.General.wanderType = wanderType;
    }
    
    /// <summary>
    /// Returns The Current AI Type
    /// </summary>
    public UniversalAIEnums.AIType GetAIType()
    {
        return universalAISystem.General.AIType;
    }
    
    /// <summary>
    /// Returns The Current AI Confidence
    /// </summary>
    public UniversalAIEnums.AIConfidence GetAIConfidence()
    {
       return universalAISystem.General.AIConfidence;
    }
    
    /// <summary>
    /// Returns The Current AI Wandering Type
    /// </summary>
    public UniversalAIEnums.WanderType GetAIWanderType()
    {
        return universalAISystem.General.wanderType;
    }

    /// <summary>
    /// Returns The Current Distance Between AI And AI'S Current Target. (Returns -1 If The Target Is Null)
    /// </summary>
    public float GetTargetDistance()
    {
        if (GetTarget() == null)
        {
            return -1f;
        }
        else
        {
            return universalAISystem.TargetDistance;
        }
    }

    /// <summary>
    /// Enables The Health Regeneration Of The AI
    /// </summary>
    public void EnableHealthRegeneration()
    {
        universalAISystem.Stats.AIRegeneratesHealth = UniversalAIEnums.YesNo.Yes;
    }
    
    /// <summary>
    /// Disables The Health Regeneration Of The AI
    /// </summary>
    public void DisableHealthRegeneration()
    {
        universalAISystem.Stats.AIRegeneratesHealth = UniversalAIEnums.YesNo.No;
    }
}
#if UNITY_EDITOR
    
[CustomEditor(typeof(UniversalAICommandManager))]
public class UniversalAICommandManagerEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
    
}