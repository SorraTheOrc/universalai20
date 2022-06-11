using System;
using UnityEngine;

namespace UniversalAI
{
    [CreateAssetMenu(fileName = "Animation Profile", menuName = "UniversalAI/AnimationProfile", order = 1)]
    public class AnimationStorage : ScriptableObject
    {
        [Header("MOVEMENT")] 
        
        [Space] 
        [Reorderable] public UniversalAIAnimatorCreator.AnimationClipOverrideList Idles;
        
        [Space] 
        public AnimationClip WalkForward;
        public float ForwardSpeed = 1f;
        public bool MirrorForwardAnimation;

        [Space] public AnimationClip WalkRight;
        public float RightSpeed = 1f;
        public bool MirrorRightAnimation;

        [Space] public AnimationClip WalkLeft;
        public float LeftSpeed = 1f;
        public bool MirrorLeftAnimation;

        [Space] public AnimationClip WalkBackwards;
        public float BackwardsSpeed = 1f;
        public bool MirrorBackwardsAnimation;

        [Space] public AnimationClip RunForward;
        public float RunForwardSpeed = 1f;
        public bool MirrorRunForwardAnimation;

        [Space] public AnimationClip RunRight;
        public float RunRightSpeed = 1f;
        public bool MirrorRunRightAnimation;

        [Space] public AnimationClip RunLeft;
        public float RunLeftSpeed = 1f;
        public bool MirrorRunLeftAnimation;

        [Space] public bool UseTurnAnimations = true;
        [Space] [Space] public AnimationClip TurnLeft;
        public float TurnLeftSpeed = 1f;
        public bool MirrorTurnLeft;


        [Space] public AnimationClip TurnRight;
        public float TurnRightSpeed = 1f;
        public bool MirrorTurnRight;

        //Combat

        [Header("COMBAT MOVEMENT")]
        [Reorderable]
        public UniversalAIAnimatorCreator.AnimationClipOverrideList CombatIdles;
        
        [Space]
        public AnimationClip CombatWalkForward;
        public float WalkForwardSpeed = 1f;
        public bool MirrorCombatForward; 
       

        [Space]
        public AnimationClip CombatWalkRight;
        public float WalkRightSpeed = 1f;
        public bool MirrorCombatRight; 
        

        [Space]
        public AnimationClip CombatWalkLeft;
        public float WalkLeftSpeed = 1f;
        public bool MirrorCombatLeft;
        
        [Space] 
        public AnimationClip CombatWalkBackwards;
        public float WalkBackwardsSpeed = 1f;
        public bool MirrorCombatBackwards;
        
        [Space] 
        public AnimationClip CombatRunForward;
        public float CombatRunForwardSpeed = 1f;
        public bool MirrorCombatRunForward;

        [Space]
        public AnimationClip CombatRunRight;
        public float CombatRunRightSpeed = 1f;
        public bool MirrorRunCombatRight; 

        [Space]
        public bool UseCombatTurnAnimations = true;
        
        [Space] public AnimationClip CombatRunLeft;
        public float CombatRunLeftSpeed = 1f;
        public bool MirrorRunCombatLeft;

        [Space] 
        public AnimationClip CombatTurnLeft;
        public float CombatTurnLeftSpeed = 1f;
        public bool MirrorCombatTurnLeft;
        
        [Space]
        public AnimationClip CombatTurnRight;
        public float CombatTurnRightSpeed = 1f;
        public bool MirrorCombatTurnRight;
        
        //Combat
        
        [Space] 
        [Header("COMBAT")]
        [Reorderable]
        public UniversalAIAnimatorCreator.AnimationAttackClipOverrideList AttackAnimations;

        [Space] 
        public bool UseHitReactions;
        [Reorderable] public UniversalAIAnimatorCreator.AnimationClipOverrideList HitReactionAnimations;
       
        [Space]
        public bool IsWeapon = false;
        [Space]
        
     
        public bool UseEquipAnimations = true;
        public AnimationClip EquipAnimation;
        public float EquipAnimationSpeed = 1f;
        
        [Space]
        public AnimationClip UnEquipAnimation;
        public float UnEquipAnimationSpeed = 1f;
        
        [Space]
        public bool UseBlockingSystem = false;
        [Reorderable] public UniversalAIAnimatorCreator.AnimationClipOverrideList BlockAnimations;
        
        [Space]
        public bool IsShooterWeapon = false;
        
        [Space]
        public AnimationClip ReloadAnimation;
        public float ReloadAnimationSpeed = 1f;
        
        [Space]
        [Reorderable]
        public UniversalAIAnimatorCreator.AnimationClipOverrideList DeathAnimations;
    }
}