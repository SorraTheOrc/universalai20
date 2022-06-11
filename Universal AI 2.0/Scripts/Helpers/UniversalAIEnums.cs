//Darking Assets

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

public class UniversalAIEnums : MonoBehaviour
{
    public enum AIType
    {
        Enemy,
        Companion,
        Pet,
    }
    
    public enum FireType
    {
        Projectile,
        Raycast,
        Simulated,
    }
    
    public enum AIRegenerate
    {
        Both,
        Combat,
        NonCombat,
    }
    
    public enum WeaponType
    {
        SingleShot,
        AdditiveShot,
    }

    public enum DeathMethod
    {
        Ragdoll, 
        Animation
    }
    [System.Flags]
    public enum SoundType
    {
        WalkSound = 1, 
        RunSound = 2,
        JumpSound = 3,
        ShootSound = 4,
        MeleeSound = 5,
        CrouchMovementSound = 6,
        OtherSound = 7,
    }
    
    public enum AIConfidence
    {
        Brave,
        Neutral,
        Coward,
    }
    public enum HealthBarDisplay
    {
        HealthAndName,
        OnlyHealth,
        OnlyName,
    }
    public enum AttackType
    {
        Melee,
        LongRange,
    }
    
    public enum WanderType
    {
        Dynamic,
        Waypoint,
        Stationary,
    }
    
    public enum WaypointType
    {
        InOrder,
        Random,
    }
    
    public enum YesNo
    {
        Yes,
        No,
    }
    
    public enum HandIKType
    {
        Both,
        OnlyLeftHand,
        OnlyRightHand,
    }
    
    public enum DetectionType
    {
        LineOfSight,
        Trigger,
    }
    
    public enum Factions
    {
        Player,
        Creature,
        Wildlife,
        NPC,
        Soldier,
        Fighter,
        GeneralAI,
    }
    
    public enum ApproachType
    {
        Enemy,
        Friendly,
    }
    
    public enum DecisionType
    {
        ClosestTarget,
        FirstTarget,
    }
    
    public enum AnimationType
    {
        Idle,
        Walk,
        Run,
        BackingUp,
        GetHit,
        Attack,
        Reload,
        Death,
        Block,
        TurnR,
        TurnL,
    }
    
    public enum AdjustNav
    {
        True,
        False,
        None,
    }
    
    public enum AICurrentState
    {
        Idle,
        Walk,
        Run,
        BackingUp,
        BackingUpPlayer,
        ChasePlayer,
        GotHit,
        Death,
        Attack,
        Frozen,
    }
}
    
}