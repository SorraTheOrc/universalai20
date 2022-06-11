//Darking Assets

using System.Linq;

#if UniversalAI_Integration_PathfindingPro
using Pathfinding;
#endif

#if UniversalAI_Integration_Puppetmaster
using RootMotion.Dynamics;
#endif

using UnityEngine.Events;

namespace UniversalAI
{
    
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using AnimationClipOverrideList = UniversalAIAnimatorCreator.AnimationClipOverrideList;
using AnimationAttackClipOverrideList = UniversalAIAnimatorCreator.AnimationAttackClipOverrideList;
#endif

using AnimationType = UniversalAIEnums.AnimationType;
using AdjustNav = UniversalAIEnums.AdjustNav;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif
   
    public class BoolEvent : UnityEvent<bool> {}
    public class BoolMeleeEvent : UnityEvent<bool, bool> {}
    
    public class UniversalAISystem : MonoBehaviour, UniversalAIDamageable
{
    #region Variables
    
#if UNITY_EDITOR
    #region Animation List Variables

    [HideInInspector] public AnimationClipOverrideList Idles;
    
    [HideInInspector] public AnimationClipOverrideList CombatIdles;
    
    [HideInInspector] public AnimationAttackClipOverrideList Attacks;
    
    [HideInInspector] public AnimationClipOverrideList HitReactions;
    
    [HideInInspector] public AnimationClipOverrideList Deaths;
    
    #endregion
#endif
    
    #region IK Variables

    [HideInInspector] private Transform[] boneTransforms;
    [HideInInspector] private int iterations = 10; 
    [HideInInspector] public Transform AimTransform;
    [HideInInspector] public bool UseIK = false;
    [HideInInspector] public float state = 0; 
    [HideInInspector] public float elapsedTime = 0;
    [HideInInspector] private float DistanceFixLerp;
    [HideInInspector] private Vector3 TheVel = Vector3.zero;
    [HideInInspector] private Vector3 FixedLookPosition;
    [HideInInspector] private Vector3 lastone;
    #endregion
    
    #region Main Variables

    [HideInInspector] public float Health;
    
    [HideInInspector] public GameObject Target = null;

#if !UniversalAI_Integration_PathfindingPro
    [HideInInspector] public Collider MainCollider;
#else
    [HideInInspector] public CharacterController MainCollider;
#endif
    
#if !UniversalAI_Integration_PathfindingPro
    [HideInInspector] public NavMeshAgent Nav;
#else
    [HideInInspector] public AIPath NavPro;
#endif

   
    
    [HideInInspector] public Animator Anim;

    [HideInInspector] public float TargetDistance;
    
    [HideInInspector] public List<GameObject> VisibleTargets = new List<GameObject>();

    #endregion

    #region Private Variables

    [HideInInspector] public BoolEvent TriggerHitboxAttacks = new BoolEvent();
    [HideInInspector] public BoolMeleeEvent WeaponStateEvent = new BoolMeleeEvent();
    [HideInInspector] public UnityEvent OnEquipped = new UnityEvent();
    [HideInInspector] public UnityEvent OnUnEquipped = new UnityEvent();
    
    
    [HideInInspector] public bool returntobrave;
    [HideInInspector] public float BodyWeight = 0;
    [HideInInspector] public int IdleCount;
    [HideInInspector] public int CombatIdleCount;
    [HideInInspector] public int AttackCount;
    [HideInInspector] public bool UseBlockSystem;
    [HideInInspector] public bool IsBlocking;
    [HideInInspector] public int HitCount;
    [HideInInspector] public int BlockCount;
    [HideInInspector] public int DeathCount;
    [HideInInspector] public List<Vector2> damages = new List<Vector2>();
    [HideInInspector] public Vector3 LastKnownPlayerPos;
    [HideInInspector] public float CurrentDamage;
    [HideInInspector] public bool Frozen;
    [HideInInspector] public bool StopAIBehaviour;
    [HideInInspector] public bool SearchSettedTarget;
    [HideInInspector] public bool OvverideWandering;
    [HideInInspector] public Vector3 OvverideWanderingPos;
    [HideInInspector] public bool IsRigidbody;
    [HideInInspector] public bool Regenerating;
    [HideInInspector] public bool Unfreeze;
    [HideInInspector] public bool InIdle;
    [HideInInspector] public int idlecount;
    [HideInInspector] public int CurrentWaypoint = -1;
    [HideInInspector] public int oldCurrentWaypoint = -1;
    [HideInInspector] public bool FirstDetect = true;
    [HideInInspector] public bool Waiting;
    [HideInInspector] public bool Alerted;
    [HideInInspector] public bool Searching;
    [HideInInspector] public bool DoubleIt;
    [HideInInspector] public bool Approaching;
    [HideInInspector] public bool FleeAway;
    [HideInInspector] public bool Attacking;
    [HideInInspector] public bool IsWeapon;
    [HideInInspector] public bool IsEquipping;
    [HideInInspector] public bool IsShooter;
    [HideInInspector] public bool CanAttack = true;
    [HideInInspector] public bool GotHit;
    [HideInInspector] public bool DamageSearching;
    [HideInInspector] public bool SoundSearching;
    [HideInInspector] public bool ReturnToNeutral;
    [HideInInspector] public bool Reloading;
    [HideInInspector] public bool CanFire;
    [HideInInspector] public bool DontGoCombat;
    [HideInInspector] public bool AlwaysEquipped;
    [HideInInspector] public string PlayerTag = "Player";
    [HideInInspector] public Vector3 WanderDestination;
    [HideInInspector] public UniversalAIEvents UniversalAIEvents;
    [HideInInspector] public UniversalAISounds UniversalAISounds;
    [HideInInspector] public UniversalAIHealthBar UniversalAIHealthBar;
    [HideInInspector] public UniversalAICommandManager UniversalAICommandManager;
    [HideInInspector] public UniversalAIEnums.AICurrentState OldState;
    [HideInInspector] public UniversalAIWaypoints.WaypointOverrideList Waypoints = new UniversalAIWaypoints.WaypointOverrideList();
    [HideInInspector] public UniversalAIWaypoints UniversalAIWaypoints;
    
    //Required for checking
    struct DistanceChecker
    {
        public float DistanceCalc;
        public GameObject TheTarget;
    }
    
    #endregion

    #region Public Variables
    [Space]
    
    #region Stats

    [Group("Open ")] public stats Stats;

    [Serializable]
    public class stats
    {
        [Header("STATS")]
        [Space(5)]
        
        [Tooltip("AI's Health Will Be Refilled On Start.")]
        public UniversalAIEnums.YesNo MaxHealthOnStart = UniversalAIEnums.YesNo.Yes;
        [Space(5)]
        
        [Tooltip("AI's Starting Health That Will Be Initialized On Start.")]
        public int StartHealth = 100;
        [Space(5)]
        
        [Tooltip("AI's Maximum Health That AI Can Have.")]
        public int MaxHealth = 100;
        [Space(5)]
        
        [Tooltip("Will AI's Health Regenerate On Possible?")]
        public UniversalAIEnums.YesNo AIRegeneratesHealth = UniversalAIEnums.YesNo.No;
        [Space(5)]
        
        [Tooltip("When Will AI's Regenerate Health?")]
        public UniversalAIEnums.AIRegenerate AIRegeneratesWhen = UniversalAIEnums.AIRegenerate.Both;
        [Space(5)]

        [Tooltip("The AI's Health Regenerate Rate In Seconds.")]
        public int RegenerateRate = 1;
        [Space(5)]
        
        [Tooltip("The Delay Before Regeneration Starts.")] 
        public float RegenerateStartDelay = 5f;
        [Space(5)]

        [Tooltip("Will AI's Health Regenerate If Possible?")] 
        public int StopRegenerateHealthAmount = 30;
      
    }

    #endregion
    
    #region General
    
   [Group("Open ")] public general General; 
    
    [Serializable]
    public class general
    {
        [Header("GENERAL SETTINGS")]
        [Space(5)]
            
            [Tooltip("Select the AI type that best suits the AI behaviour of this AI.")]
            public UniversalAIEnums.AIType AIType;
            [Space(5)]

            [Tooltip("Select the Death type that best suits the AI.")]
            public UniversalAIEnums.DeathMethod DeathType = UniversalAIEnums.DeathMethod.Animation;
            [Space(5)]

            [Tooltip("Confidence Of The AI's Behaviour, Ignore If The AI Is 'Pet Or Companion'.")]
            public UniversalAIEnums.AIConfidence AIConfidence;
            [Space(5)] 

            [Tooltip("Select the Wander type that best suits the AI.")]
            public UniversalAIEnums.WanderType wanderType;
            [Space(5)] 
            
            [Tooltip("Will Destroy The AI Object After Death With Delay.")]
            public UniversalAIEnums.YesNo DestroyAIOnDeath = UniversalAIEnums.YesNo.Yes;
            [Space(5)]
            
            [Tooltip("The Delay Between AI Death And Destruction.")]
            public float DestroyDelay = 2.5f;
            [Space(5)]
            
            [Tooltip("Select the Wander Radius that best suits the AI.")]
            public int DynamicWanderRadius = 20;
            [Space(5)] 
            
            [Tooltip("AI Will Continue Fleeing Until Reaching This Distance.")]
            public int FleeDistance = 30;
            [Space(5)]

            [Tooltip("The 'Minimum Time' AI Can Stay In The Idle State.")]
            public int MinIdleLength = 4;
            [Space(5)]

            [Tooltip("The 'Maximum Time' AI Can Stay In The Idle State.")]
            public int MaxIdleLength = 8;
    }

    #endregion

    #region Settings
    
    [Group("Open ")] public settings Settings; 

    [Serializable]
    public class settings
    {
        [Group("Open ")] public movement Movement;

        [Serializable]
        public class movement
        {
            [HideInInspector] public bool UseRootMotionC;
            [HideInInspector] public bool RunRoot;
            [HideInInspector] public bool TurnAnims;
            [Header("MOVEMENT")]
            [Space(5)]
            
            [Tooltip("Should The AI Chase Target On Visible?")]
            public UniversalAIEnums.YesNo ChaseTarget = UniversalAIEnums.YesNo.Yes;
            [Space(5)] 
            
            [Tooltip("Can The AI Run If Possible?")]
            public UniversalAIEnums.YesNo AICanRun = UniversalAIEnums.YesNo.Yes;
            [Space(5)] 
            
            [Tooltip("Will The AI Use Root Motion For Movement?")]
            public UniversalAIEnums.YesNo UseRootMotion = UniversalAIEnums.YesNo.No;
            [Space(5)]
            
            [Condition("UseRootMotionC",false,6f)] 
            [Tooltip("The Walking Speed Of The AI For Navmesh Agent.")]
            public float WalkSpeed = 1f;
            
            [Condition("UseRootMotionC",false,6f)] 
            [Tooltip("The Walking Backwards Speed Of The AI For Navmesh Agent.")]
            public float WalkBackwardsSpeed = 0.6f;
            
            [Condition("RunRoot",true,6f)] 
            [Tooltip("The Running Speed Of The AI For Navmesh Agent.")]
            public float RunSpeed = 1.7f; [Space] [Space]
            
            [Space(-5)]
            [Tooltip("The Distance That The AI Will Stop.")]
            public float StoppingDistance = 1f;
            [Space(5)]

            [Tooltip("The Distance That The AI Will Backup From The Enemy.")]
            public float TooCloseDistance = 0f;
            [Space(5)]
            
            [Tooltip("Will AI Use Turn System?")]
           [HideInInspector] public UniversalAIEnums.YesNo UseTurnSystem = UniversalAIEnums.YesNo.Yes;
            [Space(5)] 

            [Condition("TurnAnims", true, 0f)]
            [Tooltip("The Turn Limit Of The AI. AI Will Stop Turning After Reaching This Angle.")]
            [HideInInspector] public int TurnAngleLimit = 30;
            [Space(5)]
            
            [Condition("TurnAnims", true, 0f)]
            [Tooltip("The Turn Speed Of The AI. Find The Best Value For Your AI.")]
            [HideInInspector]  public int TurnSpeed = 50;
        }
        
        [Space]
        
        [Group("Open ")] public attack Attack;

        [Serializable]
        public class attack
        {
            [HideInInspector] public bool UseBlockSystem;
            [HideInInspector] public bool IsWeapon;
            
            [Header("ATTACK")]
            [Space(5)]

            [Tooltip("Can The AI Play Hit Reaction Animation While Attacking Too?")]
            public UniversalAIEnums.YesNo PlayHitAnimationsOnAttack = UniversalAIEnums.YesNo.Yes;
            [Space(5)]
            
           
            [Tooltip("The Attack Distance Of The AI For Start Attacking To The Target.")]
            public float AttackDistance = 1.5f;
            [Space(5)]

            // [Tooltip("The Distance That The Attack Will Be Successful.")]
            // public float MaxDamageDistance = 4f;
            // [Space(5)]

            [Tooltip("The Minimum Time To Wait Before Attacking.")]
            public float MinAttackDelay = 0.4f;
            [Space(5)]
            
            [Tooltip("The Maximum Time The AI Will Wait Before Attacking.")]
            public float MaxAttackDelay = 1f;
            [Space(1)]
            
            [Tooltip("The Hit Reaction Play Chance Of The AI.")]
            [Range(1,100)]
            public int HitReactionChance = 100;
            
            [Space(1)]
         
            [Condition("UseBlockSystem", true, 0f)] 
            [Tooltip("The Blocking Chance Of The AI.")]
            [Range(1,100)]
            public int BlockChance = 50;
            
            [Space(1)]
            
            [Condition("UseBlockSystem", true, 0f)] 
            [Tooltip("The Block Be Successful Chance Of The AI.")]
            [Range(1,100)]
            public int BlockSuccessfulChance = 70;
        }
    }
    
    #endregion
    
    #region TypeSettings
    
    [Group("Open ")] public typeSettings TypeSettings; 

    [Serializable]
    public class typeSettings
    {
        [Group("Open ")] public companionSettings CompanionSettings;

        [Serializable]
        public class companionSettings
        {
            [HideInInspector] public AttackState AttackState = AttackState.Aggressive;
            [HideInInspector] public CompanionBehaviour companionBehaviour = CompanionBehaviour.Follow;
            
            [Header("COMPANION AI SETTINGS")]
            [Space(5)]
            
            
            [Tooltip("The Companion AI's Stop Following Player Distance.")]
            public float FollowingStopDistance = 4f;
            [Space(5)]

            [Tooltip("Companion AI Will Start Running If The Player Is Too Far Away.")]
            public float FollowingStartRunningDistance = 9f;
            [Space(5)]
            
            [Tooltip("The Distance That The AI Will Start Backing Up From The Player!")]
            public float TooClosePlayerDistance = 2f;

        }

        [Space] 
        
        [Group("Open ")] public petSettings PetSettings;

        [Serializable]
        public class petSettings
        {
            [HideInInspector] public PetBehaviour PetBehaviour = PetBehaviour.Follow;
            
            [Header("PET AI SETTINGS")]
            [Space(5)]
            
            [Tooltip("The Pet AI's Stop Following Player Distance.")]
            public float FollowingStopDistance = 4f;
            [Space(5)]
            
            [Tooltip("Pet AI Will Start Running If The Player Is Too Far Away.")]
            public float FollowingStartRunningDistance = 9f;
        }
        
        [Space]
        
        [Group("Open ")] public playerSettings PlayerSettings;

        [Serializable]
        public class playerSettings
        {
            [Header("PLAYER SETTINGS")]
            [Space(5)]
        
            [Tooltip("The Player Tag To Find The Player. (Ignore If You Aren't Using Companion Or Pet AI)")]
            public string PlayerTag = "Player";
            [Space(5)] 
            
            [Tooltip("The Delay Until Your Player Spawns. (Ignore If You Aren't Using Companion Or Pet AI)")]
            public float FindPlayerDelay = 0.1f;

            // public int IgnorePlayerDamage = 999;
            // [Space] [Space] [Help("How Many Times Will The AI Ignore Player Damage Before Becoming Aggressive?", HelpBoxMessageType.Info)]
            
            [HideInInspector]
            public GameObject PlayerObject;
        }
    }
    
    #endregion
    
    #region Detection
    
    [Group("Open ")] public detection Detection; 

    [Serializable]
    public class detection
    {
        [Group("Open ")] public detectionsettings DetectionSettings;

        [Serializable]
        public class detectionsettings
        {
            [Header("DETECTION")]
            [Space(5)] 

            [Tooltip("The 'Detection Type' Of The AI For Detecting Possible Targets.")]
            public UniversalAIEnums.DetectionType DetectionType = UniversalAIEnums.DetectionType.LineOfSight;
            [Space(5)] 
            
            [Tooltip("The 'Detection Frequency' Of The AI, X seconds before updating detection.")]
            public float DetectionInterval = 0.3f;
            [Space(5)]  

            [Tooltip("The 'Detection Layers' That AI Can Detect.")]
            public LayerMask DetectionLayers = 1;
            [Space(5)] 
            
            [Tooltip("The 'Obstacle Layers' That AI Can't See Through.")]
            public LayerMask ObstacleLayers = 1;
            [Space(5)] 
            
            [Tooltip("The 'Detection Distance' Of The AI For The Detection.")]
            public float DetectionDistance = 10;
            [Space(5)] 
            
            [Tooltip("The 'Detection Angle' Of The AI, Ignore If 'Detection Type' is trigger.")]
            public float DetectionAngle = 70;
            [Space(5)]  
       
            [Tooltip("The 'Head Transform' Of The AI For Detection.")]
            public Transform HeadTransform = null;
            [Space(5)]  
            
            public bool AutoFindHeadTransform = false;
            
             
            [HideInInspector] public  List<GameObject> IgnoredObjects = new List<GameObject>();
            
            [HideInInspector] public  List<GameObject> DetectObjects = new List<GameObject>();
        }

        [Space]
        
        [Group("Open ")] public alertSettings AlertSettings;

        [Serializable]
        public class alertSettings
        {
            [Header("ALERT SETTINGS")]
            [Space(5)]  
            
            [Tooltip("The Minimum Time AI Will Stay Alerted After Loosing The Target.")]
            public UniversalAIEnums.YesNo DoubleTheDetectionDistance = UniversalAIEnums.YesNo.Yes;
            [Space(5)]
            
            [Tooltip("The Minimum Time AI Will Stay Alerted After Loosing The Target.")]
            public UniversalAIEnums.YesNo DoubleTheDetectionAngle = UniversalAIEnums.YesNo.Yes;
            [Space(5)]

            [Tooltip("The Minimum Time AI Will Stay Alerted After Loosing The Target.")]
            public float MinStayAlertLength = 15f;
            [Space(5)]   
            
            [Tooltip("The Maximum Time AI Will Stay Alerted After Loosing The Target.")]
            public float MaxStayAlertLength = 25f;
            // [Space(5)]  
            
            // [Header("BACKUP SETTINGS")]
            // [Space(5)]  
            //
            // [Tooltip("Can The AI Call For Backup On Alert?")]
            // public UniversalAIEnums.YesNo CallForBackup = UniversalAIEnums.YesNo.Yes;
            // [Space(5)] 
            //
            // [Tooltip("The Maximum Distance An AI Can Hear The Call.")]
            // public float CallForBackupRadius = 15f;
            // [Space(5)] 
            //
            // [Tooltip("The 'AI Layers' That AI Can Call For Backup.")]
            // public LayerMask CallForBackupLayers = 1;
            //
            // [HideInInspector] public UniversalAIEnums.YesNo CallingBackup = UniversalAIEnums.YesNo.No;
        }

        [Space]
        
        [Group("Open ")] public factions Factions;

        [Serializable]
        public class factions
        {
            [Header("FACTIONS")]
            [Space(5)] 
            
            [Tooltip("The 'Faction' Of The AI, For Target Decisions.")]
            public UniversalAIEnums.Factions Factions = UniversalAIEnums.Factions.Creature;
            [Space(5)] 
            
            public List<FactionsOverride> FactionGroups = new List<FactionsOverride>();
            [Space(20)] [Help("Create And Choose The Approach Types Between Factions.",HelpBoxMessageType.Info)]
            
            [Tooltip("Can The AI Detect Player On Visible?")]
            public UniversalAIEnums.YesNo CanDetectPlayer = UniversalAIEnums.YesNo.Yes;
        }
    }

    [Serializable]
    public class FactionsOverride
    {
        [Tooltip("The 'Faction Type' Of The Decision.")]
        public UniversalAIEnums.Factions Faction;

        [Tooltip("The 'Approach Type' Of The Decision. How Should The AI Approach To This Faction?")]
        public UniversalAIEnums.ApproachType ApproachType = UniversalAIEnums.ApproachType.Enemy;
    }
    
    #endregion
    
    #region Inverse Kinematics
     
     [Group("Open ")] public inverseKinematics InverseKinematics;

     [Serializable]
     public class inverseKinematics
     {
         [Header("INVERSE KINEMATICS")] 
         [Space(5)]

         [Tooltip("Will The AI Use The 'Inverse Kinematics' System?")]
         public UniversalAIEnums.YesNo UseInverseKinematics = UniversalAIEnums.YesNo.No;
         [Space(8)]
         
         [Header("HAND IK")]
         [Space(5)]

         [Tooltip("Will The AI Use The 'Hand IK' System?")]
         public UniversalAIEnums.YesNo UseHandIK = UniversalAIEnums.YesNo.No;
         [Space(5)] 
         
         [Tooltip("Which Hands Should The IK Apply To?")]
         public UniversalAIEnums.HandIKType HandIKType = UniversalAIEnums.HandIKType.OnlyLeftHand;
         [Space(5)] 
        
         [Tooltip("Hand IK Weight Amount.")]
         public float HandIKWeight = 1f;
         [Space(5)] 
         
         [Tooltip("Will AI Use Hand IK Smooth?")]
         public UniversalAIEnums.YesNo UseHandIKSmooth = UniversalAIEnums.YesNo.No;
         [Space(5)]
         
         [Tooltip("Hand IK Smooth Amount.")]
         public float HandIKSmooth = 0.3f;
         [Space(8)]

         [Header("LOOK AT IK")]
         [Space(5)]

         [Tooltip("Will The AI Use The 'Look At IK' System?")]
         public UniversalAIEnums.YesNo UseLookAtIK = UniversalAIEnums.YesNo.No;
         [Space(5)] 
         
         [Tooltip("LookAt IK Weight Amount.")]
         public float LookAtIKWeight = 1f;
         [Space(5)] 

         [Tooltip("The Target Layers AI Will Look At.")]
         public LayerMask LookAtLayers;
         [Space(5)]
         
         [Tooltip("LookAt IK Maximum Angle Limit.")]
         public float LookAtAngleLimit = 60f;
         [Space(5)]
         
         [Tooltip("LookAt IK Maximum Distance Limit.")]
         public float LookAtDistanceLimit = 7f;
         [Space(8)]
         
         [Header("AIM IK")]
         [Space(5)]

         [Tooltip("Will The AI Use The 'AIM IK' System?")]
         public UniversalAIEnums.YesNo UseAimIK = UniversalAIEnums.YesNo.No;
         [Space(5)]

         [Tooltip("Aim IK Weight Amount.")]
         public float AimIKWeight = 1f;
         [Space(5)] 
         
         [Tooltip("Hand IK Smooth Amount.")]
         public float AimIKSmooth = 0.20f;
         [Space(8)]
         
         [Tooltip("Which Bones Will Be Used For The AIM IK?")]
         public List<HumanBonesOverride> humanBones = new List<HumanBonesOverride>();
         [Space(5)]
    
         [Tooltip("Aim IK Offset.")]
         public Vector3 AimAtOffset = new Vector3(0, 0.65f, 0);
         [Space(5)] 
         
         [Tooltip("Aim IK Maximum Angle Limit.")]
         public float AngleLimit = 90f;

         [HideInInspector] public Transform LeftHandIK;
         [HideInInspector] public Transform RightHandIK;
     }

     [HideInInspector] public UniversalAITags TagStorage;
     [Serializable]
     public class HumanBonesOverride
     {
         public HumanBodyBones bone;
     }
     
     #endregion
     
     #region Debug
    
    [Group("Open ")] public debug Debug;

    [Serializable]
    public class debug
    {
        [BeginReadOnlyGroup]
        
        [Header("STATS")] 
        
        [Space]
        public float HealthDebug;
        
        [Space]
        [Header("COMPONENTS")] 
        
        [Space]
        public GameObject CurrentTargetDebug = null;

// #if !UniversalAI_Integration_PathfindingPro
//        [Space]
//         public Collider MainColliderDebug;
//         
// #else
//         [Space]
//         public CharacterController MainColliderDebug = null;
// #endif

#if !UniversalAI_Integration_PathfindingPro
        [Space]
        public NavMeshAgent NavmeshAgentDebug;  
        
#else
        [Space]
        public AIPath AIPathDebug;  
#endif
        


        [Space] 
        public Animator AnimatorDebug;
        [Space]
        
        [Header("CONDITIONS")]
        
        [Space]
        public UniversalAIEnums.AICurrentState AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;

        [Space]
        public UniversalAIEnums.YesNo AIAlertedDebug = UniversalAIEnums.YesNo.No;
        
        [Space]
        public UniversalAIEnums.YesNo AIAttacking = UniversalAIEnums.YesNo.No;
        
        [Space]
        public UniversalAIEnums.YesNo AITurning = UniversalAIEnums.YesNo.No;

        [Space] 
        public UniversalAIEnums.YesNo TargetVisibleDebug = UniversalAIEnums.YesNo.No;
        
        [Space] 
        public List<GameObject> VisibleTargetsDebug = new List<GameObject>();

        [Space] 
        
        [Header("OTHERS")] 
        
        [Space]
        public float TargetDistanceDebug = 0;
        
        // [Space]
        // public string AICurrentAnimation = "Idle";

        [Space] [Space] [EndReadOnlyGroup] 
        public UniversalAIEnums.YesNo UpdateDebugs = UniversalAIEnums.YesNo.Yes;


    }
    #endregion
    
    
    #endregion

    #endregion
    
    #region Start / Setup
    
    private void Awake()
    {
        if (FindObjectOfType<UniversalAIManager>() == null)
        {
            GameObject manager = new GameObject("AI Manager");
            manager.AddComponent<UniversalAIManager>();
            manager.hideFlags = HideFlags.HideInHierarchy;
        }
        SetupAI();
    }
    private void SetupAI()
    {
        //Initialize AI

        CanAttack = true;

#if UniversalAI_Integration_PathfindingPro
        General.DynamicWanderRadius /= 6;
#endif
        
        UniversalAIEvents.OnTargetVisible.AddListener(CheckIK);

        if (Settings.Movement.UseRootMotionC)
        {
            Anim.applyRootMotion = true;
        }

        if (General.AIType == UniversalAIEnums.AIType.Enemy)
        {
            // if (CheckComponents())
            // {
                SetupAIFinish();
            // }   
        }
        else
        {
            Invoke("FindPlayer", TypeSettings.PlayerSettings.FindPlayerDelay);
        }
    }

#if UniversalAI_Integration_Puppetmaster

    private PuppetMaster _puppetMaster;
    
#endif
    private void SetupAIFinish()
    {
        //Will be fixed in v2.0.7
        Settings.Movement.UseTurnSystem = UniversalAIEnums.YesNo.No;
        Settings.Movement.TurnAnims = false;
        //Initialize AI

        if (!Settings.Movement.UseRootMotionC)
        {
#if UniversalAI_Integration_PathfindingPro
            NavPro.maxSpeed = Settings.Movement.WalkSpeed;
#else
 Nav.speed = Settings.Movement.WalkSpeed;
#endif
        }

        Health = Stats.StartHealth;

        if (General.DeathType == UniversalAIEnums.DeathMethod.Ragdoll)
        {
            

#if UniversalAI_Integration_Puppetmaster

            _puppetMaster = transform.root.GetComponentInChildren<PuppetMaster>();

            if (_puppetMaster == null)
            {
                UnityEngine.Debug.LogError("[UniversalAI] Couldn't find the 'Puppet Master' script for the AI: " + name + " !!, disabling the AI!");
                gameObject.SetActive(false);
                this.enabled = false;
                return;
                
            }
            
            Collider[] colliders = _puppetMaster.GetComponentsInChildren<Collider>();
            Rigidbody[] rigidbodys = _puppetMaster.GetComponentsInChildren<Rigidbody>();
            
            if (rigidbodys.Length > 0)
            {
                IsRigidbody = true;
            }
            foreach (var rig in rigidbodys)
            {
                rig.isKinematic = true;

                if (rig.GetComponent<UniversalAIRagdollBone>() == null && rig.GetComponent<MeleeHitbox>() == null)
                {
                    rig.gameObject.AddComponent<UniversalAIRagdollBone>().system = this;
                }
            }
            
#else

            Collider[] colliders = GetComponentsInChildren<Collider>();
            Rigidbody[] rigidbodys = GetComponentsInChildren<Rigidbody>();

            if (MainCollider != null)
            {
                foreach (var col in colliders)
                {
                    if (col != MainCollider && col.GetComponent<MeleeHitbox>() == null)
                    {
                        Physics.IgnoreCollision(col, MainCollider);
                    }
                }   
            }

            if (rigidbodys.Length > 0)
            {
                IsRigidbody = true;
            }
            foreach (var rig in rigidbodys)
            {
                rig.isKinematic = true;

                if (rig.GetComponent<UniversalAIRagdollBone>() == null && rig.GetComponent<MeleeHitbox>() == null)
                {
                    rig.gameObject.AddComponent<UniversalAIRagdollBone>().system = this;
                }
            }
            
#endif
            
            
           
        }

        if (Settings.Movement.StoppingDistance > Settings.Attack.AttackDistance)
        {
            Settings.Movement.StoppingDistance = Settings.Attack.AttackDistance - 0.15f;
        }

#if !UniversalAI_Integration_PathfindingPro
Nav.stoppingDistance = Settings.Movement.StoppingDistance;
#else
        NavPro.endReachedDistance = Settings.Movement.StoppingDistance;
        NavPro.slowdownDistance = Settings.Movement.StoppingDistance + 0.75f;
#endif
        

        UniversalAIEvents.OnReady.Invoke();

        if (Anim.avatar.isHuman)
        {
            boneTransforms = new Transform[InverseKinematics.humanBones.Count];
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                if (InverseKinematics.UseLookAtIK == UniversalAIEnums.YesNo.Yes &&
                    InverseKinematics.humanBones[b].bone is HumanBodyBones.Head)
                {
                    continue;
                }
                boneTransforms[b] = Anim.GetBoneTransform(InverseKinematics.humanBones[b].bone);
            }   
        }

        gameObject.AddComponent<UniversalAIIntegrationsManager>();

        Anim.SetBool("AttackHit",Settings.Attack.PlayHitAnimationsOnAttack == UniversalAIEnums.YesNo.Yes);
        
#if UniversalAI_Integration_EasySave
        if (GetComponent<UniversalAIEasySave>() != null)
        {
            if (GetComponent<UniversalAIEasySave>().GeneralSettings.LoadOnStart == UniversalAIEnums.YesNo.Yes)
            {
                GetComponent<UniversalAIEasySave>().Load();
            }
        }

#endif

        // if (AttackCount <= 0 && General.AIConfidence == UniversalAIEnums.AIConfidence.Brave)
        // {
        //     General.AIConfidence = UniversalAIEnums.AIConfidence.Coward;
        // }

        // if (General.AIConfidence.Equals(UniversalAIEnums.AIConfidence.Neutral))
        // {
        //     Detection.Factions.FactionGroups.Clear();
        // }
        
        if(UniversalAIManager.AISystems != null)
            UniversalAIManager.AISystems.Add(this);
        
        AIReady = true;

        InvokeRepeating("Detect",0,Detection.DetectionSettings.DetectionInterval);
    }

    [HideInInspector] public UniversalAIPlayerReference CurrentPlayerRef = null;
    [HideInInspector] public UniversalAISystem CurrentAITarget = null;

    [HideInInspector] public bool AIReady;
    
    private void FindPlayer()
    {
        GameObject Player = GameObject.FindGameObjectWithTag(TypeSettings.PlayerSettings.PlayerTag);
    
        if (Player == null)
        {
            UnityEngine.Debug.Log("The AI: ' " + name + " ' couldn't find the player with the tag ' " + TypeSettings.PlayerSettings.PlayerTag + " '. Disabling the AI!");
            gameObject.SetActive(false);
            return;
        }
        
        if (Player.GetComponent<UniversalAIPlayerReference>() == null)
        {
            Player.AddComponent<UniversalAIPlayerReference>();
            UnityEngine.Debug.Log("The AI: ' " + name + " ' could found the player with the tag ' " + TypeSettings.PlayerSettings.PlayerTag + " ' but it is missing the script 'Universal AI Player Reference' Adding it !");
        }

        TypeSettings.PlayerSettings.PlayerObject = Player;
        
        Player.GetComponent<UniversalAIPlayerReference>().OnTakeDamageCheck.AddListener(PlayerDamaged);
        
        //if (CheckComponents())
        //{
            SetupAIFinish();
            //}
    }

    [HideInInspector] public bool StartWithError;
    [HideInInspector] public string Reasons;
    
    
    #endregion

    #region UNITY EDITOR

#if UNITY_EDITOR
    
    public void OnValidate()
    {
       
        if(Application.isPlaying)
            return;

        
        if (General == null)
        {
            General = new general();
        }

        if (General== null)
        {
            General = new general();
        }
        
        if (Detection == null)
        {
            Detection = new detection();
        }

        if (Detection.DetectionSettings == null)
        {
            Detection.DetectionSettings = new detection.detectionsettings();
        }
        
        if (Detection.Factions == null)
        {
            Detection.Factions = new detection.factions();
        }
        
        if (Settings == null)
        {
            Settings = new settings();
        }
        
        if (Settings.Movement == null)
        {
            Settings.Movement = new settings.movement();
        }
        
        if (Settings.Attack == null)
        {
            Settings.Attack = new settings.attack();
        }

        if (Debug == null)
        {
            Debug = new debug();
        }

        if (Stats == null)
        {
            Stats = new stats();
        }
        
        if (InverseKinematics == null)
        {
            InverseKinematics = new inverseKinematics();
        }

        if (Detection.Factions.FactionGroups.Count <= 0 /* && !General.AIConfidence.Equals(UniversalAIEnums.AIConfidence.Neutral) */)
        {
            Detection.Factions.FactionGroups.Add(new FactionsOverride
            {
                Faction = UniversalAIEnums.Factions.Player,
                ApproachType = UniversalAIEnums.ApproachType.Enemy
            });
        }

        if (TagStorage == null)
        {
            TagStorage = Resources.Load<UniversalAITags>("Scriptables/Tag Storage");
        }

#if NEOFPS && UniversalAI_Integration_NEOFPS

        if (GetComponent<AINeoSoundDetection>() == null)
        {
            gameObject.AddComponent<AINeoSoundDetection>();
        }
        
#endif
        Settings.Attack.UseBlockSystem = UseBlockSystem;
        
        Settings.Attack.IsWeapon = IsWeapon;
        
        if (Detection.DetectionSettings.AutoFindHeadTransform)
        {
            Detection.DetectionSettings.AutoFindHeadTransform = false;
            // if (Detection.DetectionSettings.HeadTransform == null)
            //{
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name.Contains("head") || t.name.Contains("Head") || t.name.Contains("HEAD"))
                    {
                        if (t.GetComponent<MeshRenderer>() == null && t.GetComponent<SkinnedMeshRenderer>() == null)
                        {
                            Detection.DetectionSettings.HeadTransform = t;
                        }
                    }
                }

                if (Detection.DetectionSettings.HeadTransform == null)
                {
                    UnityEngine.Debug.LogError("[Universal AI] Head Transform Couldn't Be Automatically Found!");
                }
                else
                {
                    UnityEngine.Debug.Log("[Universal AI] Head Transform Was Found! <b> {" + Detection.DetectionSettings.HeadTransform.name + "} </b>");
                }
            //}
        }

        if (UniversalAIEvents == null)
        {
            if (GetComponent<UniversalAIEvents>() != null)
            {
                UniversalAIEvents = GetComponent<UniversalAIEvents>();
            }
        }
        
        if (UniversalAISounds == null)
        {
            if (GetComponent<UniversalAISounds>() != null)
            {
                UniversalAISounds = GetComponent<UniversalAISounds>();
            }
        }

        if (General.AIType == UniversalAIEnums.AIType.Companion)
        {
            Detection.Factions.CanDetectPlayer = UniversalAIEnums.YesNo.No;
        }
        
        if (General.wanderType == UniversalAIEnums.WanderType.Waypoint)
        {
            if (GetComponent<UniversalAIWaypoints>() == null)
            {
                gameObject.AddComponent<UniversalAIWaypoints>();
            }
        }
        
#if !UniversalAI_Integration_PathfindingPro
        Nav = GetComponent<NavMeshAgent>();
        MainCollider = GetComponent<Collider>();
#endif
        
#if UniversalAI_Integration_PathfindingPro
        NavPro = GetComponent<AIPath>();
        MainCollider = GetComponent<CharacterController>();
        General.DynamicWanderRadius /= 6;
#endif
        Anim = GetComponent<Animator>();

        if (General.wanderType == UniversalAIEnums.WanderType.Waypoint && GetComponent<UniversalAIWaypoints>() != null)
        {
            UniversalAIWaypoints = GetComponent<UniversalAIWaypoints>();
            Waypoints = UniversalAIWaypoints.Waypoints;
        }

        Settings.Movement.UseRootMotionC = Settings.Movement.UseRootMotion == UniversalAIEnums.YesNo.Yes;

        Settings.Movement.RunRoot =
            !Settings.Movement.UseRootMotionC && Settings.Movement.AICanRun == UniversalAIEnums.YesNo.Yes;
        
       
        
#if UniversalAI_Integration_PathfindingPro

            // if (GetComponent<CharacterController>() != null)
            // {
            //    Debug.MainColliderDebug = GetComponent<CharacterController>();   
            // }
        
            if (GetComponent<AIPath>() != null)
            {
                Debug.AIPathDebug = GetComponent<AIPath>();   
            }
#else
  
        if (GetComponent<NavMeshAgent>() != null)
        {
            Debug.NavmeshAgentDebug = GetComponent<NavMeshAgent>();   
        }
            
        
        // if (GetComponent<Collider>() != null)
        // {
        //     Debug.MainColliderDebug = GetComponent<Collider>();   
        // }
        
#endif
       
        
        if (GetComponent<Animator>() != null)
        {
            Debug.AnimatorDebug = GetComponent<Animator>();   
        }

        if (Stats.StartHealth > Stats.MaxHealth || Stats.MaxHealthOnStart == UniversalAIEnums.YesNo.Yes)
        {
            Stats.StartHealth = Stats.MaxHealth;
        }

        Debug.HealthDebug = Stats.StartHealth;

        if (UniversalAICommandManager == null)
        {
            if (GetComponent<UniversalAICommandManager>() != null)
            {
                UniversalAICommandManager = GetComponent<UniversalAICommandManager>();   
            }
        }

        if(UniversalAIHealthBar == null)
        {
            if(GetComponentInChildren<UniversalAIHealthBar>() != null)
            {
                UniversalAIHealthBar = GetComponentInChildren<UniversalAIHealthBar>();
            }
        }

        if (InverseKinematics.humanBones.Count <= 0)
        {
            InverseKinematics.humanBones.Add(new HumanBonesOverride
            {
                bone = HumanBodyBones.UpperChest
            });
        }

        Settings.Movement.TurnAnims = Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes;
    }

    private void Start()
    {
        if (IsShooter && General.AIConfidence == UniversalAIEnums.AIConfidence.Coward)
        {
            DontGoCombat = true;
        }
        
        if (General.AIType == UniversalAIEnums.AIType.Companion)
        {
            Detection.Factions.CanDetectPlayer = UniversalAIEnums.YesNo.No;
        }
        // UniversalAIManager.AISystems.Add(this);
    }

    void OnDrawGizmosSelected()
    {
        //fov
        if (General.AIType == UniversalAIEnums.AIType.Enemy)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, 
                Detection.DetectionSettings.DetectionDistance);
            if (Detection.DetectionSettings.DetectionType == UniversalAIEnums.DetectionType.LineOfSight)
            {
                float totalFOV = Detection.DetectionSettings.DetectionAngle;
                float rayRange = Detection.DetectionSettings.DetectionDistance;
                float halfFOV = totalFOV / 2.0f;
                Gizmos.color = Color.red;
                Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
                Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
                Vector3 leftRayDirection = leftRayRotation * transform.forward;
                Vector3 rightRayDirection = rightRayRotation * transform.forward;
                Gizmos.DrawRay( transform.position, leftRayDirection * rayRange );
                Gizmos.DrawRay( transform.position, rightRayDirection * rayRange );
            }
                
        }
            
    }
#endif

    #endregion
    
    #region Helpers

    bool TargetVisible()
    {
        return Target != null || DamageSearching || SearchSettedTarget || SoundSearching;
    }

    public void PlayerDamaged(float damage, GameObject attacker)
    {
        if(General.AIType != UniversalAIEnums.AIType.Companion)
            return;
        
        DamageSearching = true;
        Detection.DetectionSettings.DetectObjects.Add(attacker);
        Target = attacker;

        if (attacker.tag.Equals(PlayerTag))
        {
            if(attacker.GetComponent<UniversalAIPlayerReference>() != null)
                CurrentPlayerRef = attacker.GetComponent<UniversalAIPlayerReference>();
        }
        
        if (TagStorage.AvailableAITags.Contains(attacker.tag))
        {
            if(attacker.GetComponent<UniversalAISystem>() != null)
                CurrentAITarget = attacker.GetComponent<UniversalAISystem>();
        }
    }

    public float GetSqrMagnitude(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 - pos2).sqrMagnitude;
    }

    public void CheckSound(float Radius, Transform Source)
    {

        if (GetSqrMagnitude(Source.position, transform.position) <= (Radius * Radius))
        {
     
            if (Source.tag.Equals(PlayerTag))
            {
                if (Source.GetComponent<UniversalAIPlayerReference>() != null)
                {
                    UniversalAIPlayerReference reference = Source.GetComponent<UniversalAIPlayerReference>();
                    foreach (var fact in Detection.Factions.FactionGroups)
                    {
                        if (fact.Faction == reference.PlayerFaction)
                        {
                            if (fact.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                            {
                                Alerted = true;
                                Approaching = true;
                                LastKnownPlayerPos = Source.position;
                                SoundSearching = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (TagStorage.AvailableAITags.Contains(Source.tag))
            {
                if (Source.GetComponent<UniversalAISystem>() != null)
                {
                    UniversalAISystem system = Source.GetComponent<UniversalAISystem>();
                    foreach (var fact in Detection.Factions.FactionGroups)
                    {
                        if (fact.Faction == system.Detection.Factions.Factions)
                        {
                            if (fact.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                            {
                                Alerted = true;
                                Approaching = true;
                                LastKnownPlayerPos = Source.position;
                                SoundSearching = true;
                                break;
                            }
                        }
                    }
                }
                    
            }
        }
    }
    
    public void SetDebugs()
    {
        if (Target != null )
        {
            Debug.CurrentTargetDebug = Target;   
        }
        else
        {
            Debug.CurrentTargetDebug = null;
        }

        Debug.TargetDistanceDebug = TargetDistance;

        Debug.VisibleTargetsDebug = VisibleTargets;

        Debug.HealthDebug = Health;

        if (TargetVisible())
        {
            Debug.TargetVisibleDebug = UniversalAIEnums.YesNo.Yes;
        }
        else
        {
            Debug.TargetVisibleDebug = UniversalAIEnums.YesNo.No;
        }

        if (Alerted)
        {
            Debug.AIAlertedDebug = UniversalAIEnums.YesNo.Yes;
        }
        else
        {
            Debug.AIAlertedDebug = UniversalAIEnums.YesNo.No;
        }
        
        if (Attacking)
        {
            Debug.AIAttacking = UniversalAIEnums.YesNo.Yes;
        }
        else
        {
            Debug.AIAttacking = UniversalAIEnums.YesNo.No;
        }

        if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
        {
            Debug.AITurning = Anim.GetInteger("Turn") != 0
                ? UniversalAIEnums.YesNo.Yes
                : UniversalAIEnums.YesNo.No;
        }

        //Debug.AICurrentAnimation = Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

   
    
    void CheckAttackType(AttackerType attackerType, GameObject attacker)
    {
        if (attackerType == AttackerType.Player && General.AIType != UniversalAIEnums.AIType.Enemy)
        {
            return;
        }


        if (attackerType == AttackerType.Player)
        {
            Detection.Factions.CanDetectPlayer = UniversalAIEnums.YesNo.Yes;
        }
        
        if(!Detection.DetectionSettings.DetectObjects.Contains(attacker))
          Detection.DetectionSettings.DetectObjects.Add(attacker);

        Alerted = true;
            
            if (GetSqrMagnitude(transform.position, attacker.transform.position) <=
                (Detection.DetectionSettings.DetectionDistance * 3f) * (Detection.DetectionSettings.DetectionDistance * 3f))
            {
                if (attacker.tag.Equals(PlayerTag))
                {
                    if(attacker.GetComponent<UniversalAIPlayerReference>() != null)
                        CurrentPlayerRef = attacker.GetComponent<UniversalAIPlayerReference>();
                }
        
                if (TagStorage.AvailableAITags.Contains(attacker.tag))
                {
                    if (attacker.GetComponent<UniversalAISystem>() != null)
                    {
                        if (attacker.GetComponent<UniversalAISystem>().Detection.Factions.Factions.Equals(Detection.Factions.Factions))
                        {
                            CurrentAITarget = attacker.GetComponent<UniversalAISystem>();
                        }
                    }
                }
                
                DamageSearching = true;
                Target = attacker;
                LastKnownPlayerPos = Vector3.zero;
                UniversalAIEvents.OnTargetVisible.Invoke();
                CanAttack = true;
                Attacking = false;
                Alerted = true;
                
                
                
                SetCombatState(true);
            }
            else
            {
                Alerted = true;
                Approaching = false;
                LastKnownPlayerPos = attacker.transform.position + new Vector3(5, 0, 5);
            }
    }



    public void PlayAnimation(AnimationType animationType)
    {
        if (UniversalAISounds.AudioSource == null)
        {
            if(GetComponent<AudioSource>())
                UniversalAISounds.AudioSource = UniversalAISounds.GetComponent<AudioSource>();
        }
        
        if (UniversalAISounds.AudioSource != null)
            UniversalAISounds.AudioSource.Stop();
        
        if (animationType == AnimationType.Idle)
        {
            AdjustNavmesh(true);

            Anim.SetBool("Idle Active",true);

            
            if (Anim.GetBool("Idle Active"))
            {
                if (Anim.GetBool("Combating"))
                {
                    Anim.SetInteger("Idle Index",Random.Range(0, CombatIdleCount));
                }
                else
                {
                    Anim.SetInteger("Idle Index",Random.Range(0,IdleCount));      
                }   
            }


          
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
            UniversalAISounds.PlaySound(AudioType.Idle);
        }
        
      
        
        if (animationType == AnimationType.Walk)
        {
            if (!Settings.Movement.UseRootMotionC)
            {
#if !UniversalAI_Integration_PathfindingPro
                Nav.speed = Settings.Movement.WalkSpeed;
#endif
            }
            
            AdjustNavmesh(false);
            Anim.SetBool("Idle Active",false);
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
        }
        
        if (animationType == AnimationType.Run)
        {
            if (!Settings.Movement.UseRootMotionC)
            {
#if !UniversalAI_Integration_PathfindingPro
                Nav.speed = Settings.Movement.RunSpeed;
#endif
            }
            
            AdjustNavmesh(false);
            Anim.SetBool("Idle Active",false);
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
        }
        
        if (animationType == AnimationType.Death)
        {
            UniversalAIEvents.OnDeath.Invoke();
            Anim.SetBool("Idle Active",false);
            Anim.SetInteger("Die Index",Random.Range(0,DeathCount));
            Anim.SetTrigger("Die");
            UniversalAISounds.PlaySound(AudioType.Death);
        }

        if (animationType == AnimationType.Attack)
        {
            if (!IsWeapon)
            {
                Vector2 RandomDamage = damages[Random.Range(0, damages.Count)];
                CurrentDamage = Random.Range(RandomDamage.x, RandomDamage.y);   
            }
            UniversalAIEvents.OnAttack.Invoke();
            UniversalAISounds.PlaySound(AudioType.Attack);
            int attackrandom = Random.Range(0, AttackCount);
            Anim.SetBool("Idle Active",false);
            Anim.SetInteger("Attack Index",attackrandom);
            Invoke("StartFire", 0.15f);
            Anim.SetBool("Attack",true);
            
            if(!IsShooter) 
                Invoke("StopAttack", 0.3f);
            
            if (CurrentAITarget != null && IsWeapon && !IsShooter)
            {
                if(CurrentAITarget.Target == null || Anim == null)
                    return;
                
                if(Anim.GetBool("Attack") && CurrentAITarget.Target.Equals(gameObject))
                    CurrentAITarget.BlockAlert();
            }
        }

        if (animationType == AnimationType.GetHit)
        {
            UniversalAIEvents.OnGetHit.Invoke();
            Anim.SetTrigger("GotHit");
            // Anim.SetBool("Idle Active",false);
            Anim.SetInteger("Hit Index",Random.Range(0,HitCount));

            GotHit = true;
            BodyWeight = 0;
            elapsedTime = 0;
            UseIK = false;
          
            InvokeRepeating("HitCheck",0.1f,0.2f);
        }
        
        if (animationType == AnimationType.Block)
        {
            Anim.SetTrigger("Block");
            Anim.SetBool("Idle Active",true);
            Anim.SetInteger("Block Index",Random.Range(0,BlockCount));
        }
        
        if (animationType == AnimationType.Reload)
        {
            Anim.SetTrigger("Reload");
            Anim.SetBool("Idle Active",false);
        }
    }

    private bool HitStarted;
    private bool BlockStarted;
    
    public void BlockAlert()
    {
        if(!UseBlockSystem || Attacking)
            return;

        if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Block") || Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop") ||
            Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Anim.ResetTrigger("Block");
              return;
        }
          
        
      
        if (Random.Range(0, 100) <= Settings.Attack.BlockChance)
        {
            UniversalAIEvents.OnBlocked.Invoke();
            IsBlocking = true;
            InvokeRepeating("BlockCheck",0.1f,0.2f);
            PlayAnimation(AnimationType.Block);
        }
    }
    
    public void BlockCheck()
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
        {
            Anim.ResetTrigger("GotHit");
            BlockStarted = true;
            PlayAnimation(AnimationType.Idle);
        }

        if (BlockStarted || CurrentAITarget == null)
        {
            AdjustNavmesh(true);
            Anim.SetFloat("Speed", 0);
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
            {
                BlockStarted = false;
                IsBlocking = false;
                
                Anim.ResetTrigger("Block");

                CancelInvoke("BlockCheck");
            }
        }
      
    }
    public void HitCheck()
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
        {
            HitStarted = true;
            
            if(UseBlockSystem)
                Anim.ResetTrigger("Block");
        }

        if (HitStarted)
        {
            AdjustNavmesh(true);
            Anim.SetFloat("Speed", 0);
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
            {

                HitStarted = false;
                GotHit = false;
              
                if(UseBlockSystem)
                    Anim.ResetTrigger("Block");
                
                Anim.ResetTrigger("GotHit");
                if(InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes && Target != null && Anim.GetBool("Equipped"))
                    EnableIK();
            
                CancelInvoke("HitCheck");
            }
        }
      
    }
    
    public void WaitForTriggerEvent()
    {
        TriggerHitboxAttacks.Invoke(true);
    }

    private void OnAnimatorMove()
    {
        if(Time.deltaTime <= 0)
            return;
        
        try
        {
#if UniversalAI_Integration_PathfindingPro
            
            if(Settings.Movement.UseRootMotionC && (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Run || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Walk || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp))
                NavPro.maxSpeed = (Anim.deltaPosition / Time.deltaTime).magnitude;
            
#else
            if(Settings.Movement.UseRootMotionC && (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Run || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Walk || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp))
                Nav.speed = (Anim.deltaPosition / Time.deltaTime).magnitude;
#endif
            
        }
        catch{ }

#if !UniversalAI_Integration_PathfindingPro
        Nav.nextPosition = transform.position;
#endif
    }
    

    public void AdjustNavmesh(bool stop, AdjustNav Rotation = AdjustNav.None, AdjustNav Position = AdjustNav.None, AdjustNav Stopped = AdjustNav.None)
    {
        if(Health <= 0)
            return;

        #region NavmeshAgent Version

#if !UniversalAI_Integration_PathfindingPro

        if (stop)
        {
           
            if (Stopped == AdjustNav.None)
            {
                Nav.isStopped = true;
            }
            else
            {
                Nav.isStopped =  Stopped == AdjustNav.True;
            }
            
            if (Rotation == AdjustNav.None)
            {
                Nav.updateRotation = false;
            }
            else
            {
                Nav.updateRotation =  Rotation == AdjustNav.True;
            }
            
            if (Position == AdjustNav.None)
            {
                Nav.updatePosition = false;
            }
            else
            {
                Nav.updatePosition =  Rotation == AdjustNav.True;
            }
            
            Nav.ResetPath();
        }
        else
        {
            if (Stopped == AdjustNav.None)
            {
                Nav.isStopped = false;
            }
            else
            { 
                Nav.isStopped =  Stopped == AdjustNav.True;
            }
            
            if (Rotation == AdjustNav.None)
            { 
                Nav.updateRotation = true;
            }
            else
            { 
                Nav.updateRotation =  Rotation == AdjustNav.True;
            }

            if (Position == AdjustNav.None)
            { 
                Nav.updatePosition = true;
            }
            else
            {
                Nav.updatePosition =  Rotation == AdjustNav.True;
            }
        }
        
#endif
        #endregion
        
        #region AI Path Version

#if UniversalAI_Integration_PathfindingPro

        if (stop)
        {
           
            if (Stopped == AdjustNav.None)
            {
                NavPro.isStopped = true;
            }
            else
            {
                NavPro.isStopped =  Stopped == AdjustNav.True;
            }
            
            if (Rotation == AdjustNav.None)
            {
                NavPro.updateRotation = false;
            }
            else
            {
                NavPro.updateRotation =  Rotation == AdjustNav.True;
            }
            
            if (Position == AdjustNav.None)
            {
                NavPro.updatePosition = false;
            }
            else
            {
                NavPro.updatePosition =  Rotation == AdjustNav.True;
            }
            
            NavPro.SetPath(null);
        }
        else
        {
            if (Stopped == AdjustNav.None)
            {
                NavPro.isStopped = false;
            }
            else
            { 
                NavPro.isStopped =  Stopped == AdjustNav.True;
            }
            
            if (Rotation == AdjustNav.None)
            { 
                NavPro.updateRotation = true;
            }
            else
            { 
                NavPro.updateRotation =  Rotation == AdjustNav.True;
            }

            if (Position == AdjustNav.None)
            { 
                NavPro.updatePosition = true;
            }
            else
            {
                NavPro.updatePosition =  Rotation == AdjustNav.True;
            }
        }
        
#endif
        
        #endregion
    }
    
    bool CalculateNewPath(Vector3 pos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        Nav.CalculatePath(pos, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }

        return true;
    }
    
   void SetDirection()
    {
        if (Debug.AICurrentStateDebug != UniversalAIEnums.AICurrentState.BackingUp && Debug.AICurrentStateDebug != UniversalAIEnums.AICurrentState.BackingUpPlayer)
        {
#if UniversalAI_Integration_PathfindingPro
            Vector3 velocity = Quaternion.Inverse(transform.rotation) * NavPro.desiredVelocity;
#else
Vector3 velocity = Quaternion.Inverse(transform.rotation) * Nav.desiredVelocity;
#endif
            float angle = Mathf.Atan2(velocity.x, velocity.z) * 180.0f / 3.14159f;
            Anim.SetFloat("Direction",angle, 0.25f,Time.deltaTime);   
        }
    }
    
    Vector3 GetRandomLocation(float walkRadius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

#if !UniversalAI_Integration_PathfindingPro
      NavMeshHit hit;


      if (!NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
      {
        
          return GetRandomLocation(walkRadius);
      }
      
      Vector3 finalPosition = hit.position;
   
            
      if(finalPosition == Vector3.positiveInfinity || finalPosition == Vector3.negativeInfinity || finalPosition.x.Equals(Mathf.Infinity) || !CalculateNewPath(finalPosition))
          return GetRandomLocation(walkRadius);
#else
        GraphNode randomNode;


        var grid = AstarPath.active.data.gridGraph;

        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];

        if (!randomNode.Walkable)
        {
            GetRandomLocation(walkRadius);
        }
        
        Vector3 finalPosition = (Vector3)randomNode.position;
#endif
        return finalPosition;
    }

    public void FreezeAIManuel(bool freeze)
    {

        if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Death)
            return;

        Frozen = freeze;

        UniversalAIEvents.OnFreeze.Invoke(Frozen);
        
        Anim.speed = Frozen ? 0 : 1;

        if (Frozen)
        {
            OldState = Debug.AICurrentStateDebug;
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Frozen;
        }
        else
        {
            Debug.AICurrentStateDebug = OldState;
        }
        
#if !UniversalAI_Integration_PathfindingPro
      if (Frozen)
        {
            Unfreeze = !Nav.isStopped;
            Nav.isStopped = true;
            Nav.updatePosition = false;
            Nav.updateRotation = false;
        }
        else if (Unfreeze)
        {
            Nav.isStopped = false;
            Nav.updatePosition = true;
            Nav.updateRotation = true;
        }
#else
        if (Frozen)
        {
            Unfreeze = !NavPro.isStopped;
            NavPro.isStopped = true;
            NavPro.updatePosition = false;
            NavPro.updateRotation = false;
        }
        else if (Unfreeze)
        {
            NavPro.isStopped = false;
            NavPro.updatePosition = true;
            NavPro.updateRotation = true;
        }
#endif
        
    }
    
    public void Revive()
    {
        Health = Stats.StartHealth;
#if !UniversalAI_Integration_PathfindingPro
        Nav.enabled = true;
#else
        NavPro.enabled = true;
#endif 
        Anim.enabled = true;
        
        this.enabled = true;
        Anim.SetTrigger("Revive");
        UniversalAIEvents.OnRevived.Invoke();
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
    }

    #endregion
    
    #region IK SETTINGS
        
    public void EnableIK()
    {
        
        if (Reloading || GotHit)
            return;
        
        UseIK = true;
        elapsedTime = 0;
        
        if(Target != null)
          StartCoroutine(FadeInBodyIK());
    }
    
    public void DisableIK()
    {
        UseIK = false;
        elapsedTime = 0;
        StartCoroutine(FadeOutBodyIK());
    }
    
    public void CheckIK()
    {
        if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
            InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && IsShooter && BodyWeight < 0.5f && General.AIConfidence == UniversalAIEnums.AIConfidence.Brave && !GotHit && !Reloading)
        {
            if(IsEquipping && !Anim.GetBool("Equipped") || IsEquipping && Anim.GetCurrentAnimatorStateInfo(0).IsName("Equip"))
                return;
            
            StartCoroutine(FadeInBodyIK());
        }
    }

    private bool FirstIKDetect = true;
    IEnumerator FadeInBodyIK ()
    {
        if (Reloading || GotHit)
        {
            BodyWeight = 0;
            yield return null;
        }

        if (FleeAway)
        {
            BodyWeight = 0;
            yield return null;
        }
        
        float T = 0;
        float StartingBodyWeight = BodyWeight;

      
        
        
        while (T < 1f)
        {
            if (Target == null)
            {
                BodyWeight = 0;
                break;
            }
            
            T += Time.deltaTime * 0.5f;
            float Reference = Mathf.LerpAngle(StartingBodyWeight, 1, T);
            BodyWeight = Reference;
            yield return null;
        }
        
        if (Target != null)
        {
            BodyWeight = 1;   
        }
        else
        {
            BodyWeight = 0;
        }
    }

    IEnumerator FadeOutBodyIK()
    {
      
        float T = 0;
        float StartingBodyWeight = BodyWeight;

        T = 0.55f;
        StartingBodyWeight = 0.55f;

        while (T < 1f && !Reloading && !FleeAway)
        {
            if (Target != null)
            {
                BodyWeight = 1;
                break;
            }
            
            T += Time.deltaTime * 0.5f;
            BodyWeight = Mathf.LerpAngle(StartingBodyWeight, 0, T);
            yield return null;
        }

        if (Target == null || Reloading || FleeAway)
        {
            BodyWeight = 0;
        }
            
    }
    
    private void LateUpdate()
    {
        if (!AIReady)
            return;

        if (Target == null && !Alerted)
        {
            FirstIKDetect = true;
        }
        
        if(Anim.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
            return;

        if (Anim.GetInteger("Idle Index") >= CombatIdleCount && Anim.GetBool("Combating"))
        {
            Anim.SetInteger("Idle Index", 0);
        }
        
        if (Anim.GetInteger("Idle Index") >= IdleCount && !Anim.GetBool("Combating"))
        {
            Anim.SetInteger("Idle Index", 0);
        }

        AITargetDead();
        
#if UniversalAI_Integration_Puppetmaster

            if (_behaviourPuppet == null)
            {
                BehaviourPuppet[] puppets = _puppetMaster.transform.root
                    .GetComponentsInChildren<BehaviourPuppet>();
                
                if(puppets.Length != 1)
                    return;
                
                if (puppets[0] != null)
                {
                    _behaviourPuppet = puppets[0];
                }
            }
            else if (_behaviourPuppet.state != BehaviourPuppet.State.Puppet)
            {
                return;
            }
           
            
#endif

        if(DamageSearching || SearchSettedTarget || SoundSearching)
            return;

        if (Reloading || GotHit)
        {
            BodyWeight = 0;
            return;
        }
         
        
        if (Target != null)
        {
            if (AITargetDead())
                return;
        }
        
        if(!AIReady)
            return;
        
        if (Health <= 0 && !Frozen && !StopAIBehaviour)
        {
#if !UniversalAI_Integration_PathfindingPro
        if (TargetDistance > Settings.Attack.AttackDistance || Nav.updatePosition || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp)
                DistanceFixLerp = Mathf.Lerp(DistanceFixLerp, 1, Time.deltaTime * 3);
            else if (TargetDistance <= Settings.Attack.AttackDistance)
                DistanceFixLerp = Mathf.Lerp(DistanceFixLerp, 0, Time.deltaTime * 6);
#else
            if (TargetDistance > Settings.Attack.AttackDistance || NavPro.updatePosition || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp)
                DistanceFixLerp = Mathf.Lerp(DistanceFixLerp, 1, Time.deltaTime * 3);
            else if (TargetDistance <= Settings.Attack.AttackDistance)
                DistanceFixLerp = Mathf.Lerp(DistanceFixLerp, 0, Time.deltaTime * 6);
#endif 
           
        }

      
        
        if (IsShooter && Target != null && InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes && !Reloading)
        {
        
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < boneTransforms.Length; j++)
                {
                    Transform bone = boneTransforms[j];
                    if (!Frozen && !StopAIBehaviour)
                    {
                        if (CheckAngle(GetTargetPosition()))
                        {
                            if (FirstIKDetect)
                            {
                                FixedLookPosition = GetTargetPosition();
                                FirstIKDetect = false;
                            }
                            FixedLookPosition = Vector3.SmoothDamp(FixedLookPosition, GetTargetPosition(), ref TheVel, InverseKinematics.AimIKSmooth, 20);
                            AimAtTarget(bone, FixedLookPosition, BodyWeight * InverseKinematics.AimIKWeight);
                        }
                    }
                    else
                    {
                        AimAtTarget(bone, FixedLookPosition, BodyWeight * InverseKinematics.AimIKWeight);
                    }
                }
            }
        }
        else if (IsShooter && Target == null && InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes && !Reloading)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < boneTransforms.Length; j++)
                {
                    Transform bone = boneTransforms[j];
                    AimAtTarget(bone, FixedLookPosition, BodyWeight * InverseKinematics.AimIKWeight);
                }
            }
        }

        if (InverseKinematics.UseLookAtIK == UniversalAIEnums.YesNo.Yes &&
            InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes)
        {
            LookAtIK();
        }
        
    }

    private bool CheckAngle(Vector3 pos)
    {
        Vector3 direction = (new Vector3(pos.x, pos.y + pos.y / 2, pos.z)) - Detection.DetectionSettings.HeadTransform.position;
        float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), transform.forward);

        return angle <= InverseKinematics.AngleLimit / 2;
    }
    
    private bool CheckLookAngle(Vector3 pos)
    {
        Vector3 direction = (new Vector3(pos.x, pos.y + pos.y / 2, pos.z)) - Detection.DetectionSettings.HeadTransform.position;
        float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), transform.forward);

        return angle <= InverseKinematics.LookAtAngleLimit / 2;
    }
    
    private bool CheckLookPossible(Vector3 pos)
    {
        Vector3 direction = (new Vector3(pos.x, pos.y + pos.y / 2, pos.z)) - Detection.DetectionSettings.HeadTransform.position;
        float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), transform.forward);

        return angle <= InverseKinematics.LookAtAngleLimit / 2 && GetSqrMagnitude(pos, Detection.DetectionSettings.HeadTransform.position) <= InverseKinematics.LookAtDistanceLimit * InverseKinematics.LookAtDistanceLimit;
    }

   
    private void LookAtIK()
    {
        Collider[] PossibleTargets = Physics.OverlapSphere(transform.position,
            Detection.DetectionSettings.DetectionDistance, InverseKinematics.LookAtLayers);
        
        
        if (PossibleTargets.Length > 0)
        {
            List<Transform> PossibleTransforms = new List<Transform>();

            foreach (var postarget in PossibleTargets)
            {
                if (CheckLookPossible(postarget.transform.position))
                {
                    PossibleTransforms.Add(postarget.transform);
                }
            }

            if (CurrentLookTarget != null && !PossibleTransforms.Contains(CurrentLookTarget))
            {
                CurrentLookTarget = null;
            }
            
            if (PossibleTransforms.Count > 0)
            {
                DistanceChecker Min = new DistanceChecker{DistanceCalc = 9999f, TheTarget = null};

                foreach (var Targ in PossibleTransforms)
                {
                    float dist = GetSqrMagnitude(transform.position, Targ.position);
                    if (dist < (Min.DistanceCalc * Min.DistanceCalc))
                    {
                        Min.DistanceCalc = dist;    
                        Min.TheTarget = Targ.gameObject;
                    }
                }

                if (Min.TheTarget != null)
                {
                    CurrentLookTarget = Min.TheTarget.transform;
                }
                    
            }
            
        }
        else
        {
            CurrentLookTarget = null;
        }
    }
    [HideInInspector] public bool IgnoreNavmeshRadius = false;
#if !UniversalAI_Integration_PathfindingPro
    [HideInInspector] public bool IgnoreColliderRadius = false;
#endif
    private Transform CurrentLookTarget = null;
    private float WeightGoal = 0;
    private float LookWeight = 0;
    private Vector3 CurrentLookPosition;
    private Vector3 LerpedLookPosition;
    private float UnityHeadIK = 0;
    private float UnityHeadIKGoal = 0;
    private Vector3 UnityIKLookPos;
    private void OnAnimatorIK(int layerIndex)
    {

        if(InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.No)
            return;
        
        if(Frozen || StopAIBehaviour)
            return;

        if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
            InverseKinematics.UseLookAtIK == UniversalAIEnums.YesNo.Yes && CurrentLookTarget != null && !Reloading && !DamageSearching && !GotHit && !SearchSettedTarget && !SoundSearching)
        {
            if (GetSqrMagnitude(CurrentLookTarget.transform.position, transform.position) <=
                (InverseKinematics.LookAtDistanceLimit * InverseKinematics.LookAtDistanceLimit))
            {

                UnityHeadIKGoal = InverseKinematics.LookAtIKWeight;
                if (CheckLookPossible(CurrentLookTarget.transform.position))
                {
                    WeightGoal = 1;
                    CurrentLookPosition = CurrentLookTarget.position;
                }
            }
            else
            {
                WeightGoal = 0;
                float LastHeight = CurrentLookPosition.y;
                CurrentLookPosition = CurrentLookTarget.position + (Detection.DetectionSettings.HeadTransform.forward * 5);
                CurrentLookPosition.y = LastHeight;
            }

        }
        else if(InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
                InverseKinematics.UseLookAtIK == UniversalAIEnums.YesNo.Yes)
        {
            if (LookWeight <= 0.001f)
                LerpedLookPosition = Detection.DetectionSettings.HeadTransform.position + (Detection.DetectionSettings.HeadTransform.forward * 5);

            WeightGoal = 0;
        }
        
        LerpedLookPosition = Vector3.MoveTowards(LerpedLookPosition, CurrentLookPosition, Time.deltaTime * 4);
        
        UnityHeadIK = Mathf.Lerp(UnityHeadIK, UnityHeadIKGoal, Time.deltaTime);

        if (WeightGoal.Equals(1f) && LookWeight <= 0.9f || WeightGoal.Equals(0f) && LookWeight > 0.1f)
        {
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
                LookWeight = Mathf.LerpAngle(LookWeight, WeightGoal, Time.deltaTime * 2);
        }
        else
        {
            if (WeightGoal.Equals(1f) && LookWeight >= 0.9f)
            {
                LookWeight = 1;
            }
            if (WeightGoal.Equals(0f) && LookWeight <= 0.1f)
            {
                LookWeight = 0;
            }
        }
        
        Anim.SetLookAtWeight(LookWeight, 0, LookWeight, 0);
        
        UnityIKLookPos = Vector3.Lerp(UnityIKLookPos, LerpedLookPosition, Time.deltaTime * 5);
        Anim.SetLookAtPosition(UnityIKLookPos);
        
        if (UseIK) 
        {
            if (InverseKinematics.UseHandIK == UniversalAIEnums.YesNo.Yes)
            {
                if (InverseKinematics.UseHandIKSmooth == UniversalAIEnums.YesNo.Yes)
                {
                    if (elapsedTime < InverseKinematics.HandIKSmooth)
                        elapsedTime += Time.deltaTime;

                    state = Mathf.Lerp(0, 1, elapsedTime / InverseKinematics.HandIKSmooth);

                    if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.Both)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyLeftHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyRightHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                    }
                    
                    Anim.SetIKPosition(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.rotation);

                    Anim.SetIKPosition(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.rotation);
                }
                else
                {
                    if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.Both)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1 * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1 * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1 * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1 * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyLeftHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1 * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1 * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyRightHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1 * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1 * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                    }

                    Anim.SetIKPosition(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.rotation);

                    Anim.SetIKPosition(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.rotation);
                }
            }
        }
        else 
        {

            if (InverseKinematics.UseHandIK == UniversalAIEnums.YesNo.Yes)
            {
                if (InverseKinematics.UseHandIKSmooth == UniversalAIEnums.YesNo.Yes)
                {
                    if (elapsedTime < InverseKinematics.HandIKSmooth)
                        elapsedTime += Time.deltaTime;

                    state = Mathf.Lerp(0, 1, elapsedTime / InverseKinematics.HandIKSmooth);
                    state = 1 - state;

                    if (IsWeapon && Anim.GetCurrentAnimatorStateInfo(0).IsName("UnEquip") || IsWeapon && !Anim.GetBool("Combating") && Anim.GetBool("Equipped"))
                        state = 0;
                    
             
                    if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.Both)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyLeftHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, state * InverseKinematics.HandIKWeight);
                    }
                    else if (InverseKinematics.HandIKType == UniversalAIEnums.HandIKType.OnlyRightHand)
                    {
                        Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                        Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, state * InverseKinematics.HandIKWeight);
                    
                        Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                        Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                    }
                    
                    Anim.SetIKPosition(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.rotation);

                    Anim.SetIKPosition(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.rotation);
                }
                else
                {
                    Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0 * InverseKinematics.HandIKWeight);
                    Anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0 * InverseKinematics.HandIKWeight);
                    
                    Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0 * InverseKinematics.HandIKWeight);
                    Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0 * InverseKinematics.HandIKWeight);
                    
                    Anim.SetIKPosition(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.RightHand, InverseKinematics.RightHandIK.rotation);
                    
                    Anim.SetIKPosition(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.position);
                    Anim.SetIKRotation(AvatarIKGoal.LeftHand, InverseKinematics.LeftHandIK.rotation);
                }
            }
        }
    }

    private float YOffset;
    Vector3 GetTargetPosition()
    {
        Vector3 targetdir = Vector3.zero;

        if (CurrentPlayerRef != null && CurrentPlayerRef.AimPosition != null)
        {
            targetdir = CurrentPlayerRef.AimPosition.position - AimTransform.position;    
        }
        else
        {
            targetdir = Target.transform.position - AimTransform.position;
        }
        
        Vector3 aimdirection = AimTransform.forward;

        YOffset = -(TargetDistance / 10);

        targetdir.Set(targetdir.x, targetdir.y + YOffset, targetdir.z);
        
        Vector3 dir = Vector3.Slerp(targetdir, aimdirection, 0);

        if (TargetDistance <= 1.7f && lastone != Vector3.zero)
        {
            return lastone;
        }
        lastone = AimTransform.position + dir + InverseKinematics.AimAtOffset;
        
        return lastone;
    }
    
    private void AimAtTarget(Transform bone, Vector3 TargetPos, float weight)
    {
        Vector3 RifleDirection = Vector3.Lerp(AimTransform.forward, Detection.DetectionSettings.HeadTransform.forward, DistanceFixLerp);
        Vector3 AimTransf = (Vector3.Lerp(AimTransform.position, Detection.DetectionSettings.HeadTransform.position, DistanceFixLerp)) + RifleDirection * -3;
        Vector3 TargetDirection = TargetPos - AimTransf;
        Quaternion AimDirect = Quaternion.FromToRotation(RifleDirection * 4, TargetDirection);
        Quaternion BlendAmount = Quaternion.Slerp(Quaternion.identity, AimDirect, weight);
        bone.rotation = BlendAmount * bone.rotation;
    }
   
    #endregion
    
    #region Main Voids

    private void OnApplicationQuit()
    {
        IsWeapon = false;
        IsShooter = false;
    }
    
#if UniversalAI_Integration_Puppetmaster

    private BehaviourPuppet _behaviourPuppet;
#endif
    public void Update()
    {
        
#if UniversalAI_Integration_Puppetmaster

            if (_behaviourPuppet == null)
            {
                BehaviourPuppet[] puppets = _puppetMaster.transform.root
                    .GetComponentsInChildren<BehaviourPuppet>();
                
                if(puppets.Length != 1)
                    return;
                
                if (puppets[0] != null)
                {
                    _behaviourPuppet = puppets[0];
                }
            }
            else if (_behaviourPuppet.state != BehaviourPuppet.State.Puppet)
            {
                return;
            }
           
            
#endif

        if(!AIReady)
            return;
        
        // if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
        // {
            
            if ((Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Idle || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Attack || Attacking) && (Anim.GetFloat("Speed") >= -0.08f && Anim.GetFloat("Speed") < 0 || Anim.GetFloat("Speed") <= 0.01f && Anim.GetFloat("Speed") > 0))
            {
                if (Attacking)
                {
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Attack;
                }
                
                Anim.SetFloat("Speed", 0);
            }
            
            if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Walk && Anim.GetFloat("Speed") >= 0.08f && Anim.GetFloat("Speed") <= 1f || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Walk && Anim.GetFloat("Speed") <= 0.05f && Anim.GetFloat("Speed") >= 0f)
            {
                Anim.SetFloat("Speed", 1);
            }
            
            if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Run && Anim.GetFloat("Speed") >= 1.95f)
            {
                Anim.SetFloat("Speed", 2);
            }
  
            if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Attack && (Anim.GetFloat("Speed") > 0.01f || Anim.GetFloat("Speed") < -0.01f))
            {
                Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),0,Time.deltaTime * 6f));
            }
        

            if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Walk)
            {
                Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),1,Time.deltaTime * 7f));
            }

            if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Run)
            {
                Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),2,Time.deltaTime * 7f));
            }   
        //}
        
        if(Frozen || StopAIBehaviour)
            return;

        if (Health > Stats.MaxHealth)
        {
            Health = Stats.MaxHealth;
        }

        if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop") || Anim.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
        {

            if (Attacking)
            {
                StopAttack();
            }
            AdjustNavmesh(true);
            
            if(Anim.GetFloat("Speed") == 0f)
                Anim.SetBool("Idle Active", true);
        }

        if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Idle)
        {
            Anim.SetBool("Idle Active", true);
        }
        
        if (DontGoCombat)
        {
            if (Anim.GetBool("Combating"))
            {
                SetCombatState(false);
            }
        }

        AITargetDead();
        
        Functionality();
        SetDirection();

        if (Stats.AIRegeneratesHealth == UniversalAIEnums.YesNo.Yes && Health < Stats.StopRegenerateHealthAmount && Health < Stats.MaxHealth)
        {
            if (Stats.AIRegeneratesWhen.Equals(UniversalAIEnums.AIRegenerate.Both))
            {
                if (!Regenerating)
                {
                    StartCoroutine(RegenerateHealth());   
                }   
            }
            else if(Stats.AIRegeneratesWhen.Equals(UniversalAIEnums.AIRegenerate.Combat) && TargetVisible())
            {
                if (!Regenerating)
                {
                    StartCoroutine(RegenerateHealth());   
                }      
            }
            else if(Stats.AIRegeneratesWhen.Equals(UniversalAIEnums.AIRegenerate.NonCombat) && !TargetVisible())
            {
                if (!Regenerating)
                {
                    StartCoroutine(RegenerateHealth());   
                }      
            }
            else
            {
                Regenerating = false;
                StopCoroutine("RegenerateHealth");
            }
        }
        else if(Stats.AIRegeneratesHealth == UniversalAIEnums.YesNo.Yes)
        {
            Regenerating = false;
            StopCoroutine("RegenerateHealth");
        }
        
        if(Debug.UpdateDebugs == UniversalAIEnums.YesNo.Yes)
            SetDebugs();
    }

    private bool CheckBackupState()
    {
      
        if (Target == null || SoundSearching)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
            PlayAnimation(AnimationType.Idle);
            return false;
        }
        
        Vector3 difftest = transform.position - Target.transform.position;
        difftest.y = 0;
        Vector3 BackupDestinationtest = Target.transform.position + difftest.normalized * 2f * Settings.Movement.TooCloseDistance;
#if !UniversalAI_Integration_PathfindingPro
             NavMeshPath path = new NavMeshPath();
        if (Nav.CalculatePath(BackupDestinationtest, path) && path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
#endif
       
        

        if (IsShooter && TargetDistance <= Settings.Movement.TooCloseDistance / 2.55f && TargetDistance > 0.1f || !IsShooter && TargetDistance <= Settings.Attack.AttackDistance)
        {
            
            if (DontGoCombat || SingleShot)
                return false;
        
            AdjustNavmesh(true);
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Attack;
            StartAttack();
            return false;
        }
        else if(IsShooter && TargetDistance > Settings.Movement.TooCloseDistance / 2.55f || !IsShooter && TargetDistance > Settings.Attack.AttackDistance)
        {
            StopAttack();
        }
        
        if (!Approaching && !UseIK && InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes)
        {
            Invoke("EnableIK", 0.62f);
        }

        Attacking = false;
        CanAttack = true;
     
                    
        // Turn
        SmoothTurn();
        
        Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"), -1 ,Time.deltaTime * 5.2f));
        Anim.SetBool("Idle Active", false);
     
#if !UniversalAI_Integration_PathfindingPro
       if(!Settings.Movement.UseRootMotionC)
            Nav.speed = Settings.Movement.WalkBackwardsSpeed;
#else
        if(!Settings.Movement.UseRootMotionC)
            NavPro.maxSpeed = Settings.Movement.WalkBackwardsSpeed;
#endif 
        
                                   
        AdjustNavmesh(false, AdjustNav.False);
        Vector3 diff = transform.position - Target.transform.position;
        diff.y = 0;
        Vector3 BackupDestination = Target.transform.position + diff.normalized * 2f * Settings.Movement.TooCloseDistance;
                
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.BackingUp;
            
        AdjustNavmesh(false, AdjustNav.False);

#if !UniversalAI_Integration_PathfindingPro
      if (TargetDistance <= Settings.Movement.TooCloseDistance)
        {
            Nav.SetDestination(BackupDestination);
        }
        else if(Nav.remainingDistance <= Nav.stoppingDistance + 0.1f)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Attack;
        }
#else
        if (TargetDistance <= Settings.Movement.TooCloseDistance)
        {
            NavPro.destination = BackupDestination;
        }
        else if(NavPro.remainingDistance <= Settings.Movement.StoppingDistance + 0.1f)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Attack;
        }
#endif 
        
            
        return true;
      
    }
    
    private bool CheckPlayerBackupState()
    {
      
        
    Anim.SetBool("Idle Active",false);
   
        // Turn
        SmoothTurn();
#if !UniversalAI_Integration_PathfindingPro
     if(!Settings.Movement.UseRootMotionC)
            Nav.speed = Settings.Movement.WalkBackwardsSpeed;
#else
        if(!Settings.Movement.UseRootMotionC)
            NavPro.maxSpeed = Settings.Movement.WalkBackwardsSpeed;
#endif
        
                                   
        AdjustNavmesh(false, AdjustNav.False);
        Vector3 diff = transform.position - TypeSettings.PlayerSettings.PlayerObject.transform.position;
        diff.y = 0.0f;
        Vector3 BackupDestination = TypeSettings.PlayerSettings.PlayerObject.transform.position + diff.normalized * 2f * TypeSettings.CompanionSettings.TooClosePlayerDistance;
                
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.BackingUpPlayer;
            
        AdjustNavmesh(false, AdjustNav.False);

#if !UniversalAI_Integration_PathfindingPro
       if (Nav.remainingDistance > Nav.stoppingDistance + 0.1f)
        {
            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"), -1 ,Time.deltaTime * 7f));
            Anim.SetBool("Idle Active", false);
        }
#else
        if (NavPro.remainingDistance > Settings.Movement.StoppingDistance + 0.1f)
        {
            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"), -1 ,Time.deltaTime * 7f));
            Anim.SetBool("Idle Active", false);
        }
#endif
       
#if !UniversalAI_Integration_PathfindingPro
      if (PlayerDistance <= TypeSettings.CompanionSettings.TooClosePlayerDistance)
        {
            Nav.SetDestination(BackupDestination);
        }
        else if(Nav.remainingDistance <= Nav.stoppingDistance + 0.1f)
        {
            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"), 0 ,Time.deltaTime * 7f));
       
            if (Anim.GetFloat("Speed") >= -0.08f)
            {
                Anim.SetFloat("Speed", 0);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;   
            }
        }
#else
        if (PlayerDistance <= TypeSettings.CompanionSettings.TooClosePlayerDistance)
        {
            NavPro.destination = BackupDestination;
        }
        else if(NavPro.remainingDistance <= Settings.Movement.StoppingDistance + 0.1f)
        {
            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"), 0 ,Time.deltaTime * 7f));
       
            if (Anim.GetFloat("Speed") >= -0.08f)
            {
                Anim.SetFloat("Speed", 0);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;   
            }
        }
#endif
        
            
        return true;
      
    }
    public IEnumerator RegenerateHealth()
    {
        if (!Regenerating)
        {
            Regenerating = true;
            yield return new WaitForSeconds(Stats.RegenerateStartDelay);
        }
        else
        {
            Regenerating = true;
            yield return new WaitForSeconds(1);
        }
        
        Health += Stats.RegenerateRate;

        if (Health > Stats.StopRegenerateHealthAmount)
        {
            Health = Stats.StopRegenerateHealthAmount;
        }
        
        if(Health < Stats.StopRegenerateHealthAmount && Health < Stats.MaxHealth)
           StartCoroutine(RegenerateHealth());
    }
    public void TakeDamage(float damageamount, AttackerType attackerType, GameObject Attacker, bool BlockSuccess = true)
    {
        if(Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Death)
        {
            return;
        }

        if (IsBlocking && !BlockSuccess)
        {
        }
        else
        {
            Health -= damageamount;
            UniversalAIEvents.OnTakeDamage.Invoke(damageamount);
            UniversalAISounds.PlaySound(AudioType.TakeDamage);
        }
    
       

        
        if (General.AIConfidence == UniversalAIEnums.AIConfidence.Neutral)
        {
            if (AttackCount > 0)
            {
                General.AIConfidence = UniversalAIEnums.AIConfidence.Brave;   
            }
            else
            {
                General.AIConfidence = UniversalAIEnums.AIConfidence.Coward;
            }
            
            ReturnToNeutral = true;
        }
        
        
        
        if (Health <= 0)
        {
            Die();
            return;
        }

        if (HitCount > 0  && !GotHit && !Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop") && !Reloading && !IsBlocking)
        {
            if(Random.Range(0, 101) <= Settings.Attack.HitReactionChance)
                PlayAnimation(AnimationType.GetHit);
        }
        
        if(Attacker != null && Target == null)
          CheckAttackType(attackerType, Attacker);
    }

    public void IgnoreStart()
    {
        Invoke("IgnoreDone", Anim.GetCurrentAnimatorClipInfo(0).Length + 0.15f);
    }
    public void IgnoreDone()
    {
        General.AIType = UniversalAIEnums.AIType.Enemy;
        Detection.Factions.CanDetectPlayer = UniversalAIEnums.YesNo.Yes;
    }

    private bool UnEquipping = false;
    public void Functionality()
    {

        if (AttackCount <= 0 && General.AIConfidence == UniversalAIEnums.AIConfidence.Brave)
        {
            UnityEngine.Debug.Log("Attack animations are required for brave confidence!");
            General.AIConfidence = UniversalAIEnums.AIConfidence.Coward;
        }
        
        if (DamageSearching && Target == null)
        {
            DamageSearching = false;
            AdjustNavmesh(true);
            PlayAnimation(AnimationType.Idle);
        }
        if (SoundSearching && Target == null)
        {
            SoundSearching = false;
            AdjustNavmesh(true);
            PlayAnimation(AnimationType.Idle);
        }
        
        if (UseBlockSystem)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
            {
                Anim.ResetTrigger("GotHit");
                Anim.ResetTrigger("Block");
                IsBlocking = true;
            }
            else
            {
                IsBlocking = false;
            }
            
            if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
            {
                Anim.ResetTrigger("Block");
            }
        }

       
        
        if (Attacking)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Attack;
        }

        if (SoundSearching)
        {
            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
            {
                Vector3 Direction = new Vector3(Target.transform.position.x, 0, Target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 DestinationDirection = Direction;
        
                if((int) GetDestinationAngle(Target.transform.position) >= Settings.Movement.TurnAngleLimit)
                {
                    AdjustNavmesh(true);
                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget, Time.deltaTime * Settings.Movement.TurnSpeed);
            
                    Vector3 cross = Vector3.Cross(transform.forward, Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                    if (cross.y >= 0)
                    {
                        Anim.SetInteger("Turn", 2);
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 1);
                    }
                    return;
                }
                else
                {
                    Anim.SetInteger("Turn", 0);
                }   
            }

            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
            PlayAnimation(AnimationType.Walk);
            AdjustNavmesh(false);
#if !UniversalAI_Integration_PathfindingPro
            Nav.SetDestination(Target.transform.position);
#else
        NavPro.destination = Target.transform.position;
#endif
        }
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("UnEquip") && !UnEquipping)
        {
            UnEquipping = true;

            if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes)
            {
                DisableIK();
                BodyWeight = 0;
                elapsedTime = 0;
            }
        }
        
        if(UnEquipping && !Anim.GetCurrentAnimatorStateInfo(0).IsName("UnEquip"))
        {
            UnEquipping = false;
        }
        
        
        if(Anim.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
            return;

        // if (Target != null && Detection.AlertSettings.CallingBackup == UniversalAIEnums.YesNo.No)
        // {
        //     if (Detection.AlertSettings.CallForBackup == UniversalAIEnums.YesNo.Yes)
        //     {
        //        CallForBackup();
        //     }
        // }
        // else if(Target == null && Detection.AlertSettings.CallingBackup == UniversalAIEnums.YesNo.Yes && Detection.AlertSettings.CallForBackup == UniversalAIEnums.YesNo.Yes)
        // {
        //     Detection.AlertSettings.CallingBackup = UniversalAIEnums.YesNo.No;
        // }
        
        if (Target != null)
        {
            TargetDistance = Vector3.Distance(Target.transform.position, transform.position);
            if (BodyWeight < 0.1f && InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes && InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && IsShooter)
            {
                CheckIK();
            }
        }
        else
        {
            TargetDistance = 0;
        }
        

        #region AIType Enemy

        if (General.AIType == UniversalAIEnums.AIType.Enemy)
        {
           EnemyFunctions();
        }
        #endregion
        
        
        #region AIType Companion

        if (General.AIType == UniversalAIEnums.AIType.Companion)
        {
         CompanionFunctions();  
        }

        #endregion
        
        #region AIType Pet

        if (General.AIType == UniversalAIEnums.AIType.Pet)
        {
            PetFunctions();  
        }

        #endregion
          
        
        #endregion
    }

    [HideInInspector] public float PlayerDistance = 0;

    private void PetFunctions()
    {
        if(TypeSettings.PlayerSettings.PlayerObject == null)
            return;
     
        PlayerDistance = Vector3.Distance(transform.position, TypeSettings.PlayerSettings.PlayerObject.transform.position);
        
        if (TypeSettings.PetSettings.PetBehaviour == PetBehaviour.Follow)
        {
            FollowPlayerPet();
        }
        else
        {
#if !UniversalAI_Integration_PathfindingPro
            if (Nav.updatePosition)
            {
                AdjustNavmesh(true);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
            }
#else
            if (NavPro.updatePosition)
            {
                AdjustNavmesh(true);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
            }
#endif
        }
        
    }

    private void CompanionFunctions()
    {
        #region Coward
        if (General.AIConfidence == UniversalAIEnums.AIConfidence.Coward)
        {
            if (FleeAway)
            {
                
#if !UniversalAI_Integration_PathfindingPro
     if (Nav.remainingDistance > Nav.stoppingDistance + 0.2f)
                {
                    return;
                }
#else
                if (NavPro.remainingDistance > NavPro.endReachedDistance)
                {
                    return;
                }
#endif
                FleeAway = false;
                    StopAlertMode();
            }

            if (TargetVisible())
            {
                if (!Alerted)
                {
                    Alerted = true;
                    UniversalAISounds.PlaySound(AudioType.Alerted);
                }
                
                if (!IsWeapon)
                {
                    SetCombatState(true);
                }

                Flee();
            }
            
            if(FleeAway)
                return;
        }

        #endregion
       
        if(TypeSettings.PlayerSettings.PlayerObject == null)
            return;
     
        PlayerDistance = Vector3.Distance(transform.position, TypeSettings.PlayerSettings.PlayerObject.transform.position);

        if (TypeSettings.CompanionSettings.AttackState == AttackState.Passive && Anim.GetBool("Combating"))
        {
            Alerted = false;
            SetCombatState(false);
        }
        else if (TypeSettings.CompanionSettings.AttackState == AttackState.Aggressive && (!Alerted && TargetVisible() || !Alerted && Searching))
        {
            if (!Alerted)
            {
                Alerted = true;
                UniversalAISounds.PlaySound(AudioType.Alerted);
            }


            if (Anim.GetBool("Idle Active"))
            {
                Anim.SetBool("Idle Active",false);
            }
            
            SetCombatState(true);

        }
        else if(Anim.GetBool("Combating") && !Alerted)
        {
            SetCombatState(false);
              
            Anim.SetBool("Idle Active",false);
        }
        
        if(Reloading)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
            AdjustNavmesh(true);
            return;
        }

        if (!SoundSearching)
        {
            if (TargetVisible() && TargetDistance <= Settings.Movement.TooCloseDistance && TargetDistance > 0 || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp)
            {
                if (General.AIConfidence != UniversalAIEnums.AIConfidence.Coward)
                {
                    if (CanBackup(2f * Settings.Movement.TooCloseDistance))
                    {
                        AdjustNavmesh(false, AdjustNav.False);
                        CheckBackupState();
                        return;      
                    }
                }
            }   
        }

        if (TypeSettings.CompanionSettings.AttackState == AttackState.Passive && Approaching)
        {
            Approaching = false;
            Attacking = false;
            CanAttack = true;
            LastKnownPlayerPos = Vector3.zero;
            StopAttack();
        }

        if (TypeSettings.CompanionSettings.AttackState == AttackState.Aggressive)
        {
            if (!TargetVisible() && Approaching)
            {
                if(LastKnownPlayerPos == Vector3.zero && Target != null)
                    LastKnownPlayerPos = Target.transform.position;

                if (Settings.Movement.ChaseTarget == UniversalAIEnums.YesNo.No)
                {
                    LastKnownPlayerPos = Vector3.zero;
                    Approaching = false;
                    Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
                    AdjustNavmesh(true);
                    UniversalAIEvents.OnTargetLost.Invoke();
                    UniversalAISounds.PlaySound(AudioType.LostTarget);
                }
            
                GoToLastKnownPosition();
            }   
        }

        if (!TargetVisible() && LastKnownPlayerPos == Vector3.zero || General.AIConfidence == UniversalAIEnums.AIConfidence.Coward)
        {
            if (TypeSettings.CompanionSettings.companionBehaviour == CompanionBehaviour.Follow)
            {
                FollowPlayerCompanion();   
            }
            else
            {
#if !UniversalAI_Integration_PathfindingPro
                if (Nav.updatePosition)
                {
                    AdjustNavmesh(true);
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                    PlayAnimation(AnimationType.Idle);
                }
#else
            if (NavPro.updatePosition)
            {
                AdjustNavmesh(true);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
            }
#endif
            }
        }
        else if(LastKnownPlayerPos == Vector3.zero && TypeSettings.CompanionSettings.AttackState == AttackState.Aggressive)
        {
            ApproachToTarget();
        }
    }

    private bool CanBackup(float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(1,1,0), -transform.forward, out hit, distance,
                -1, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }
    private void EnemyFunctions()
    {
     
         #region Brave

            if (General.AIConfidence == UniversalAIEnums.AIConfidence.Brave) 
            {
              
                if(Alerted && !Anim.GetBool("Combating"))
                    SetCombatState(true);
                
                if (!Alerted && TargetVisible() || !Alerted && Searching)
                {
                    if (!Alerted)
                    {
                        Alerted = true;
                        UniversalAISounds.PlaySound(AudioType.Alerted);
                    }


                    if (Anim.GetBool("Idle Active"))
                    {
                        Anim.SetBool("Idle Active",false);
                    }
                    
                    SetCombatState(true);
                    
                }
                else if (Anim.GetBool("Combating") && !Alerted)
                {
                    SetCombatState(false);

                    Anim.SetBool("Idle Active", false);
                }
                

                if(Reloading)
                {
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                    AdjustNavmesh(true);
                    return;
                }

                if (!SoundSearching)
                {
                    if (TargetVisible() && TargetDistance <= Settings.Movement.TooCloseDistance && TargetDistance > 0 || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp && !FleeAway)
                    {
                        if(CanBackup(2f * Settings.Movement.TooCloseDistance))
                        {
                            AdjustNavmesh(false, AdjustNav.False);
                            CheckBackupState();
                            return;   
                        }
                    }   
                }
                
                if (!TargetVisible() && Approaching)
                {
                    
                    if(LastKnownPlayerPos == Vector3.zero && Target != null)
                        LastKnownPlayerPos = Target.transform.position;
            
                    if (Settings.Movement.ChaseTarget == UniversalAIEnums.YesNo.No)
                    {
                        LastKnownPlayerPos = Vector3.zero;
                        Approaching = false;
                        Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
                        AdjustNavmesh(true);
                        UniversalAIEvents.OnTargetLost.Invoke();
                        UniversalAISounds.PlaySound(AudioType.LostTarget);
                    }
                    
                    GoToLastKnownPosition();
                }
        
                if (!TargetVisible() && LastKnownPlayerPos == Vector3.zero || TargetVisible() && TargetDistance > Settings.Attack.AttackDistance && !CalculateNewPath(Target.transform.position) && LastKnownPlayerPos == Vector3.zero)
                {
                    if (!OvverideWandering)
                    {
                        Wander();      
                    }
                    else
                    {
                        OverrideWander();
                    }
                }
                else if(LastKnownPlayerPos == Vector3.zero)
                {
                    if (TargetDistance > Settings.Attack.AttackDistance && !CalculateNewPath(Target.transform.position))
                    {
                        
                    }
                    else
                    {
                        ApproachToTarget();   
                    }
                }
            }
        
            #endregion

        #region Coward
        if (General.AIConfidence == UniversalAIEnums.AIConfidence.Coward)
        {
                if (TargetVisible())
                {
                    FleeAway = true;
                    Flee();
                }
                else
                {
#if !UniversalAI_Integration_PathfindingPro
                    if (Nav.remainingDistance > Nav.stoppingDistance + 0.1f)
                    {
                        return;
                    }
#else
                if (NavPro.remainingDistance > NavPro.endReachedDistance)
                {
                    return;
                } 
#endif
                    
                    if (FleeAway)
                    {
                        FleeAway = false;
                        Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
                    }
                  
                }

            
                if (!TargetVisible() && !FleeAway)
                {
                    
                        if(Anim.GetBool("Combating") && !Alerted)
                        {
                            SetCombatState(false);
                        }
                
                    if (!OvverideWandering)
                    {
                        Wander();      
                    }
                    else
                    {
                        OverrideWander();
                    }
                }
                else
                {
                    if (!Alerted)
                    {
                        Alerted = true;
                        UniversalAISounds.PlaySound(AudioType.Alerted);
                    }
                
                    if (!IsWeapon)
                    {
                        SetCombatState(true);
                    }
                }
        }

        #endregion
       
        #region Neutral
        if (General.AIConfidence == UniversalAIEnums.AIConfidence.Neutral)
        {
            if(Anim.GetBool("Combating") && !Alerted)
            {
                Anim.SetBool("Idle Active",false);
            
                SetCombatState(false);
            }

            if (!TargetVisible())
            {
                if (!OvverideWandering)
                {
                    Wander();      
                }
                else
                {
                    OverrideWander();
                }
            }
        } 
    }

    private void OverrideWander()
    {
        OvverideWandering = true;

        if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
        {
            Vector3 Direction = new Vector3(OvverideWanderingPos.x, 0, OvverideWanderingPos.z) -
                                new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 DestinationDirection = Direction;

            if ((int) GetDestinationAngle(OvverideWanderingPos) >= Settings.Movement.TurnAngleLimit)
            {
                AdjustNavmesh(true);
                Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                    Time.deltaTime * Settings.Movement.TurnSpeed);

                Vector3 cross = Vector3.Cross(transform.forward,
                    Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                if (cross.y >= 0)
                {
                    Anim.SetInteger("Turn", 2);
                }
                else
                {
                    Anim.SetInteger("Turn", 1);
                }

                return;
            }
            else
            {
                Anim.SetInteger("Turn", 0);
            }
        }
#if !UniversalAI_Integration_PathfindingPro
        if (Nav.hasPath)
        {
            if (GetSqrMagnitude(transform.position,Nav.destination) > (Nav.stoppingDistance + 0.2f  * Nav.stoppingDistance + 0.2f))
            {
                PlayAnimation(AnimationType.Walk);
                return;
            }
            else
            {
                OvverideWandering = false;
                OvverideWanderingPos = Vector3.zero;
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
                AdjustNavmesh(true);
                return;
            }
        }
#else
        if (NavPro.hasPath)
        {
            if (GetSqrMagnitude(transform.position,NavPro.destination) > (NavPro.endReachedDistance * NavPro.endReachedDistance))
            {
                PlayAnimation(AnimationType.Walk);
                return;
            }
            else
            {
             
                OvverideWandering = false;
                OvverideWanderingPos = Vector3.zero;
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
                AdjustNavmesh(true);
                return;
            }
        }
#endif
        
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
        PlayAnimation(AnimationType.Walk);
        
        AdjustNavmesh(false);

#if !UniversalAI_Integration_PathfindingPro
        Nav.SetDestination(OvverideWanderingPos);
#else
        NavPro.destination = OvverideWanderingPos;
#endif
    }
    private void FollowPlayerCompanion()
    {
        if (PlayerDistance <= TypeSettings.CompanionSettings.TooClosePlayerDistance || Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUpPlayer)
        {
            if (CanBackup(2f * TypeSettings.CompanionSettings.TooClosePlayerDistance))
            {
                AdjustNavmesh(false, AdjustNav.False);
                CheckPlayerBackupState();
                return;   
            }
        }

        if (Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.BackingUp && General.AIConfidence != UniversalAIEnums.AIConfidence.Coward)
        {
            return;
        }

        if (PlayerDistance <= TypeSettings.CompanionSettings.FollowingStopDistance)
        {
            
#if !UniversalAI_Integration_PathfindingPro
        if (!Nav.isStopped)
            {
                AdjustNavmesh(true);
            }
#else
            if (!NavPro.isStopped)
            {
                AdjustNavmesh(true);
            }
#endif
           
            
            if(!Anim.GetBool("Idle Active"))
                PlayAnimation(AnimationType.Idle);
                
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle; 
       
        }
        else if(PlayerDistance > TypeSettings.CompanionSettings.FollowingStopDistance + 1.5f)
        {
            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
            {
                Vector3 Direction =
                    new Vector3(TypeSettings.PlayerSettings.PlayerObject.transform.position.x, 0,
                        TypeSettings.PlayerSettings.PlayerObject.transform.position.z) -
                    new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 DestinationDirection = Direction;

                if ((int) GetDestinationAngle(TypeSettings.PlayerSettings.PlayerObject.transform.position) >=
                    Settings.Movement.TurnAngleLimit)
                {
                    AdjustNavmesh(true);
                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                        Time.deltaTime * Settings.Movement.TurnSpeed);

                    Vector3 cross = Vector3.Cross(transform.forward,
                        Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                    if (cross.y >= 0)
                    {
                        Anim.SetInteger("Turn", 2);
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 1);
                    }

                    return;
                }
                else
                {
                    Anim.SetInteger("Turn", 0);
                }
            }
#if !UniversalAI_Integration_PathfindingPro
        if (!Nav.isStopped)
            {
                AdjustNavmesh(false);
            }
#else
            if (!NavPro.isStopped)
            {
                AdjustNavmesh(false);
            }
#endif

            if (PlayerDistance <= TypeSettings.CompanionSettings.FollowingStartRunningDistance)
            {
                PlayAnimation(AnimationType.Walk);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
             
            }
            else if(PlayerDistance > TypeSettings.CompanionSettings.FollowingStartRunningDistance + 1.5f)
            {
                PlayAnimation(AnimationType.Run);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
              
            }
            
#if !UniversalAI_Integration_PathfindingPro
         Nav.SetDestination(TypeSettings.PlayerSettings.PlayerObject.transform.position);
#else
            NavPro.destination = TypeSettings.PlayerSettings.PlayerObject.transform.position;
#endif

        }

    }
    
     private void FollowPlayerPet()
    {
        
        if (PlayerDistance <= TypeSettings.PetSettings.FollowingStopDistance)
        {
            
#if !UniversalAI_Integration_PathfindingPro
        if (!Nav.isStopped)
            {
                AdjustNavmesh(true);
            }
#else
            if (!NavPro.isStopped)
            {
                AdjustNavmesh(true);
            }
#endif
           
            
            if(!Anim.GetBool("Idle Active"))
                PlayAnimation(AnimationType.Idle);
                
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle; 
       
        }
        else if(PlayerDistance > TypeSettings.PetSettings.FollowingStopDistance + 1.5f)
        {
            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
            {
                Vector3 Direction =
                    new Vector3(TypeSettings.PlayerSettings.PlayerObject.transform.position.x, 0,
                        TypeSettings.PlayerSettings.PlayerObject.transform.position.z) -
                    new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 DestinationDirection = Direction;

                if ((int) GetDestinationAngle(TypeSettings.PlayerSettings.PlayerObject.transform.position) >=
                    Settings.Movement.TurnAngleLimit)
                {
                    AdjustNavmesh(true);
                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                        Time.deltaTime * Settings.Movement.TurnSpeed);

                    Vector3 cross = Vector3.Cross(transform.forward,
                        Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                    if (cross.y >= 0)
                    {
                        Anim.SetInteger("Turn", 2);
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 1);
                    }

                    return;
                }
                else
                {
                    Anim.SetInteger("Turn", 0);
                }
            }

#if !UniversalAI_Integration_PathfindingPro
        if (!Nav.isStopped)
            {
                AdjustNavmesh(false);
            }
#else
            if (!NavPro.isStopped)
            {
                AdjustNavmesh(false);
            }
#endif

            if (PlayerDistance <= TypeSettings.PetSettings.FollowingStartRunningDistance)
            {
                PlayAnimation(AnimationType.Walk);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
             
            }
            else if(PlayerDistance > TypeSettings.PetSettings.FollowingStartRunningDistance + 1.5f)
            {
                PlayAnimation(AnimationType.Run);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
              
            }
            
#if !UniversalAI_Integration_PathfindingPro
         Nav.SetDestination(TypeSettings.PlayerSettings.PlayerObject.transform.position);
#else
            NavPro.destination = TypeSettings.PlayerSettings.PlayerObject.transform.position;
#endif

        }

    }

     public void EquipCheck()
     {
         if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Equip"))
         {
             Equipped();
             CancelInvoke("EquipCheck");
         }
     }
     
     public void SetCombatState(bool enabled)
     {
         if (enabled && !DontGoCombat)
         {
             if (!IsEquipping)
             {
                 Equipped();
             }
             else
             {
                 InvokeRepeating("EquipCheck", 0.2f, 0.1f);
             }
         }
         else if(!enabled)
         {
             if (IsEquipping)
             {
                 DisableIK();
                 OnUnEquipped.Invoke();
                 UniversalAIEvents.OnHolster.Invoke();
                 Invoke("UnEquipped", 0.4f);
             }
             else
             {
                 UnEquipped();
             }
         }
         
         if(DontGoCombat)
             Anim.SetBool("Combating", false);
         else
             Anim.SetBool("Combating", enabled);

         if (!enabled && IsWeapon && !AlwaysEquipped)
         {
             DisableIK();
         }

     }
    public void Flee()
    {
        if (BodyWeight > 0)
        {
            BodyWeight = 0;
        }
        
        if(Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
            return;
 
        SetCombatState(true);
        
        if (!DoubleIt)
        {
            DoubleIt = true;
            
            if (Detection.AlertSettings.DoubleTheDetectionDistance == UniversalAIEnums.YesNo.Yes)
            {
                Detection.DetectionSettings.DetectionDistance *= 1.5f;
            }
            
            if (Detection.AlertSettings.DoubleTheDetectionAngle == UniversalAIEnums.YesNo.Yes)
            {
                Detection.DetectionSettings.DetectionAngle *= 1.5f;
            }
        }
        
        
        
        FleeAway = true;

#if !UniversalAI_Integration_PathfindingPro
        if (Nav.isStopped)
        {
            AdjustNavmesh(false);
        }
#else
        if (NavPro.isStopped)
        {
            AdjustNavmesh(false);
        }
#endif
        
        if (Settings.Movement.AICanRun == UniversalAIEnums.YesNo.Yes)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
            PlayAnimation(AnimationType.Run);
        }
        if (Settings.Movement.AICanRun == UniversalAIEnums.YesNo.No)
        {
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
            PlayAnimation(AnimationType.Walk);
        }

#if !UniversalAI_Integration_PathfindingPro
        if (Nav.remainingDistance <= Settings.Movement.StoppingDistance + 0.1f)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            Vector3 GeneratedDestination = transform.position + -direction * General.FleeDistance + Random.insideUnitSphere * 10;
            GeneratedDestination.y = transform.position.y;
            
            NavMeshHit hit;
            NavMesh.SamplePosition(GeneratedDestination, out hit, 500, 1);
            Vector3 finalPosition = hit.position;

            if (finalPosition == Vector3.positiveInfinity || finalPosition == Vector3.negativeInfinity ||
                finalPosition.x.Equals(Mathf.Infinity) || !CalculateNewPath(finalPosition))
            {
                GeneratedDestination = UnityEngine.Random.insideUnitSphere * 10;
                GeneratedDestination += transform.position;
            }
            Nav.SetDestination(GeneratedDestination);    
        }
#else
        if (NavPro.remainingDistance <= NavPro.endReachedDistance || !NavPro.hasPath)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            Vector3 GeneratedDestination = transform.position + -direction * General.FleeDistance + Random.insideUnitSphere * 10;
            GeneratedDestination.y = transform.position.y;
            NavPro.destination =  GeneratedDestination;
            NavPro.SearchPath();
        }
       
#endif
       
     
    }
    public void StopAlertMode()
    {
        if(TargetVisible())
            return;
       
        SetCombatState(false);
        
        Alerted = false;

        if (ReturnToNeutral)
        {
            General.AIConfidence = UniversalAIEnums.AIConfidence.Neutral;
        }

        DoubleIt = false;
        
        if (Detection.AlertSettings.DoubleTheDetectionDistance == UniversalAIEnums.YesNo.Yes)
        {
            Detection.DetectionSettings.DetectionDistance /= 1.5f;
        }
        
        if (Detection.AlertSettings.DoubleTheDetectionAngle == UniversalAIEnums.YesNo.Yes)
        {
            Detection.DetectionSettings.DetectionAngle /= 1.5f;
        }
    }
    void GoToLastKnownPosition()
    {
  
        if (Attacking)
        {
            StopAttack();
        }

        if (UseIK && !Alerted)
        {
            Invoke("DisableIK", 0.1f);
        }
   
     
        PlayAnimation(AnimationType.Walk);
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;

#if !UniversalAI_Integration_PathfindingPro
        if (CalculateNewPath(LastKnownPlayerPos))
        {
            Nav.SetDestination(LastKnownPlayerPos);   
        }
        else
        {
            LastKnownPlayerPos = Vector3.zero;
            Approaching = false;
            Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
            Nav.ResetPath();
            UniversalAIEvents.OnTargetLost.Invoke();
            UniversalAISounds.PlaySound(AudioType.LostTarget);
        }
#else
        NavPro.destination = LastKnownPlayerPos;
#endif
       
#if !UniversalAI_Integration_PathfindingPro
       if (GetSqrMagnitude(transform.position, LastKnownPlayerPos) <= (Nav.stoppingDistance + 0.1f * Nav.stoppingDistance + 0.1f))
        {
            LastKnownPlayerPos = Vector3.zero;
            Approaching = false;
            Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
            Nav.ResetPath();
            UniversalAIEvents.OnTargetLost.Invoke();
            UniversalAISounds.PlaySound(AudioType.LostTarget);
        }
#else
        if (GetSqrMagnitude(transform.position, LastKnownPlayerPos) <= (Settings.Movement.StoppingDistance + 0.1f * Settings.Movement.StoppingDistance + 0.1f))
        {
            LastKnownPlayerPos = Vector3.zero;
            Approaching = false;
            Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
            UniversalAIEvents.OnTargetLost.Invoke();
            NavPro.SetPath(null);
            UniversalAISounds.PlaySound(AudioType.LostTarget);
        }
#endif
        
    }
    public void Equipped()
    {
        if (IsWeapon)
        {
            OnEquipped.Invoke();
            UniversalAIEvents.OnDraw.Invoke();
            Anim.SetBool("Equipped",true);
        }
    }
    public void UnEquipped()
    {
        if(IsWeapon)
            Anim.SetBool("Equipped",false);
    }
    
    public void EnableWeapon()
    {
       WeaponState(true);
    }
    public void DisableWeapon()
    {
      WeaponState(false);
    }

    public void SmoothTurn()
    {
        //Smooth look
      
        Quaternion targetRotation =
            Quaternion.LookRotation(
                Target.transform.position - transform.position);
       
        // Turn
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            2.5f * Time.deltaTime);
    }
    
    public float GetTargetAngle()
    {
        if (Target == null)
        {
            return 0;
        }

        Vector3 Direction = new Vector3(Target.transform.position.x, 0, Target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        float angle = Vector3.Angle(transform.forward, Direction);
        float RotationDifference = transform.localEulerAngles.x;
        RotationDifference = (RotationDifference > 180) ? RotationDifference - 360 : RotationDifference;
        float AdjustedAngle = Mathf.Abs(angle) - Mathf.Abs(RotationDifference);
        return AdjustedAngle;
    }
    
    public float GetDestinationAngle(Vector3 pos)
    {
        if (pos == Vector3.zero)
        {
            return 0;
        }

        Vector3 Direction = new Vector3(pos.x, 0, pos.z) - new Vector3(transform.position.x, 0, transform.position.z);
        float angle = Vector3.Angle(transform.forward, Direction);
        float RotationDifference = transform.localEulerAngles.x;
        RotationDifference = (RotationDifference > 180) ? RotationDifference - 360 : RotationDifference;
        float AdjustedAngle = Mathf.Abs(angle) - Mathf.Abs(RotationDifference);
        return AdjustedAngle;
    }

    private void WeaponState(bool enable)
    {
        WeaponStateEvent.Invoke(enable, false);
    }
    public void ApproachToTarget()
    {
        if(SoundSearching)
            return;
        
        if(Anim.GetCurrentAnimatorStateInfo(0).IsTag("Stop"))
            return;
       
        
        if (!DoubleIt)
        {
            DoubleIt = true;
            
            if (Detection.AlertSettings.DoubleTheDetectionDistance == UniversalAIEnums.YesNo.Yes)
            {
                Detection.DetectionSettings.DetectionDistance *= 1.5f;
            }
            
            if (Detection.AlertSettings.DoubleTheDetectionAngle == UniversalAIEnums.YesNo.Yes)
            {
                Detection.DetectionSettings.DetectionAngle *= 1.5f;
            }
        }

        if (!Approaching && !UseIK && InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes)
        {
            if (!Anim.GetBool("Equipped") || Anim.GetCurrentAnimatorStateInfo(0).IsName("UnEquip"))
            {
                
            }
            else
            {
                Invoke("EnableIK", 0.62f);   
            }
        }

        if (Target == null)
        {
            StopAttack();
            return;
        }
          
        if(IsBlocking)
            return;


        Approaching = true;
        if(TargetDistance > Settings.Attack.AttackDistance + 2f && Attacking)
        {
            StopAttack();
        }

      
        if (TargetDistance > Settings.Attack.AttackDistance + 2f && Attacking || TargetDistance > Settings.Attack.AttackDistance && !Attacking)
        {
            if (Settings.Movement.ChaseTarget == UniversalAIEnums.YesNo.Yes)
            {
                if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
                {
                    Vector3 Direction = new Vector3(Target.transform.position.x, 0, Target.transform.position.z) -
                                        new Vector3(transform.position.x, 0, transform.position.z);
                    Vector3 DestinationDirection = Direction;

                    if ((int) GetDestinationAngle(Target.transform.position) >= Settings.Movement.TurnAngleLimit)
                    {
                        AdjustNavmesh(true);
                        Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                            Time.deltaTime * Settings.Movement.TurnSpeed);

                        Vector3 cross = Vector3.Cross(transform.forward,
                            Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                        if (cross.y >= 0)
                        {
                            Anim.SetInteger("Turn", 2);
                        }
                        else
                        {
                            Anim.SetInteger("Turn", 1);
                        }

                        return;
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 0);
                    }
                }
#if !UniversalAI_Integration_PathfindingPro
                    if (Nav.isStopped)
                    {
                        AdjustNavmesh(false);
                        return;
                    }
#else
        if (NavPro.isStopped)
        {
            AdjustNavmesh(false);
            return;
        }
#endif

                
                    if (Settings.Movement.AICanRun == UniversalAIEnums.YesNo.Yes)
                    {
                        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
                        PlayAnimation(AnimationType.Run);
                        AdjustNavmesh(false);
                    }

                    if (Settings.Movement.AICanRun == UniversalAIEnums.YesNo.No)
                    {
                        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
                        PlayAnimation(AnimationType.Walk);
                        AdjustNavmesh(false);
                    }

#if !UniversalAI_Integration_PathfindingPro
                    Nav.SetDestination(Target.transform.position);
#else
        NavPro.destination = Target.transform.position;
#endif
                }
                else
                {
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                    PlayAnimation(AnimationType.Idle);
                    AdjustNavmesh(true);
                    return;
                }
            }   
        
       

        if (Reloading)
        {
            Attacking = false;
            CanAttack = true;
            CancelInvoke("StartAttack");
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
            PlayAnimation(AnimationType.Idle);
            AdjustNavmesh(true);
            return;
        }
        
        
        if (TargetDistance <= Settings.Attack.AttackDistance)
        {
            SmoothTurn();
            if(Attacking)
                return;
           
         
            AdjustNavmesh(true);
          
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle; 
            
            if(!Anim.GetBool("Idle Active"))
              PlayAnimation(AnimationType.Idle);

            if (SingleShot)
                return;
            
            if (CanAttack && !Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                CanAttack = false;
                Invoke("StartAttack",Random.Range(Settings.Attack.MinAttackDelay, Settings.Attack.MaxAttackDelay));
            }
        }

    }

    [HideInInspector] public bool SingleShot;
    public void StartAttack()
    {
        if (TargetDistance > Settings.Attack.AttackDistance || TargetDistance == 0)
        {
         
            CanAttack = true;
            Attacking = false;
            return;
        }

        AdjustNavmesh(true);
        Attacking = true;
        PlayAnimation(AnimationType.Attack);
    }
    public void StopAttack()
    {
        CanFire = false;
        Anim.SetBool("Attack",false);
        CanAttack = true;
        Attacking = false;
        TriggerHitboxAttacks.Invoke(false);
    }
    public void UniversalAIAttack()
    {
        
        if (IsWeapon)
        {
            CanFire = false;
            Anim.SetBool("Attack",false);
            CanAttack = true;
            Attacking = false;
            return;
        }
        
        StopAttack();

      
        if(Target == null)
            return;
        
        if(GetSqrMagnitude(Target.transform.position, transform.position) > (Settings.Attack.AttackDistance + 2f * Settings.Attack.AttackDistance + 2f))
            return;

     
        if (CurrentPlayerRef != null)
        {
            CurrentPlayerRef.TakeDamage(CurrentDamage, AttackerType.AI, gameObject);
            UniversalAIEvents.OnDealDamage.Invoke(CurrentDamage);
            UniversalAISounds.PlaySound(AudioType.Damaged);
        }
        
        else if (CurrentAITarget != null)
        {
            if(AITargetDead())
                return;

            CurrentAITarget.TakeDamage(CurrentDamage, AttackerType.AI, gameObject);
            UniversalAIEvents.OnDealDamage.Invoke(CurrentDamage);
            UniversalAISounds.PlaySound(AudioType.Damaged);
            AITargetDead();
        }
    }
    bool AITargetDead()
    {
        if (Target == null)
            return true;
        
        if (CurrentAITarget != null)
        {
            if (CurrentAITarget.Health <= 0)
            {
                Target = null;
                if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
                    InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && IsShooter && BodyWeight >0)
                {
                    StartCoroutine(FadeOutBodyIK());
                }
                
                Debug.CurrentTargetDebug = Target;
                LastKnownPlayerPos = Vector3.zero;
                CurrentAITarget = null;
                Approaching = false;
                if(UseBlockSystem)
                    Anim.ResetTrigger("Block");
                Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
#if !UniversalAI_Integration_PathfindingPro
                Nav.ResetPath();
#else
                NavPro.SetPath(null);
#endif
                UniversalAIEvents.OnTargetLost.Invoke();
                UniversalAISounds.PlaySound(AudioType.LostTarget);
                StopAttack();
                return true;
            }
        }
        else if(CurrentPlayerRef == null && Target.GetComponent<UniversalAISystem>() != null)
        {
            CurrentAITarget = Target.GetComponent<UniversalAISystem>();
        }
        
        if (CurrentPlayerRef != null)
        {
            if (CurrentPlayerRef.CurrentHealth <= 0)
            {
                UnityEngine.Debug.Log("1s");
                Target = null;
                if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
                    InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && IsShooter && BodyWeight >0)
                {
                    StartCoroutine(FadeOutBodyIK());
                }
                Debug.CurrentTargetDebug = Target;
                LastKnownPlayerPos = Vector3.zero;
                Approaching = false;
                CurrentPlayerRef = null;
                Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
#if !UniversalAI_Integration_PathfindingPro
                Nav.ResetPath();
#else
               NavPro.SetPath(null);
#endif
                
                UniversalAIEvents.OnTargetLost.Invoke();
                UniversalAISounds.PlaySound(AudioType.LostTarget);
                StopAttack();
                return true;
            }
        }
        else if(CurrentAITarget == null && Target.GetComponent<UniversalAIPlayerReference>() != null)
        {
            CurrentPlayerRef = Target.GetComponent<UniversalAIPlayerReference>();
        }
        
        return false;
    }
    public void Wander()
    {
        if (General.wanderType == UniversalAIEnums.WanderType.Dynamic)
        {
            DynamicWander();
        }

        if (General.wanderType == UniversalAIEnums.WanderType.Waypoint)
        {
            WaypointWander();
        }

        if (General.wanderType == UniversalAIEnums.WanderType.Stationary)
        {

#if !UniversalAI_Integration_PathfindingPro
            if (Nav.updatePosition)
            {
                AdjustNavmesh(true);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
            }
#else
            if (NavPro.updatePosition)
            {
                AdjustNavmesh(true);
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                PlayAnimation(AnimationType.Idle);
            }
#endif
            
        }
    }

    public void StopWaiting()
    {
        Waiting = false;
    }
    void WaypointWander()
    {
        if(Waiting)
            return;

        if (WorkingPath)
        {
#if !UniversalAI_Integration_PathfindingPro
            if (Nav.pathPending)
                return;
            
            if (Nav.remainingDistance <= Nav.stoppingDistance + 0.1f)
            {
                if (Waypoints[CurrentWaypoint].WaitOnArrival)
                {
                    Waiting = true;
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                    PlayAnimation(AnimationType.Idle);
                    AdjustNavmesh(true);
                    
                    Invoke("StopWaiting", Random.Range(Waypoints[CurrentWaypoint].MinRandomWaitLength, Waypoints[CurrentWaypoint].MaxRandomWaitLength + 1));
                    WorkingPath = false;
                }
                else
                {
                    Waiting = false;
                    WorkingPath = false;
                }
            }
            else
            {
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
                PlayAnimation(AnimationType.Walk);
                AdjustNavmesh(false);
            }
#else
            if (NavPro.pathPending)
                return;
            
            if (NavPro.remainingDistance <= NavPro.endReachedDistance)
            {
                if (Waypoints[CurrentWaypoint].WaitOnArrival)
                {
                    Waiting = true;
                    Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Idle;
                    PlayAnimation(AnimationType.Idle);
                    AdjustNavmesh(true);
                    
                    Invoke("StopWaiting", Random.Range(Waypoints[CurrentWaypoint].MinRandomWaitLength, Waypoints[CurrentWaypoint].MaxRandomWaitLength + 1));
                    WorkingPath = false;
                }
                else
                {
                    Waiting = false;
                    WorkingPath = false;
                }
            }
            else
            {
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
                PlayAnimation(AnimationType.Walk);
                AdjustNavmesh(false);
            }
#endif

            return;
        }

        if (UniversalAIWaypoints.WaypointType == UniversalAIEnums.WaypointType.InOrder)
        {
            CurrentWaypoint++;

            if (CurrentWaypoint > Waypoints.Length - 1)
            {
                CurrentWaypoint = 0;
            }

            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
            {
                Vector3 Direction =
                    new Vector3(Waypoints[CurrentWaypoint].WaypointPosition.x, 0,
                        Waypoints[CurrentWaypoint].WaypointPosition.z) -
                    new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 DestinationDirection = Direction;

                if ((int) GetDestinationAngle(Waypoints[CurrentWaypoint].WaypointPosition) >=
                    Settings.Movement.TurnAngleLimit)
                {
                    AdjustNavmesh(true);
                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                        Time.deltaTime * Settings.Movement.TurnSpeed);

                    Vector3 cross = Vector3.Cross(transform.forward,
                        Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                    if (cross.y >= 0)
                    {
                        Anim.SetInteger("Turn", 2);
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 1);
                    }

                    return;
                }
                else
                {
                    Anim.SetInteger("Turn", 0);
                }
            }

#if !UniversalAI_Integration_PathfindingPro
            Nav.SetDestination(Waypoints[CurrentWaypoint].WaypointPosition);
#else
            NavPro.destination = Waypoints[CurrentWaypoint].WaypointPosition;
            NavPro.SearchPath();
#endif
            Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
            PlayAnimation(AnimationType.Walk);
            AdjustNavmesh(false);
            WorkingPath = true;
        }
        
        if (UniversalAIWaypoints.WaypointType == UniversalAIEnums.WaypointType.Random)
        {
            oldCurrentWaypoint = CurrentWaypoint;

            while (CurrentWaypoint == oldCurrentWaypoint)
            {
                CurrentWaypoint = Random.Range(0, Waypoints.Length);
            }

            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
            {
                Vector3 Direction =
                    new Vector3(Waypoints[CurrentWaypoint].WaypointPosition.x, 0,
                        Waypoints[CurrentWaypoint].WaypointPosition.z) -
                    new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 DestinationDirection = Direction;

                if ((int) GetDestinationAngle(Waypoints[CurrentWaypoint].WaypointPosition) >=
                    Settings.Movement.TurnAngleLimit)
                {
                    AdjustNavmesh(true);
                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                        Time.deltaTime * Settings.Movement.TurnSpeed);

                    Vector3 cross = Vector3.Cross(transform.forward,
                        Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                    if (cross.y >= 0)
                    {
                        Anim.SetInteger("Turn", 2);
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 1);
                    }

                    return;
                }
                else
                {
                    Anim.SetInteger("Turn", 0);
                }
            }
#if !UniversalAI_Integration_PathfindingPro
            Nav.SetDestination(Waypoints[CurrentWaypoint].WaypointPosition);
#else
            NavPro.destination = Waypoints[CurrentWaypoint].WaypointPosition;
            NavPro.SearchPath();
#endif

            if (GetSqrMagnitude(transform.position, Waypoints[CurrentWaypoint].WaypointPosition) >=
                (UniversalAIWaypoints.StartRunningDistance * UniversalAIWaypoints.StartRunningDistance))
            {
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Run;
                PlayAnimation(AnimationType.Run);
            }
            else
            {
                Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Walk;
                PlayAnimation(AnimationType.Walk);
            }
            AdjustNavmesh(false);
            WorkingPath = true;
        }
    }

    private bool WorkingPath;
    private int RandomWander;
    void DynamicWander()
        {
#if !UniversalAI_Integration_PathfindingPro
        if (Nav.hasPath)
            {
                if (GetSqrMagnitude(transform.position,Nav.destination) > (Nav.stoppingDistance + 0.2f * Nav.stoppingDistance + 0.2f))
                {
                    return;
                }
            }
#else
            if (NavPro.hasPath)
            {
                if (NavPro.remainingDistance > NavPro.endReachedDistance)
                {
                    return;
                }
            }
#endif
            
            if(InIdle && Debug.AITurning == UniversalAIEnums.YesNo.No) return;


            if(Debug.AITurning == UniversalAIEnums.YesNo.No)
                RandomWander = Random.Range(1, 4);
            
            Vector3 pos = Vector3.zero;

            if (RandomWander != 3 && !InIdle)
            {
                pos = GetRandomLocation(General.DynamicWanderRadius);
            
                if(pos.Equals(Vector3.zero))
                    return;
            }
            
            
            if (RandomWander == 1)
            {
                idlecount = 0;

                if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
                {
                    Vector3 Direction = new Vector3(pos.x, 0, pos.z) -
                                        new Vector3(transform.position.x, 0, transform.position.z);
                    Vector3 DestinationDirection = Direction;

                    if ((int) GetDestinationAngle(pos) >= Settings.Movement.TurnAngleLimit)
                    {
                        AdjustNavmesh(true);
                        Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                            Time.deltaTime * Settings.Movement.TurnSpeed);

                        Vector3 cross = Vector3.Cross(transform.forward,
                            Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                        if (cross.y >= 0)
                        {
                            Anim.SetInteger("Turn", 2);
                        }
                        else
                        {
                            Anim.SetInteger("Turn", 1);
                        }

                        return;
                    }
                    else
                    {
                        Anim.SetInteger("Turn", 0);
                    }
                }

#if !UniversalAI_Integration_PathfindingPro
                    Nav.SetDestination(pos);
#else
                    NavPro.destination = pos;
#endif

                    WanderDestination = pos;
                   
                    PlayAnimation(AnimationType.Walk);
                    Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),1,Time.deltaTime * 5.2f));
            }
            if (RandomWander == 2)
            {
                if (Settings.Movement.AICanRun == UniversalAIEnums.YesNo.Yes && Random.Range(1, 3) == 1)
                {
                    idlecount = 0;

                    if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
                    {
                        Vector3 Direction = new Vector3(pos.x, 0, pos.z) -
                                            new Vector3(transform.position.x, 0, transform.position.z);
                        Vector3 DestinationDirection = Direction;
                        if ((int) GetDestinationAngle(pos) >= Settings.Movement.TurnAngleLimit)
                        {
                            AdjustNavmesh(true);
                            Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                                Time.deltaTime * Settings.Movement.TurnSpeed);

                            Vector3 cross = Vector3.Cross(transform.forward,
                                Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                            if (cross.y >= 0)
                            {
                                Anim.SetInteger("Turn", 2);
                            }
                            else
                            {
                                Anim.SetInteger("Turn", 1);
                            }

                            return;
                        }
                        else
                        {
                            Anim.SetInteger("Turn", 0);
                        }
                    }
#if !UniversalAI_Integration_PathfindingPro
                    Nav.SetDestination(pos);
#else
                        NavPro.destination = pos;
#endif
                        WanderDestination = pos;
                        
                        PlayAnimation(AnimationType.Run);
                        Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),2,Time.deltaTime * 5.2f));
                    }
                    else
                    {
                        if (Random.Range(1, 3) == 1)
                        {
                            PlayAnimation(AnimationType.Idle);
                            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),0,Time.deltaTime * 5.2f));
                            InIdle = true;
                            StartCoroutine(IdleWander());
                        }
                        else
                        {
                            idlecount = 0;

                            if (Settings.Movement.UseTurnSystem == UniversalAIEnums.YesNo.Yes)
                            {
                                Vector3 Direction = new Vector3(pos.x, 0, pos.z) -
                                                    new Vector3(transform.position.x, 0, transform.position.z);
                                Vector3 DestinationDirection = Direction;

                                if ((int) GetDestinationAngle(pos) >= Settings.Movement.TurnAngleLimit)
                                {
                                    AdjustNavmesh(true);
                                    Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                                    transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget,
                                        Time.deltaTime * Settings.Movement.TurnSpeed);

                                    Vector3 cross = Vector3.Cross(transform.forward,
                                        Quaternion.LookRotation(DestinationDirection, Vector3.up) * Vector3.forward);

                                    if (cross.y >= 0)
                                    {
                                        Anim.SetInteger("Turn", 2);
                                    }
                                    else
                                    {
                                        Anim.SetInteger("Turn", 1);
                                    }

                                    return;
                                }
                                else
                                {
                                    Anim.SetInteger("Turn", 0);
                                }
                            }

#if !UniversalAI_Integration_PathfindingPro
                    Nav.SetDestination(pos);
#else
                            NavPro.destination = pos;
#endif
                            
                            WanderDestination = pos;
                            
                            PlayAnimation(AnimationType.Walk);
                            Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),1,Time.deltaTime * 5.2f));
                        }
                       
                    }
                   
            }
            if (RandomWander == 3)
            {
                InIdle = true;
                PlayAnimation(AnimationType.Idle);
                Anim.SetFloat("Speed",Mathf.Lerp(Anim.GetFloat("Speed"),0,Time.deltaTime * 5.2f));
                StartCoroutine(IdleWander());
            }
        }
    IEnumerator IdleWander()
        {
            float random = Random.Range(General.MinIdleLength, General.MaxIdleLength);

            idlecount++;
            
            if (idlecount < 3)
                yield return new WaitForSeconds(random);
         
            InIdle = false;
        }
#if UniversalAI_Integration_Puppetmaster

    private IEnumerator FadeOutPinWeight() {
        while (_puppetMaster.pinWeight > 0f) {
            _puppetMaster.pinWeight = Mathf.MoveTowards(_puppetMaster.pinWeight, 0f, Time.deltaTime * 5);
            yield return null;
        }
    }

    // Fading out puppetMaster.muscleWeight to deadMuscleWeight
    private IEnumerator FadeOutMuscleWeight() {
        while (_puppetMaster.muscleWeight > 0f) {
            _puppetMaster.muscleWeight = Mathf.MoveTowards(_puppetMaster.muscleWeight, 0.2f, Time.deltaTime * 5);
            yield return null;
        }
    }
    
#endif

    [HideInInspector] public GameObject muzzle;
    public void Die()
    {
        if(Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Death)
            return;
        
        Debug.AICurrentStateDebug = UniversalAIEnums.AICurrentState.Death;
            
        Health = 0;
        if (Frozen)
        {
            FreezeAIManuel(false);
        }

        if (UniversalAIHealthBar != null)
        {
            UniversalAIHealthBar.gameObject.SetActive(false);
        }
        
        CanFire = false;
           
            
        SetDebugs();
            
        AdjustNavmesh(true);
        
#if !UniversalAI_Integration_PathfindingPro
        Nav.enabled = false;
#else
        NavPro.enabled = false;
#endif
        
        if (MainCollider != null)
            MainCollider.enabled = false;
        
        if (General.DeathType == UniversalAIEnums.DeathMethod.Animation)
        {
            PlayAnimation(AnimationType.Death);
        }
        else
        {
            Anim.enabled = false;
                
            Collider[] colliders = GetComponentsInChildren<Collider>();
            Rigidbody[] rigidbodys = GetComponentsInChildren<Rigidbody>();

            foreach (var rig in rigidbodys)
            {
                rig.isKinematic = false;
            }
            foreach (var col in colliders)
            {
                if (MainCollider == null)
                {
                    col.enabled = true;
                        continue;
                }
                
                if (col != MainCollider)
                {
                    col.enabled = true;
                }
            }
        }
        
        if (General.DestroyAIOnDeath == UniversalAIEnums.YesNo.Yes)
        {
            Destroy(gameObject, General.DestroyDelay);
        }
        
        UniversalAIEvents.OnDeath.Invoke();
        this.enabled = false;
    }

    //WIP
    
    // public void CallForBackup()
    // {
    //     if(Target == null)
    //         return;
    //     
    //     Detection.AlertSettings.CallingBackup = UniversalAIEnums.YesNo.Yes;
    //
    //     Collider[] PossibleAI = Physics.OverlapSphere(transform.position,
    //         Detection.AlertSettings.CallForBackupRadius, Detection.AlertSettings.CallForBackupLayers);
    //
    //     foreach (var target in PossibleAI)
    //     {
    //         if(target.gameObject.Equals(Target) || target.gameObject.Equals(gameObject))
    //             continue;
    //         
    //         UniversalAISystem system = target.GetComponent<UniversalAISystem>();
    //
    //         if (system == null && target.transform.root.gameObject != target.gameObject)
    //         {
    //             system = target.transform.root.GetComponent<UniversalAISystem>();
    //         }
    //         
    //         if(system == null)
    //             continue;
    //
    //         if(Target == null)
    //             break;
    //
    //         if (system.Target == null)
    //         {
    //             if(system.Equals(this) && system.gameObject != Target)
    //                 return;
    //             
    //             if (system.Detection.Factions.Factions == Detection.Factions.Factions)
    //             {
    //                 system.UniversalAICommandManager.SetTarget(Target);
    //             }   
    //         }
    //     }
    // }
   
    public void Detect()
    {
   
#if UniversalAI_Integration_Puppetmaster

            if (_behaviourPuppet == null)
            {
                BehaviourPuppet[] puppets = _puppetMaster.transform.root
                    .GetComponentsInChildren<BehaviourPuppet>();
                
                if(puppets.Length != 1)
                    return;
                
                if (puppets[0] != null)
                {
                    _behaviourPuppet = puppets[0];
                }
            }
            else if (_behaviourPuppet.state != BehaviourPuppet.State.Puppet)
            {
                return;
            }
           
            
#endif

        if(Health <= 0)
            return;
        
        Collider[] PossibleTargets = Physics.OverlapSphere(transform.position,
            Detection.DetectionSettings.DetectionDistance, Detection.DetectionSettings.DetectionLayers);

        
        if (Target != null && !DamageSearching && !SearchSettedTarget && !SoundSearching)
        {
      
            bool FoundTarget = false;
            
            foreach (var tar in PossibleTargets)
            {
                if (tar.gameObject.Equals(Target))
                {
                    RaycastHit Hit;
               
                    if (Physics.Linecast(transform.position + new Vector3(0,1,0), Target.transform.position + new Vector3(0,1,0), out Hit,
                            Detection.DetectionSettings.ObstacleLayers, QueryTriggerInteraction.Ignore))
                    {
                        continue;
                    }

                    FoundTarget = true;
                }
            }

            if (!FoundTarget)
            {
                if(LastKnownPlayerPos == Vector3.zero && Target != null)
                    LastKnownPlayerPos = Target.transform.position;
                              
               
                Target = null;
                if (InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes &&
                    InverseKinematics.UseAimIK == UniversalAIEnums.YesNo.Yes && IsShooter && BodyWeight >0)
                {
                    StartCoroutine(FadeOutBodyIK());
                }
            }
        }
        if ((DamageSearching || SearchSettedTarget) && Target != null)
        {
            foreach (var tar in PossibleTargets)
            {
                if (tar.gameObject.Equals(Target))
                {
                    DamageSearching = false;
                    SearchSettedTarget = false;
                    break;
                }
            }
        }
       
       
        
        VisibleTargets.Clear();

        foreach (var PosT in PossibleTargets)
        {
            GameObject ToDetect = null;

            if (TagStorage.AvailableAITags.Contains(PosT.transform.root.gameObject.tag) || PosT.transform.root.gameObject.tag.Equals(PlayerTag))
            {
                if (PosT.transform.root.GetComponent<UniversalAISystem>() != null)
                {
                    ToDetect = PosT.transform.root.gameObject;
                }
                else if (PosT.transform.root.GetComponent<UniversalAIPlayerReference>() != null)
                {
                    ToDetect = PosT.transform.root.gameObject;
                }
                else
                {
                    ToDetect = PosT.gameObject;
                }
            }
            else
            {
                ToDetect = PosT.gameObject;
            }
            
            if (ToDetect != gameObject && !VisibleTargets.Contains(ToDetect))
            {
                if (ToDetect.tag.Equals(PlayerTag))
                {
                    UniversalAIPlayerReference playerReference = PosT.GetComponent<UniversalAIPlayerReference>();
                    
                    if (playerReference != null)
                    {
                        if (playerReference.CurrentHealth <= 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        UniversalAIPlayerReference playerReferenceAdded = ToDetect.AddComponent<UniversalAIPlayerReference>();

                        if (playerReferenceAdded.CurrentHealth <= 0)
                        {
                            continue;
                        }
                    }
                }
                else if (TagStorage.AvailableAITags.Contains(ToDetect.tag))
                {
                    UniversalAISystem system = PosT.GetComponent<UniversalAISystem>();
                    if (system != null)
                    {
                        if (system.Debug.AICurrentStateDebug == UniversalAIEnums.AICurrentState.Death)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
               
                
                RaycastHit Hit;

                if (Physics.Linecast(transform.position + new Vector3(0,1,-1), PosT.transform.position + new Vector3(0,1,0), out Hit,
                        Detection.DetectionSettings.ObstacleLayers, QueryTriggerInteraction.Ignore))
                {
                    if(Hit.transform.gameObject != gameObject)
                        continue;
                }
                
                
                if (Detection.DetectionSettings.DetectionType == UniversalAIEnums.DetectionType.LineOfSight)
                {
                    Vector3 direction = (new Vector3(PosT.transform.position.x, PosT.transform.position.y + PosT.transform.localScale.y / 2, PosT.transform.position.z)) - Detection.DetectionSettings.HeadTransform.position;
                    float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), transform.forward);
                    if (angle <= Detection.DetectionSettings.DetectionAngle / 2f)
                    {
                        VisibleTargets.Add(ToDetect);
                    }
                }
                else if(!Detection.DetectionSettings.IgnoredObjects.Contains(ToDetect))
                { 
                    VisibleTargets.Add(ToDetect);   
                }
            }
        }

        Debug.VisibleTargetsDebug = VisibleTargets;
        
        if (VisibleTargets.Contains(Target) && SoundSearching)
        {
            SoundSearching = false;
          
        }
          
        if(Target != null)
            return;
        
        
        if (VisibleTargets != null && General.AIConfidence != UniversalAIEnums.AIConfidence.Neutral)
        {
            if(VisibleTargets.Count > 0)
            {
                UniversalAISystem AISystem = null;
                UniversalAIPlayerReference PlayerRef = null;
                
                if (TagStorage.AvailableAITags.Contains(VisibleTargets[0].gameObject.tag))
                {
                    AISystem = VisibleTargets[0].gameObject.GetComponent<UniversalAISystem>();
                }

                if (VisibleTargets[0].gameObject.tag.Equals(PlayerTag))
                {
                    PlayerRef = VisibleTargets[0].gameObject.GetComponent<UniversalAIPlayerReference>();
                }

                if (AISystem != null)
                {
                    if (Detection.DetectionSettings.DetectObjects.Contains(VisibleTargets[0].gameObject))
                    {
                        Target = VisibleTargets[0].gameObject;
                        CurrentAITarget = AISystem;
                        CurrentPlayerRef = null;
                        LastKnownPlayerPos = Vector3.zero;
                        UniversalAIEvents.OnTargetVisible.Invoke();
                    }
                    else if (Detection.Factions.FactionGroups != null)
                    {
                        foreach (var Fact in Detection.Factions.FactionGroups) 
                        {
                            if (Fact.Faction == AISystem.Detection.Factions.Factions)
                            {
                                if (Fact.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                                { 
                                    Target = VisibleTargets[0].gameObject;
                                    CurrentAITarget = AISystem;
                                    CurrentPlayerRef = null;
                                    LastKnownPlayerPos = Vector3.zero;
                                    UniversalAIEvents.OnTargetVisible.Invoke();
                                }
                                else
                                {
                                    for (int i = 0; i < VisibleTargets.Count; i++)
                                    {
                                        UniversalAISystem AISystems = null;
                                        UniversalAIPlayerReference PlayerRefs = null;
                
                                        if (TagStorage.AvailableAITags.Contains(VisibleTargets[i].gameObject.tag))
                                        {
                                            AISystems = VisibleTargets[i].gameObject.GetComponent<UniversalAISystem>();
                                        }

                                        if (VisibleTargets[i].gameObject.tag.Equals(PlayerTag))
                                        {
                                            PlayerRefs = VisibleTargets[i].gameObject.GetComponent<UniversalAIPlayerReference>();
                                        }

                                        if (AISystems != null)
                                        {
                                            if (Detection.Factions.FactionGroups != null)
                                            {
                                                foreach (var Facts in Detection.Factions.FactionGroups) 
                                                {
                                                    if (Facts.Faction == AISystems.Detection.Factions.Factions)
                                                    {
                                                        if (Facts.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                                                        { 
                                                            Target = VisibleTargets[i].gameObject;
                                                            CurrentAITarget = AISystems;
                                                            CurrentPlayerRef = null;
                                                            LastKnownPlayerPos = Vector3.zero;
                                                            UniversalAIEvents.OnTargetVisible.Invoke();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (PlayerRefs != null && PlayerRef.CurrentHealth > 0)
                                        {
                                            if (Detection.Factions.CanDetectPlayer == UniversalAIEnums.YesNo.Yes)
                                            {
                                                if (Detection.Factions.FactionGroups != null)
                                                {
                                                    foreach (var Facts in Detection.Factions.FactionGroups) 
                                                    {
                                                        if (Facts.Faction == PlayerRefs.PlayerFaction)
                                                        {
                                                            if (Facts.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                                                            { 
                                                                Target = VisibleTargets[i].gameObject;
                                                                CurrentAITarget = null;
                                                                CurrentPlayerRef = PlayerRef;
                                                                LastKnownPlayerPos = Vector3.zero;
                                                                UniversalAIEvents.OnTargetVisible.Invoke();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (PlayerRef != null && PlayerRef.CurrentHealth > 0)
                {
                    if (Detection.Factions.CanDetectPlayer == UniversalAIEnums.YesNo.Yes)
                    {
                        if (Detection.Factions.FactionGroups != null)
                        {
                            foreach (var Facts in Detection.Factions.FactionGroups) 
                            {
                                if (Facts.Faction == PlayerRef.PlayerFaction)
                                {
                                    if (Facts.ApproachType == UniversalAIEnums.ApproachType.Enemy)
                                    { 
                                        Target = VisibleTargets[0].gameObject;
                                        CurrentAITarget = null;
                                        CurrentPlayerRef = PlayerRef;
                                        LastKnownPlayerPos = Vector3.zero;
                                        UniversalAIEvents.OnTargetVisible.Invoke();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }

    }
    
    #endregion
    
    #region Shooter Functions

    public void StartFire()
    {
        CanFire = true;
    }
    public void Reload()
    {
        StopAttack();
        Reloading = true;
        CanFire = false;

        BodyWeight = 0;
        elapsedTime = 0;
        UseIK = false;

        PlayAnimation(AnimationType.Reload);
        Anim.SetBool("Idle Active", true);
        
        AdjustNavmesh(true);

        Attacking = false;
        Approaching = false;
        LastKnownPlayerPos = Vector3.zero;
        CanAttack = true;
        
        InvokeRepeating("ReloadCheck",0.2f,0.1f);
    }

    [HideInInspector] public UnityEvent PrivateReloadEnd = new UnityEvent();
    private bool ReloadStarted;
    public void ReloadCheck()
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsTag("Reload"))
            ReloadStarted = true;
        
        Anim.SetFloat("Speed", 0);
        if (ReloadStarted)
        {
            if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("Reload"))
            {
                ReloadStarted = false;
                Reloading = false;
                CanFire = true;
                
                if(InverseKinematics.UseInverseKinematics == UniversalAIEnums.YesNo.Yes)
                    EnableIK();

                CancelInvoke("ReloadCheck");
                PrivateReloadEnd.Invoke();

                if (Target == null)
                {
                    StopAttack();
                    LastKnownPlayerPos = Vector3.zero;
                    Approaching = false;
                    Invoke("StopAlertMode",Random.Range(Detection.AlertSettings.MinStayAlertLength, Detection.AlertSettings.MaxStayAlertLength + 1));
#if !UniversalAI_Integration_PathfindingPro
                Nav.ResetPath();
#else
                    NavPro.SetPath(null);
#endif
                    UniversalAIEvents.OnTargetLost.Invoke();
                    UniversalAISounds.PlaySound(AudioType.LostTarget);
                }
            }
           
        }
        
    }

    #endregion
    
}
    
}