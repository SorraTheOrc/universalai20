//Darking Assets

using System;

using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace UniversalAI
{

    public class UniversalAIAnimatorCreator : MonoBehaviour
    {
#if UNITY_EDITOR
    #region Private Variables

    [HideInInspector] public bool Open;

    [HideInInspector] public bool IsCombat;

    [HideInInspector] public bool IsMovementCombat;

    [HideInInspector] public bool IsMovement;

    [HideInInspector] public int CurrentTab;

    #endregion

    public void OnValidate()
    {
        if (Application.isPlaying)
            return;
        
        if (Combat == null)
        {
            Combat = new combat();
        }
        
        if (Movement == null)
        {
            Movement = new movement();
        }
        
        if (CombatMovement == null)
        {
            CombatMovement = new combatMovement();
        }
            
        if (Movement != null && Movement.Idles != null)
        {
            foreach (var List1 in Movement.Idles)
            {
                if (List1.AnimationSpeed <= 0)
                {
                    List1.AnimationSpeed = 1;
                }
            }
        }

        if (CombatMovement != null && CombatMovement.CombatIdles != null)
        {
            foreach (var List1 in CombatMovement.CombatIdles)
            {
                if (List1.AnimationSpeed <= 0)
                {
                    List1.AnimationSpeed = 1;
                }
            }
        }

        if (Combat != null)
        {
            if (Combat.HitReactionAnimations != null)
            {
                foreach (var List1 in Combat.HitReactionAnimations)
                {
                    if (List1.AnimationSpeed <= 0)
                    {
                        List1.AnimationSpeed = 1;
                    }
                }
            }

            if (Combat.BlockAnimations != null)
            {
                foreach (var List1 in Combat.BlockAnimations)
                {
                    if (List1.AnimationSpeed <= 0)
                    {
                        List1.AnimationSpeed = 1;
                    }
                }
            }

            if (Combat.AttackAnimations != null)
            {
                foreach (var List1 in Combat.AttackAnimations)
                {
                    if (List1.AnimationSpeed <= 0)
                    {
                        List1.AnimationSpeed = 1;
                    }

                    if (List1.MinDamageAmount <= 0)
                    {
                        List1.MinDamageAmount = 15;
                    }

                    if (List1.MaxDamageAmount <= 0)
                    {
                        List1.MaxDamageAmount = 30;
                    }
                }
            }

            if (Combat.DeathAnimations != null)
            {
                foreach (var List1 in Combat.DeathAnimations)
                {
                    if (List1.AnimationSpeed <= 0)
                    {
                        List1.AnimationSpeed = 1;
                    }
                }
            }
        }

        

        switch (CurrentTab)
        {
            case 0:
                IsCombat = false;
                IsMovementCombat = false;
                IsMovement = true;
                break;
            case 1:
                IsCombat = false;
                IsMovementCombat = true;
                IsMovement = false;
                break;
            case 2:
                IsCombat = true;
                IsMovementCombat = false;
                IsMovement = false;
                break;
        }

        if (!Open)
        {
            IsCombat = false;
            IsMovementCombat = false;
            IsMovement = false;
        }

        if (!Combat.IsWeapon)
        {
            Combat.UseEquipAnimations = false;
            Combat.IsShooterWeapon = false;
        }

    }
    
    
    [Space] 
    [Condition("IsMovement", true, 0f)]
    public movement Movement;

    [Serializable]
    public class movement
    {
        [Space] [Space]
        [Space]
   
        [Header("MOVEMENT")] [Reorderable] public AnimationClipOverrideList Idles;

        [Header("WALK")]
        [Space] public AnimationClip WalkForward;
        //[Space] [Space] [Help("The Walking Forwards Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float ForwardSpeed = 1f;
        public bool MirrorForwardAnimation; [Space]
        
        [Space] public AnimationClip WalkRight;
        //[Space] [Space] [Help("The Walking Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RightSpeed = 1f;
        public bool MirrorRightAnimation; [Space]

        [Space] public AnimationClip WalkLeft;
        //[Space] [Space] [Help("The Walking Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float LeftSpeed = 1f;
        public bool MirrorLeftAnimation; [Space]
        
        [Space] public AnimationClip WalkBackwards;
        //[Space] [Space] [Help("The Walking Backwards Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float BackwardsSpeed = 1f;
        public bool MirrorBackwardsAnimation; [Space]
        
        [Space]
        
        [Header("RUN")]
        [Space] public AnimationClip RunForward;
        //[Space] [Space] [Help("The Running Forwards Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunForwardSpeed = 1f;
        public bool MirrorRunForwardAnimation; [Space]

        [Space] public AnimationClip RunRight;
        //[Space] [Space] [Help("The Running Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunRightSpeed = 1f;
        public bool MirrorRunRightAnimation; [Space]

        [Space] public AnimationClip RunLeft;
        //[Space] [Space] [Help("The Running Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunLeftSpeed = 1f;
        public bool MirrorRunLeftAnimation; 
        [Space] 
        
        [Space]
        [Header("TURN")]
        public bool UseTurnAnimations = true;
        [Space]
        
        [Condition("UseTurnAnimations", true, 0f)]
        [Space] public AnimationClip TurnLeft;
        //[Space] [Space] [Help("The Combat Turn Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        [Condition("UseTurnAnimations", true, 0f)]
        public float TurnLeftSpeed = 1f;
        [Condition("UseTurnAnimations", true, 0f)]
        public bool MirrorTurnLeft; [Space]

        [Condition("UseTurnAnimations", true, 0f)]
        [Space] public AnimationClip TurnRight;
        //[Space] [Space] [Help("The Combat Turn Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        [Condition("UseTurnAnimations", true, 0f)]
        public float TurnRightSpeed = 1f;
        [Condition("UseTurnAnimations", true, 0f)]
        public bool MirrorTurnRight;
        
    }

    [Condition("IsMovementCombat", true, 0f)]
    public combatMovement CombatMovement;

    [Serializable]
    public class combatMovement
    {
        [Space]
        [Space] 
        [Space]
        
        [Header("COMBAT MOVEMENT")] [Reorderable]
        public AnimationClipOverrideList CombatIdles;

        [Header("WALK")]
        [Space] public AnimationClip CombatWalkForward;
        //[Space] [Space] [Help("The Combat Walk Forward Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float WalkForwardSpeed = 1f;
        public bool MirrorCombatForward; [Space]
       

        [Space] public AnimationClip CombatWalkRight;
        //[Space] [Space] [Help("The Combat Walk Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float WalkRightSpeed = 1f;
        public bool MirrorCombatRight; [Space]
        

        [Space] public AnimationClip CombatWalkLeft;
        //[Space] [Space] [Help("The Combat Walk Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float WalkLeftSpeed = 1f;
        public bool MirrorCombatLeft; [Space]
        
        [Space] public AnimationClip CombatWalkBackwards;
        //[Space] [Space] [Help("The Combat Walk Backwards Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float WalkBackwardsSpeed = 1f;
        public bool MirrorCombatBackwards;
        [Space]
      
        
        [Space]
        
        [Header("RUN")]
        [Space] public AnimationClip CombatRunForward;
        //[Space] [Space] [Help("The Combat Run Forward Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunForwardSpeed = 1f;
        public bool MirrorCombatRunForward; [Space]

        [Space] public AnimationClip CombatRunRight;
        //[Space] [Space] [Help("The Combat Run Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunRightSpeed = 1f;
        public bool MirrorRunCombatRight; [Space]

        [Space] public AnimationClip CombatRunLeft;
        //[Space] [Space] [Help("The Combat Run Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        public float RunLeftSpeed = 1f;
        public bool MirrorRunCombatLeft;
        [Space] 
        
        [Space]
        [Header("TURN")]
        public bool UseCombatTurnAnimations = true;
        [Space]
        
        [Condition("UseCombatTurnAnimations", true, 0f)]
        [Space] public AnimationClip CombatTurnLeft;
        //[Space] [Space] [Help("The Combat Turn Left Animation For The AI !",HelpBoxMessageType.Info)]
        
        [Condition("UseCombatTurnAnimations", true, 0f)]
        public float CombatTurnLeftSpeed = 1f;
        [Condition("UseCombatTurnAnimations", true, 0f)]
        public bool MirrorCombatTurnLeft; [Space]

        [Condition("UseCombatTurnAnimations", true, 0f)]
        [Space] public AnimationClip CombatTurnRight;
        //[Space] [Space] [Help("The Combat Turn Right Animation For The AI !",HelpBoxMessageType.Info)]
        
        [Condition("UseCombatTurnAnimations", true, 0f)]
        public float CombatTurnRightSpeed = 1f;
        [Condition("UseCombatTurnAnimations", true, 0f)]
        public bool MirrorCombatTurnRight;
    }

    [Condition("IsCombat", true, 0f)] public combat Combat;

    [Serializable]
    public class combat
    {
        [Space] [Space]
        [Header("COMBAT")]
        [Reorderable]
        public AnimationAttackClipOverrideList AttackAnimations;

        [Space] public bool UseHitReactions;

        [Reorderable] public AnimationClipOverrideList HitReactionAnimations;
        
        [Space] 
        
        public bool IsWeapon = false;

        //[Space] [Space] [Help("Does The AI Use A Weapon ?", HelpBoxMessageType.Info)] 
        
        [Condition("IsWeapon", true, 6f)]
        public bool UseEquipAnimations = true;
        
        [Condition("UseEquipAnimations", true, 6f)]
        public AnimationClip EquipAnimation;
        
        [Condition("UseEquipAnimations", true, 6f)]
        public float EquipAnimationSpeed = 1f;
        
        [Space]
        
        [Condition("UseEquipAnimations", true, 6f)]
        public AnimationClip UnEquipAnimation;
        
        [Condition("UseEquipAnimations", true, 6f)]
        public float UnEquipAnimationSpeed = 1f;
        
        [Space]
        [Space]
        [Space]
        
        [Condition("IsWeapon", true, 6f)]
        public bool UseBlockingSystem = false;

        [Condition("UseBlockingSystem", true, 6f)]
        [Reorderable] public AnimationClipOverrideList BlockAnimations;
        
        [Space]
        
        [Condition("IsWeapon", true, 6f)]
        public bool IsShooterWeapon = false;
        
        [Space]
        
        [Condition("IsShooterWeapon", true, 6f)]
        public AnimationClip ReloadAnimation;
        
        [Condition("IsShooterWeapon", true, 6f)]
        public float ReloadAnimationSpeed = 1f;
        
        [Space]
        [Space]
        [Help("If you aren't using 'Animation' as the death type, you can leave the 'Death Animations' empty!", HelpBoxMessageType.BigWarning)]
        [Reorderable]
        public AnimationClipOverrideList DeathAnimations;
    }


    [Serializable]
    public class AnimationClipOverrideList : ReorderableArray<AnimationClipOverride>
    {
    }

    [Serializable]
    public class AnimationClipOverride
    {
        public AnimationClip AnimationClip;
        public float AnimationSpeed = 1;

    }
    [Serializable]
    public class AnimationAttackClipOverrideList : ReorderableArray<AnimationAttackClipOverride>
    {
    }

    [Serializable]
    public class AnimationAttackClipOverride
    {
        [HideInInspector] public string label = "Attack Animation";
        public AnimationClip AnimationClip;
        public float AnimationSpeed = 1;
        
        [Space]
        [Help("Ignore The Damage Values If Your AI Uses A Weapon!",HelpBoxMessageType.BigInfo)]
        [Space]
        
        public float MinDamageAmount = 10;
        public float MaxDamageAmount = 25;
    }
    [HideInInspector] public bool StartWithError;
    [HideInInspector] public bool ErrorMovement;
    [HideInInspector] public bool ErrorCombatMovement;
    [HideInInspector] public bool ErrorCombat;
    public void Reset()
    {
        if(Combat == null || CombatMovement == null || Movement == null)
            return;
        
        CombatMovement.CombatIdles = null;
        CombatMovement.CombatWalkForward = null;
        CombatMovement.MirrorCombatForward = false;
        CombatMovement.CombatWalkRight = null;
        CombatMovement.MirrorCombatRight = false;
        CombatMovement.CombatWalkLeft = null;
        CombatMovement.MirrorCombatLeft = false;
        CombatMovement.CombatRunForward = null;
        CombatMovement.MirrorCombatRunForward = false;
        CombatMovement.CombatRunLeft = null;
        CombatMovement.MirrorRunCombatLeft = false;
        CombatMovement.CombatRunRight = null;
        CombatMovement.MirrorRunCombatRight = false;
        CombatMovement.MirrorCombatBackwards = false;
        CombatMovement.CombatWalkBackwards = null;
        CombatMovement.UseCombatTurnAnimations = false;
        CombatMovement.CombatTurnLeft = null;
        CombatMovement.CombatTurnRight = null;
        Movement.UseTurnAnimations = false;
        Movement.TurnLeft = null;
        Movement.TurnRight = null;
        Movement.Idles = null;
        Movement.ForwardSpeed = 1f;
        Movement.LeftSpeed = 1f;
        Movement.WalkForward = null;
        Movement.WalkLeft = null;
        Movement.WalkRight = null;
        Movement.RightSpeed = 1f;
        Movement.RunForward = null;
        Movement.RunLeft = null;
        Movement.RunRight = null;
        Movement.MirrorForwardAnimation = false;
        Movement.MirrorLeftAnimation = false;
        Movement.MirrorRightAnimation = false;
        Movement.RunForwardSpeed = 1f;
        Movement.RunLeftSpeed = 1f;
        Movement.RunRightSpeed = 1f;
        Movement.MirrorRunRightAnimation = false;
        Movement.MirrorRunLeftAnimation = false;
        Movement.MirrorRunForwardAnimation = false;
        Combat.AttackAnimations = null;
        Combat.DeathAnimations = null;
        Combat.HitReactionAnimations = null;
        Combat.UseHitReactions = false;
        Combat.BlockAnimations = null;
        Combat.UseBlockingSystem = false;
        Combat.EquipAnimation = null;
        Combat.UnEquipAnimation = null;
        Combat.EquipAnimationSpeed = 1f;
        Combat.UnEquipAnimationSpeed = 1f;
        Combat.ReloadAnimation = null;
        Combat.ReloadAnimationSpeed = 1f;
        Combat.IsWeapon = false;
    }
   
    public void CopyFromMovement()
    {
        CombatMovement.CombatIdles.Clear();
        foreach (var Anim in Movement.Idles)
        {
            CombatMovement.CombatIdles.Add(Anim);
        }
        CombatMovement.CombatWalkForward = Movement.WalkForward;
        CombatMovement.MirrorCombatForward = Movement.MirrorForwardAnimation;
        CombatMovement.CombatWalkRight = Movement.WalkRight;
        CombatMovement.MirrorCombatRight = Movement.MirrorRightAnimation;
        CombatMovement.CombatWalkLeft = Movement.WalkLeft;
        CombatMovement.MirrorCombatLeft = Movement.MirrorLeftAnimation;
        CombatMovement.CombatRunForward = Movement.RunForward;
        CombatMovement.MirrorCombatRunForward = Movement.MirrorRunForwardAnimation;
        CombatMovement.CombatRunLeft = Movement.RunLeft;
        CombatMovement.MirrorRunCombatLeft = Movement.MirrorRunLeftAnimation;
        CombatMovement.CombatRunRight = Movement.RunRight;
        CombatMovement.MirrorRunCombatRight = Movement.MirrorRunRightAnimation;
        CombatMovement.CombatTurnRight = Movement.TurnRight;
        CombatMovement.CombatTurnLeft = Movement.TurnLeft;
        CombatMovement.UseCombatTurnAnimations = Movement.UseTurnAnimations;
        if (Movement.WalkBackwards == null)
        {
            CombatMovement.CombatWalkBackwards = Movement.WalkForward;
            CombatMovement.MirrorCombatBackwards = true;   
        }
        else
        {
            CombatMovement.CombatWalkBackwards = Movement.WalkBackwards;
            CombatMovement.MirrorCombatBackwards = Movement.MirrorBackwardsAnimation;   
        }
    }
    
    public void Apply(RuntimeAnimatorController m_RuntimeAnimatorController, string path)
    {
        bool Failed = false;
        GetComponent<Animator>().runtimeAnimatorController = m_RuntimeAnimatorController;
        AnimatorController m_AnimatorController = m_RuntimeAnimatorController as AnimatorController;

        if (m_AnimatorController == null)
            return;

        #region Apply
        
        GetComponent<UniversalAISystem>().IsEquipping =
            Combat.IsWeapon && Combat.UseEquipAnimations && Combat.EquipAnimation != null && Combat.UnEquipAnimation != null;
        GetComponent<UniversalAISystem>().CombatIdleCount = CombatMovement.CombatIdles.Count;
        GetComponent<UniversalAISystem>().IdleCount = Movement.Idles.Count;
        GetComponent<UniversalAISystem>().AttackCount = Combat.AttackAnimations.Count;
        GetComponent<UniversalAISystem>().HitCount = Combat.HitReactionAnimations.Length;
        GetComponent<UniversalAISystem>().BlockCount = Combat.BlockAnimations.Length;
        GetComponent<UniversalAISystem>().UseBlockSystem = Combat.UseBlockingSystem && Combat.IsWeapon;
        
        if (!Combat.UseBlockingSystem)
            GetComponent<UniversalAISystem>().BlockCount = 0;
        if (!Combat.UseHitReactions)
            GetComponent<UniversalAISystem>().HitCount = 0;
        
        GetComponent<UniversalAISystem>().DeathCount = Combat.DeathAnimations.Count;
        GetComponent<UniversalAISystem>().damages.Clear();
        foreach (var atck in Combat.AttackAnimations)
        {
            GetComponent<UniversalAISystem>().damages.Add(new Vector2(atck.MinDamageAmount,atck.MaxDamageAmount));
        }

        GetComponent<UniversalAISystem>().Idles = Movement.Idles;
        GetComponent<UniversalAISystem>().CombatIdles = CombatMovement.CombatIdles;
        GetComponent<UniversalAISystem>().Attacks = Combat.AttackAnimations;
        GetComponent<UniversalAISystem>().HitReactions = Combat.HitReactionAnimations;
        GetComponent<UniversalAISystem>().Deaths = Combat.DeathAnimations;
        GetComponent<UniversalAISystem>().Settings.Movement.UseTurnSystem =
            Movement.UseTurnAnimations && CombatMovement.UseCombatTurnAnimations
                ? UniversalAIEnums.YesNo.Yes
                : UniversalAIEnums.YesNo.No;
        //Movement & Combat
    if(Combat.HitReactionAnimations.Length > 0)
    {
        Combat.UseHitReactions = true;
    }

    if (Movement.UseTurnAnimations)
        {
            for (int a = 0; a < m_AnimatorController.layers[0].stateMachine.states.Length; a++)
            {
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Turn Left")
                {
                    if (Movement.TurnLeft == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'TURN LEFT' </b> animation missing! Be sure you have entered <b> an </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = Movement.TurnLeft;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = Movement.TurnLeftSpeed;
                    }
                }
                
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Turn Right")
                {
                    if (Movement.TurnLeft == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'TURN RIGHT' </b> animation missing! Be sure you have entered <b> an </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = Movement.TurnRight;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = Movement.TurnRightSpeed;
                    }
                }
            }
        }
        
        if (CombatMovement.UseCombatTurnAnimations)
        {
            for (int a = 0; a < m_AnimatorController.layers[0].stateMachine.states.Length; a++)
            {
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Turn Left Combat")
                {
                    if (Movement.TurnLeft == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'COMBAT TURN LEFT' </b> animation missing! Be sure you have entered <b> an </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = CombatMovement.CombatTurnLeft;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = CombatMovement.CombatTurnLeftSpeed;
                    }
                }
                
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Turn Right Combat")
                {
                    if (CombatMovement.CombatTurnLeft == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'COMBAT TURN RIGHT' </b> animation missing! Be sure you have entered <b> an </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = CombatMovement.CombatTurnRight;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = CombatMovement.CombatTurnRightSpeed;
                    }
                }
            }
        }
        
    if (Combat.IsWeapon)
    {
        if (Combat.IsShooterWeapon)
        {
            for (int a = 0; a < m_AnimatorController.layers[0].stateMachine.states.Length; a++)
            {
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Reload")
                {
                    if (Combat.ReloadAnimation == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'RELOAD' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = Combat.ReloadAnimation;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = Combat.ReloadAnimationSpeed;
                    }
                }
            }
        }

        if (Combat.UseEquipAnimations)
        {
            for (int a = 0; a < m_AnimatorController.layers[0].stateMachine.states.Length; a++)
            {
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "Equip")
                {
                    if (Combat.EquipAnimation == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'EQUIP' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = Combat.EquipAnimation;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = Combat.EquipAnimationSpeed;
                    }
                }
                if (m_AnimatorController.layers[0].stateMachine.states[a].state.name == "UnEquip")
                {
                    if (Combat.UnEquipAnimation == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'UNEQUIP' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        m_AnimatorController.layers[0].stateMachine.states[a].state.motion = Combat.UnEquipAnimation;
                        m_AnimatorController.layers[0].stateMachine.states[a].state.speed = Combat.UnEquipAnimationSpeed;
                    }
                }
            }
        }
        
        
        if (Failed)
        {
            GetComponent<Animator>().runtimeAnimatorController = null;

            try
            {
                AssetDatabase.DeleteAsset(path);
            }
            catch
            {
                return;
            }

            return;
        }
    }
   
        for (int i = 0; i < m_AnimatorController.layers[0].stateMachine.stateMachines.Length; i++)
        {
            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Hit States" && Combat.UseHitReactions) 
            { 
                    if (Combat.HitReactionAnimations.Length <= 0)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'HIT REACTION' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        if (Combat.HitReactionAnimations[0].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 1' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion
                            = Combat.HitReactionAnimations[0].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed
                            = Combat.HitReactionAnimations[0].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[0].AnimationSpeed;
                    }

                    if (Combat.HitReactionAnimations.Length >= 2)
                    {
                        if (Combat.HitReactionAnimations[1].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 2' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }
                        else
                        {
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion
                                = Combat.HitReactionAnimations[1].AnimationClip;
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed
                                = Combat.HitReactionAnimations[1].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[1].AnimationSpeed;;   
                        }
                    }

                    if (Combat.HitReactionAnimations.Length >= 3)
                    {
                        if (Combat.HitReactionAnimations[2].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 3' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion
                            = Combat.HitReactionAnimations[2].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed
                            = Combat.HitReactionAnimations[2].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[2].AnimationSpeed;;
                    }

                
            }
            
            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Combat Block States" && Combat.UseBlockingSystem) 
            { 
                    if (Combat.BlockAnimations.Length <= 0)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'BLOCK' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        if (Combat.BlockAnimations[0].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'BLOCK 1' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }
                        else
                        {
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state
                                    .motion
                                = Combat.BlockAnimations[0].AnimationClip;
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state
                                    .speed
                                = Combat.BlockAnimations[0].AnimationSpeed == 0
                                    ? 1
                                    : Combat.BlockAnimations[0].AnimationSpeed;
                        }
                    }

                    if (Combat.BlockAnimations.Length >= 2)
                    {
                        if (Combat.BlockAnimations[1].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'BLOCK 2' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }
                        else
                        {
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state
                                    .motion
                                = Combat.BlockAnimations[1].AnimationClip;
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state
                                    .speed
                                = Combat.BlockAnimations[1].AnimationSpeed == 0
                                    ? 1
                                    : Combat.BlockAnimations[1].AnimationSpeed;
                        }
                    }

                    if (Combat.BlockAnimations.Length >= 3)
                    {
                        if (Combat.BlockAnimations[2].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'BLOCK 3' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }
                        else
                        {
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion
                                = Combat.BlockAnimations[2].AnimationClip;
                            m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed
                                = Combat.BlockAnimations[2].AnimationSpeed == 0 ? 1 : Combat.BlockAnimations[2].AnimationSpeed;;   
                        }
                    }

                
            }
            
            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Idle States")
            {
                if (Movement.Idles.Length <= 0)
                {
                    Debug.LogError(
                        "[UniversalAI] The AI has the <b> 'IDLE' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                        gameObject);
                    Failed = true;
                }
                else
                {
                    if (Movement.Idles[0].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'IDLE 1' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion =
                        Movement.Idles[0].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed =
                        Movement.Idles[0].AnimationSpeed == 0 ? 1 : Movement.Idles[0].AnimationSpeed;;
                }

                if (Movement.Idles.Length >= 2)
                {
                    if (Movement.Idles[1].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'IDLE 2' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion =
                        Movement.Idles[1].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed =
                        Movement.Idles[1].AnimationSpeed == 0 ? 1 : Movement.Idles[1].AnimationSpeed;;
                }

                if (Movement.Idles.Length >= 3)
                {
                    if (Movement.Idles[2].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'IDLE 3' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion =
                        Movement.Idles[2].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed =
                        Movement.Idles[2].AnimationSpeed == 0 ? 1 : Movement.Idles[2].AnimationSpeed;;
                }

            }
            
            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Combat Idle States")
            {
               
                if (CombatMovement.CombatIdles.Length <= 0)
                {
                    Debug.LogError(
                        "[UniversalAI] The AI has the <b> ' COMBAT IDLE' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                        gameObject);
                    Failed = true;
                }
                else
                {
                    if (CombatMovement.CombatIdles[0].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'COMBAT IDLE 1' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion =
                        CombatMovement.CombatIdles[0].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed =
                        CombatMovement.CombatIdles[0].AnimationSpeed == 0 ? 1 : CombatMovement.CombatIdles[0].AnimationSpeed;;
                }

                if (CombatMovement.CombatIdles.Length >= 2)
                {
                    if (CombatMovement.CombatIdles[1].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'COMBAT IDLE 2' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion =
                        CombatMovement.CombatIdles[1].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed =
                        CombatMovement.CombatIdles[1].AnimationSpeed == 0 ? 1 : CombatMovement.CombatIdles[1].AnimationSpeed;;
                }

                if (CombatMovement.CombatIdles.Length >= 3)
                {
                    if (CombatMovement.CombatIdles[2].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'COMBAT IDLE 3' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion =
                        CombatMovement.CombatIdles[2].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed =
                        CombatMovement.CombatIdles[2].AnimationSpeed == 0 ? 1 : CombatMovement.CombatIdles[2].AnimationSpeed;;
                }

            }

            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Death States")
            {
                if(GetComponent<UniversalAISystem>().General.DeathType == UniversalAIEnums.DeathMethod.Ragdoll)
                    continue;
                
                if (Combat.DeathAnimations.Length <= 0) 
                {
                    Debug.LogError(
                        "[UniversalAI] The AI has the <b> 'DEATH' </b> animation missing! Changing The 'AI DEATH TYPE' To Ragdoll !",
                        gameObject); 
                }
                else 
                {
                    if (Combat.DeathAnimations[0].AnimationClip == null)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI's <b> 'DEATH 1' </b> animation is null! Be sure you fill or delete it from the list!",
                            gameObject);
                        Failed = true;
                    }

                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion =
                        Combat.DeathAnimations[0].AnimationClip;
                    m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed =
                        Combat.DeathAnimations[0].AnimationSpeed == 0 ? 1 : Combat.DeathAnimations[0].AnimationSpeed;;


                    if (Combat.DeathAnimations.Length >= 2)
                    {
                        if (Combat.DeathAnimations[1].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'DEATH 2' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion
                            = Combat.DeathAnimations[1].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed
                            = Combat.DeathAnimations[1].AnimationSpeed == 0 ? 1 : Combat.DeathAnimations[1].AnimationSpeed;;
                    }

                    if (Combat.DeathAnimations.Length >= 3)
                    {
                        if (Combat.DeathAnimations[2].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'DEATH 3' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion
                            = Combat.DeathAnimations[2].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed
                            = Combat.DeathAnimations[2].AnimationSpeed == 0 ? 1 : Combat.DeathAnimations[2].AnimationSpeed;;
                    } 
                }
            }

            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Combat Hit States"  && Combat.UseHitReactions)
            {
                   
                    if (Combat.HitReactionAnimations.Length <= 0)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'HIT REACTION' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation!",
                            gameObject);
                        Failed = true;
                    }
                    else
                    {
                        if (Combat.HitReactionAnimations[0].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 1' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion
                            = Combat.HitReactionAnimations[0].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed
                            = Combat.HitReactionAnimations[0].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[0].AnimationSpeed;;
                    }

                    if (Combat.HitReactionAnimations.Length >= 2)
                    {
                        if (Combat.HitReactionAnimations[1].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 2' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion
                            = Combat.HitReactionAnimations[1].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed
                            = Combat.HitReactionAnimations[1].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[0].AnimationSpeed;;
                    }

                    if (Combat.HitReactionAnimations.Length >= 3)
                    {
                        if (Combat.HitReactionAnimations[2].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'HIT REACTION 3' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion
                            = Combat.HitReactionAnimations[2].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed
                            = Combat.HitReactionAnimations[2].AnimationSpeed == 0 ? 1 : Combat.HitReactionAnimations[0].AnimationSpeed;;
                    }

            }
                
            if (m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name == "Attack States")
            {
                    if (Combat.AttackAnimations.Length <= 0)
                    {
                        Debug.LogError(
                            "[UniversalAI] The AI has the <b> 'ATTACK' </b> animation missing! Be sure you have entered at least <b> 1 </b> animation if you are going to use brave state!",
                            gameObject);
                       continue;
                    }
                    else
                    {
                        if (Combat.AttackAnimations[0].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'ATTACK 1' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.motion
                            = Combat.AttackAnimations[0].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[0].state.speed
                            = Combat.AttackAnimations[0].AnimationSpeed == 0 ? 1 : Combat.AttackAnimations[0].AnimationSpeed;;
                    }

                    if (Combat.AttackAnimations.Length >= 2)
                    {
                        if (Combat.AttackAnimations[1].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'ATTACK 2' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.motion
                            = Combat.AttackAnimations[1].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[1].state.speed
                            = Combat.AttackAnimations[1].AnimationSpeed == 0 ? 1 : Combat.AttackAnimations[1].AnimationSpeed;;
                    }

                    if (Combat.AttackAnimations.Length >= 3)
                    {
                        if (Combat.AttackAnimations[2].AnimationClip == null)
                        {
                            Debug.LogError(
                                "[UniversalAI] The AI's <b> 'ATTACK 3' </b> animation is null! Be sure you fill or delete it from the list!",
                                gameObject);
                            Failed = true;
                        }

                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.motion
                            = Combat.AttackAnimations[2].AnimationClip;
                        m_AnimatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states[2].state.speed
                            = Combat.AttackAnimations[2].AnimationSpeed == 0 ? 1 : Combat.AttackAnimations[2].AnimationSpeed;;
                    }
                    
            }
            

            UnityEditor.Animations.AnimatorState m_StateMachine =
                m_AnimatorController.layers[0].stateMachine.states[0].state;
            UnityEditor.Animations.BlendTree MovementBlendTree =
                m_StateMachine.motion as UnityEditor.Animations.BlendTree;

            var SerializedIdleBlendTreeRef = new SerializedObject(MovementBlendTree);
            var MovementBlendTreeChildren = SerializedIdleBlendTreeRef.FindProperty("m_Childs");

            var MovementMotionSlot0 = MovementBlendTreeChildren.GetArrayElementAtIndex(0);
            var MovementMotion0 = MovementMotionSlot0.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree WalkBackBlendTree =
                MovementMotion0.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedWalkBackBlendTree = new SerializedObject(WalkBackBlendTree);
            var WalkBackBlendTreeChildren = SerializedWalkBackBlendTree.FindProperty("m_Childs");
            var WalkBackMotionSlot = WalkBackBlendTreeChildren.GetArrayElementAtIndex(0);
            var WalkBackAnimation = WalkBackMotionSlot.FindPropertyRelative("m_Motion");
            var WalkBackAnimationSpeed = WalkBackMotionSlot.FindPropertyRelative("m_TimeScale");
            
            if (Movement.WalkBackwards == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Walk Backwards' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                WalkBackAnimationSpeed.floatValue = Movement.BackwardsSpeed;
                WalkBackAnimation.objectReferenceValue = Movement.WalkBackwards;
            }
            
            SerializedWalkBackBlendTree.ApplyModifiedProperties();
            
            var MovementMotionSlot1 = MovementBlendTreeChildren.GetArrayElementAtIndex(1);
            var MovementMotion1 = MovementMotionSlot1.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree IdleBlendTree =
                MovementMotion1.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedIdleBlendTree = new SerializedObject(IdleBlendTree);
            var IdleBlendTreeChildren = SerializedIdleBlendTree.FindProperty("m_Childs");
            var IdleMotionSlot = IdleBlendTreeChildren.GetArrayElementAtIndex(0);
            var IdleAnimation = IdleMotionSlot.FindPropertyRelative("m_Motion");
            var IdleAnimationSpeed = IdleMotionSlot.FindPropertyRelative("m_TimeScale");
            IdleAnimationSpeed.floatValue = Movement.Idles[0].AnimationSpeed == 0 ? 1 : Movement.Idles[0].AnimationSpeed;;
            IdleAnimation.objectReferenceValue = Movement.Idles[0].AnimationClip;
            SerializedIdleBlendTree.ApplyModifiedProperties();

            var MovementMotionSlot2 = MovementBlendTreeChildren.GetArrayElementAtIndex(2);
            var MovementMotion2 = MovementMotionSlot2.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree WalkBlendTree =
                MovementMotion2.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedWalkBlendTree = new SerializedObject(WalkBlendTree);
            var WalkBlendTreeChildren = SerializedWalkBlendTree.FindProperty("m_Childs");

            // Left
            var WalkMotionSlot1 = WalkBlendTreeChildren.GetArrayElementAtIndex(0);
            var WalkLeftAnimation = WalkMotionSlot1.FindPropertyRelative("m_Motion");
            var WalkLeftAnimationSpeed = WalkMotionSlot1.FindPropertyRelative("m_TimeScale");
            var WalkLeftMirror = WalkMotionSlot1.FindPropertyRelative("m_Mirror");
            if (Movement.WalkLeft == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Walk Left' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                WalkLeftAnimationSpeed.floatValue = Movement.LeftSpeed;
                WalkLeftAnimation.objectReferenceValue = Movement.WalkLeft;
                WalkLeftMirror.boolValue = Movement.MirrorLeftAnimation;
            }
            

            // Forward
            var WalkMotionSlot2 = WalkBlendTreeChildren.GetArrayElementAtIndex(1);
            var WalkStraightAnimation = WalkMotionSlot2.FindPropertyRelative("m_Motion");
            var WalkStraightAnimationSpeed = WalkMotionSlot2.FindPropertyRelative("m_TimeScale");
            if (Movement.WalkForward == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Walk Forward' </b> animation is null! Be sure to fill it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                WalkStraightAnimationSpeed.floatValue = Movement.ForwardSpeed;
                WalkStraightAnimation.objectReferenceValue = Movement.WalkForward;
            }
           

            // Right
            var WalkMotionSlot3 = WalkBlendTreeChildren.GetArrayElementAtIndex(2);
            var WalkRightAnimation = WalkMotionSlot3.FindPropertyRelative("m_Motion");
            var WalkRightAnimationSpeed = WalkMotionSlot3.FindPropertyRelative("m_TimeScale");
            var WalkRightMirror = WalkMotionSlot3.FindPropertyRelative("m_Mirror");
            if (Movement.WalkRight == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Walk Right' </b> animation is null!  If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                WalkRightAnimationSpeed.floatValue = Movement.RightSpeed;
                WalkRightAnimation.objectReferenceValue = Movement.WalkRight;
                WalkRightMirror.boolValue = Movement.MirrorRightAnimation;
            }
           

            SerializedWalkBlendTree.ApplyModifiedProperties();
            
            //Running
            var MovementMotionSlot3 = MovementBlendTreeChildren.GetArrayElementAtIndex(3);
            var MovementMotion3 = MovementMotionSlot3.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree RunBlendTree = MovementMotion3.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedRunBlendTree = new SerializedObject(RunBlendTree);
            var RunBlendTreeChildren = SerializedRunBlendTree.FindProperty("m_Childs");

            // Left
            var RunMotionSlot1 = RunBlendTreeChildren.GetArrayElementAtIndex(0);
            var RunLeftAnimation = RunMotionSlot1.FindPropertyRelative("m_Motion");
            var RunLeftAnimationSpeed = RunMotionSlot1.FindPropertyRelative("m_TimeScale");
            var RunLeftMirror = RunMotionSlot1.FindPropertyRelative("m_Mirror");
            if (Movement.RunLeft == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Strafe Left' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                RunLeftAnimationSpeed.floatValue = Movement.RunLeftSpeed;
                RunLeftAnimation.objectReferenceValue = Movement.RunLeft;
                RunLeftMirror.boolValue = Movement.MirrorRunLeftAnimation;
            }
        

            // Forward
            
            var RunMotionSlot2 = RunBlendTreeChildren.GetArrayElementAtIndex(1);
            var RunStraightAnimation = RunMotionSlot2.FindPropertyRelative("m_Motion");
            var RunStraightAnimationSpeed = RunMotionSlot2.FindPropertyRelative("m_TimeScale");
            if (Movement.RunForward == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Run Forward' </b> animation is null! Be sure to fill it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                RunStraightAnimationSpeed.floatValue = Movement.RunForwardSpeed;
                RunStraightAnimation.objectReferenceValue = Movement.RunForward;
            }
            

            // Right
            var RunMotionSlot3 = RunBlendTreeChildren.GetArrayElementAtIndex(2);
            var RunRightAnimation = RunMotionSlot3.FindPropertyRelative("m_Motion");
            var RunRightAnimationSpeed = RunMotionSlot3.FindPropertyRelative("m_TimeScale");
            var RunRightMirror = RunMotionSlot3.FindPropertyRelative("m_Mirror");
            if (Movement.RunRight == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Strafe Right' </b> animation is null!  If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                RunRightAnimationSpeed.floatValue = Movement.RunRightSpeed;
                RunRightAnimation.objectReferenceValue = Movement.RunRight;
                RunRightMirror.boolValue = Movement.MirrorRunRightAnimation;
            }
            


            SerializedRunBlendTree.ApplyModifiedProperties();
            
            //CombatMovement
            
            UnityEditor.Animations.AnimatorState m_StateMachine_Combat = m_AnimatorController.layers[0].stateMachine.states[1].state;
            UnityEditor.Animations.BlendTree CombatMovementBlendTree = m_StateMachine_Combat.motion as UnityEditor.Animations.BlendTree;

            var SerializedCombatIdleBlendTreeRef = new SerializedObject(CombatMovementBlendTree);
            var CombatMovementBlendTreeChildren = SerializedCombatIdleBlendTreeRef.FindProperty("m_Childs");

            var CombatMovementMotionSlot0 = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(0);
            var CombatMovementMotion0 = CombatMovementMotionSlot0.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree CombatBackBlendTree = CombatMovementMotion0.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedCombatBackBlendTree = new SerializedObject(CombatBackBlendTree);
            var CombatBackBlendTreeChildren = SerializedCombatBackBlendTree.FindProperty("m_Childs");
            var CombatBackMotionSlot = CombatBackBlendTreeChildren.GetArrayElementAtIndex(0);
            var CombatBackAnimation = CombatBackMotionSlot.FindPropertyRelative("m_Motion");
            var CombatBackAnimationSpeed = CombatBackMotionSlot.FindPropertyRelative("m_TimeScale");
            var CombatWalkBackMirror = CombatBackMotionSlot.FindPropertyRelative("m_Mirror");
            if (CombatMovement.CombatWalkBackwards == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'Combat Walk Backwards' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
            {
                CombatBackAnimation.objectReferenceValue = CombatMovement.CombatWalkBackwards;
                CombatBackAnimationSpeed.floatValue = CombatMovement.WalkBackwardsSpeed;
                CombatWalkBackMirror.boolValue = CombatMovement.MirrorCombatBackwards;
            }
           
           
            SerializedCombatBackBlendTree.ApplyModifiedProperties();
            
            var CombatMovementMotionSlot1 = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(1);
            var CombatMovementMotion1 = CombatMovementMotionSlot1.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree CombatIdleBlendTree = CombatMovementMotion1.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedCombatIdleBlendTree = new SerializedObject(CombatIdleBlendTree);
            var CombatIdleBlendTreeChildren = SerializedCombatIdleBlendTree.FindProperty("m_Childs");
            var CombatIdleMotionSlot = CombatIdleBlendTreeChildren.GetArrayElementAtIndex(0);
            var CombatIdleAnimation = CombatIdleMotionSlot.FindPropertyRelative("m_Motion");
            var CombatIdleAnimationSpeed = CombatIdleMotionSlot.FindPropertyRelative("m_TimeScale");
            
            CombatIdleAnimationSpeed.floatValue = CombatMovement.CombatIdles[0].AnimationSpeed == 0 ? 1 : CombatMovement.CombatIdles[0].AnimationSpeed;
            CombatIdleAnimation.objectReferenceValue = CombatMovement.CombatIdles[0].AnimationClip;
            SerializedCombatIdleBlendTree.ApplyModifiedProperties();

            var CombatMovementMotionSlot2 = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(2);
            var CombatMovementMotion2 = CombatMovementMotionSlot2.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree CombatWalkBlendTree = CombatMovementMotion2.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var SerializedCombatWalkBlendTree = new SerializedObject(CombatWalkBlendTree);
            var CombatWalkBlendTreeChildren = SerializedCombatWalkBlendTree.FindProperty("m_Childs");
            
           
            var CombatWalkMovementMotionThreshold = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(1).FindPropertyRelative("m_Threshold");
            var CombatRunMovementMotionThreshold = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(2).FindPropertyRelative("m_Threshold");

            SerializedCombatIdleBlendTreeRef.ApplyModifiedProperties();

            
            
             //Run
            var CombatMovementMotionSlot3 = CombatMovementBlendTreeChildren.GetArrayElementAtIndex(3);
            var CombatMovementMotion3 = CombatMovementMotionSlot3.FindPropertyRelative("m_Motion");
            UnityEditor.Animations.BlendTree CombatRunBlendTree = CombatMovementMotion3.objectReferenceValue as UnityEditor.Animations.BlendTree;
            var CombatSerializedRunBlendTree = new SerializedObject(CombatRunBlendTree);
            var CombatRunBlendTreeChildren = CombatSerializedRunBlendTree.FindProperty("m_Childs");


            //Run Left
            var CombatRunMotionSlot1 = CombatRunBlendTreeChildren.GetArrayElementAtIndex(0);
            var CombatRunLeftAnimation = CombatRunMotionSlot1.FindPropertyRelative("m_Motion");
            var CombatRunLeftAnimationSpeed = CombatRunMotionSlot1.FindPropertyRelative("m_TimeScale");
            var CombatRunLeftMirror = CombatRunMotionSlot1.FindPropertyRelative("m_Mirror");
        
            CombatRunLeftAnimationSpeed.floatValue = CombatMovement.RunLeftSpeed;
            if (CombatMovement.CombatRunLeft == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatRunLeft' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
             CombatRunLeftAnimation.objectReferenceValue = CombatMovement.CombatRunLeft;
            CombatRunLeftMirror.boolValue = CombatMovement.MirrorRunCombatLeft;
          

            //Run Straight
            var CombatRunMotionSlot2 = CombatRunBlendTreeChildren.GetArrayElementAtIndex(1);
            var CombatRunStraightAnimation = CombatRunMotionSlot2.FindPropertyRelative("m_Motion");
            var CombatRunStraightAnimationSpeed = CombatRunMotionSlot2.FindPropertyRelative("m_TimeScale");
            CombatRunStraightAnimationSpeed.floatValue = CombatMovement.RunForwardSpeed;
            if (CombatMovement.CombatRunForward == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatRunForward' </b> animation is null! Be sure you fill it!",
                    gameObject);
                Failed = true;
            }
            else
             CombatRunStraightAnimation.objectReferenceValue = CombatMovement.CombatRunForward;

            //Run Right
            var CombatRunMotionSlot3 = CombatRunBlendTreeChildren.GetArrayElementAtIndex(2);
            var CombatRunRightAnimation = CombatRunMotionSlot3.FindPropertyRelative("m_Motion");
            var CombatRunRightAnimationSpeed = CombatRunMotionSlot3.FindPropertyRelative("m_TimeScale");
            var CombatRunRightMirror = CombatRunMotionSlot3.FindPropertyRelative("m_Mirror");
            CombatRunRightAnimationSpeed.floatValue = CombatMovement.RunRightSpeed;
            if (CombatMovement.CombatWalkRight == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatWalkRight' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
             CombatRunRightAnimation.objectReferenceValue = CombatMovement.CombatRunRight;
            
            CombatRunRightMirror.boolValue = CombatMovement.MirrorRunCombatRight;
            CombatSerializedRunBlendTree.ApplyModifiedProperties();
            
            //Move Left
            var CombatWalkMotionSlot1 = CombatWalkBlendTreeChildren.GetArrayElementAtIndex(0);
            var CombatWalkLeftAnimation = CombatWalkMotionSlot1.FindPropertyRelative("m_Motion");
            var CombatWalkLeftAnimationSpeed = CombatWalkMotionSlot1.FindPropertyRelative("m_TimeScale");
            var CombatWalkLeftMirror = CombatWalkMotionSlot1.FindPropertyRelative("m_Mirror");
            CombatWalkLeftAnimationSpeed.floatValue = CombatMovement.WalkLeftSpeed;
            if (CombatMovement.CombatWalkLeft == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatWalkLeft' </b> animation is null! If u don't have a turn animation, you can use walk animation instead of it!",
                    gameObject);
                Failed = true;
            }
            else
                CombatWalkLeftAnimation.objectReferenceValue = CombatMovement.CombatWalkLeft;
            
            CombatWalkLeftMirror.boolValue = CombatMovement.MirrorCombatLeft;

            //Move Straight
            
            var CombatWalkMotionSlot2 = CombatWalkBlendTreeChildren.GetArrayElementAtIndex(1);
            var CombatWalkStraightAnimation = CombatWalkMotionSlot2.FindPropertyRelative("m_Motion");
            var CombatWalkStraightAnimationSpeed = CombatWalkMotionSlot2.FindPropertyRelative("m_TimeScale");
            CombatWalkStraightAnimationSpeed.floatValue = CombatMovement.WalkForwardSpeed;
            if (CombatMovement.CombatWalkForward == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatWalkForward' </b> animation is null! Be sure you fill it!",
                    gameObject);
                Failed = true;
            }
            else
             CombatWalkStraightAnimation.objectReferenceValue = CombatMovement.CombatWalkForward;

            

            //Move Right
            var CombatWalkMotionSlot3 = CombatWalkBlendTreeChildren.GetArrayElementAtIndex(2);
            var CombatWalkRightAnimation = CombatWalkMotionSlot3.FindPropertyRelative("m_Motion");
            var CombatWalkRightAnimationSpeed = CombatWalkMotionSlot3.FindPropertyRelative("m_TimeScale");
            var CombatWalkRightMirror = CombatWalkMotionSlot3.FindPropertyRelative("m_Mirror");
            CombatWalkRightAnimationSpeed.floatValue = CombatMovement.WalkRightSpeed;
            if (CombatMovement.CombatWalkRight == null)
            {
                Debug.LogError(
                    "[UniversalAI] The AI's <b> 'CombatWalkRight' </b> animation is null! Be sure you fill it!",
                    gameObject);
                Failed = true;
            }
            else
             CombatWalkRightAnimation.objectReferenceValue = CombatMovement.CombatWalkRight;
            CombatWalkRightMirror.boolValue =CombatMovement.MirrorCombatRight;

            SerializedCombatWalkBlendTree.ApplyModifiedProperties();

           
            
            #endregion

            if (Failed)
            {
                GetComponent<Animator>().runtimeAnimatorController = null;

                try
                {
                    AssetDatabase.DeleteAsset(path);
                }
                catch
                {
                    return;
                }

                return;
            }


        }

        
    } 
#endif 
    }

}
