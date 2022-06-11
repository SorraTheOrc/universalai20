using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniversalAI
{

    [CustomEditor(typeof(UniversalAIAnimatorCreator))]
    public class UniversalAIAnimatorCreatorEditor : Editor
    {
        private UniversalAIAnimatorCreator AITarget;
        
        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
        private bool Open;

        private void OnEnable()
        {
            AITarget = (UniversalAIAnimatorCreator) target;
        }


        private bool IgnoreWalkBack;
        private bool IgnoreWalkBackCombat;
        private bool IgnoreAttacks;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.fontSize = 13;
            EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
            EditorGUILayout.LabelField("Universal AI Animations", style, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
           
            if (!Application.isPlaying)
            {
                #region Error Checker

                AITarget.StartWithError = false;
                AITarget.ErrorCombat = false;
                AITarget.ErrorMovement = false;
                AITarget.ErrorCombatMovement = false;
                
                if (AITarget.Movement.Idles.Length <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Idle' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(AITarget.Movement.Idles[0].AnimationClip == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Idle' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.Idles[0].AnimationClip.isLooping)
                {
                    bool Found = false;
                    foreach (var anim in AITarget.Movement.Idles)
                    {
                        if (!anim.AnimationClip.isLooping)
                        {
                            Found = true;
                            break;
                        }
                    }

                    if (Found)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox("At least on of the 'Idle' animations aren't set to loop, please fix it!", MessageType.Error);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                        AITarget.StartWithError = true;
                        AITarget.ErrorMovement = true;
                    }
                }
                else if (AITarget.Movement.WalkForward == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Forward' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.WalkForward.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Forward' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.WalkRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Right' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.WalkRight.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Right' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.WalkLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Left' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.WalkLeft.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Left' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.WalkBackwards == null && !IgnoreWalkBack)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Backwards' animation is empty, please assign an animation in order to make your AI backup!", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
 
                    if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                    {
                        IgnoreWalkBack = true;
                    }
                
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                
                    EditorGUILayout.Space();
                    AITarget.StartWithError = false;
                    AITarget.ErrorMovement = true;
                }
                else if(!IgnoreWalkBack && !AITarget.Movement.WalkBackwards.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Walk Backwards' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.RunForward == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Forward' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.RunForward.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Forward' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.RunRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Right' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                }
                else if(!AITarget.Movement.RunRight.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Right' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.RunLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Left' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.Movement.RunLeft.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Run Left' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.UseTurnAnimations && AITarget.Movement.TurnLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Turn Left' animation is null, please assign one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.Movement.UseTurnAnimations && AITarget.Movement.TurnRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Turn Right' animation is null, please assign one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = false;
                    AITarget.ErrorMovement = true;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatIdles.Length <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Idle' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(AITarget.CombatMovement.CombatIdles[0].AnimationClip == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Idle' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!AITarget.CombatMovement.CombatIdles[0].AnimationClip.isLooping)
                {
                    bool Found = false;
                    foreach (var anim in AITarget.CombatMovement.CombatIdles)
                    {
                        if (!anim.AnimationClip.isLooping)
                        {
                            Found = true;
                            break;
                        }
                    }

                    if (Found)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox("At least on of the 'Combat Idle' animations aren't set to loop, please fix it!", MessageType.Error);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                        AITarget.StartWithError = true;
                        AITarget.ErrorCombatMovement = true;
                        AITarget.ErrorMovement = false;
                        AITarget.ErrorCombat = false;
                    }
                }
                else if (AITarget.CombatMovement.CombatWalkForward == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Forward' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatWalkForward.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Forward' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatWalkRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Right' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatWalkRight.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Right' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatWalkLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Left' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatWalkLeft.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Left' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatWalkBackwards == null && !IgnoreWalkBackCombat)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Backwards' animation is empty, please assign an animation in order to make your AI backup!", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
 
                    if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                    {
                        IgnoreWalkBackCombat = true;
                    }
                
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                
                    EditorGUILayout.Space();
                    AITarget.StartWithError = false;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if(!IgnoreWalkBackCombat && !AITarget.CombatMovement.CombatWalkBackwards.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Walk Backwards' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatRunForward == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Forward' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatRunForward.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Forward' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatRunRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Right' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatRunRight.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Right' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.CombatRunLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Right' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!AITarget.CombatMovement.CombatRunLeft.isLooping)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Run Left' animation isn't set to loop, please fix it!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.UseCombatTurnAnimations && AITarget.CombatMovement.CombatTurnLeft == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Turn Left' animation is null, please assign one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (AITarget.CombatMovement.UseCombatTurnAnimations && AITarget.CombatMovement.CombatTurnRight == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Combat Turn Right' animation is null, please assign one!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombatMovement = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombat = false;
                }
                else if (!IgnoreAttacks && AITarget.Combat.AttackAnimations.Length <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Attack' animations are empty, please assign at least one animation in order to make your AI Attack. (If you are using coward or pet AI, you can leave attacks empty)", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
 
                    if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                    {
                        IgnoreAttacks = true;
                    }
                
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                
                    EditorGUILayout.Space();
                    
                    
                    AITarget.StartWithError = false;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(!IgnoreAttacks && AITarget.Combat.AttackAnimations[0].AnimationClip == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Attack' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(!IgnoreAttacks && AITarget.Combat.AttackAnimations[0].AnimationClip != null && !AITarget.Combat.IsWeapon)
                {
                    bool EventFound = false;
                    if (AITarget.Combat.AttackAnimations[0].AnimationClip.events.Length > 0)
                    {
                        foreach (var Event in AITarget.Combat.AttackAnimations[0].AnimationClip.events)
                        {
                            if (Event.functionName == "UniversalAIAttack")
                            {
                                EventFound = true;
                                break;
                            }
                        }
                    }

                    if (!EventFound || AITarget.Combat.AttackAnimations[0].AnimationClip.events.Length <= 0)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox("The 'UniversalAIAttack' event couldn't be found on at least one of the attack animations, please follow the documentation below!", MessageType.Error);
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
                        
                        AITarget.StartWithError = true;
                        AITarget.ErrorCombat = true;
                        AITarget.ErrorMovement = false;
                        AITarget.ErrorCombatMovement = false;
                    }
                }
                else if (AITarget.Combat.UseHitReactions && AITarget.Combat.HitReactionAnimations.Length <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Hit Reaction' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.UseHitReactions && AITarget.Combat.HitReactionAnimations[0].AnimationClip == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Hit Reaction' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.IsWeapon && AITarget.Combat.UseEquipAnimations && AITarget.Combat.EquipAnimation == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Equip' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.EquipAnimation != null && AITarget.Combat.IsWeapon && AITarget.Combat.UseEquipAnimations)
                {
                    // bool IKEventFound = false;
                    bool EquipEventFound = false;
                    if (AITarget.Combat.EquipAnimation.events.Length > 0)
                    {
                        foreach (var Event in AITarget.Combat.EquipAnimation.events)
                        {
                            if (Event.functionName == "EnableWeapon")
                            {
                                EquipEventFound = true;
                                // IKEventFound = true;
                            }
                            // if (Event.functionName == "EnableIK")
                            // {
                            //     IKEventFound = true;
                            // }
                        }
                    }

                    // if (!AITarget.Combat.IsShooterWeapon)
                    //     IKEventFound = true;
                    
                    if (AITarget.Combat.EquipAnimation.events.Length <= 0)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox("The 'EnableWeapon' event couldn't be found on the 'Equip Animation', please follow the documentation below!", MessageType.Error);
                        GUI.backgroundColor = Color.white;
 
                        GUILayout.Space(3);
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
 
                        if (GUILayout.Button("Open Documentation", GUILayout.Width(150),GUILayout.Height(25)))
                        {
                            Application.OpenURL("https://aidocs.darkingassets.com/ai-system-and-modules/weapon-equipping#setting-up-the-animations");
                        }
                
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                
                        EditorGUILayout.Space();
                        
                        AITarget.StartWithError = true;
                        AITarget.ErrorCombat = true;
                        AITarget.ErrorMovement = false;
                        AITarget.ErrorCombatMovement = false;
                    }
                    else if (!EquipEventFound)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox(
                            "The 'EnableWeapon' event couldn't be found on the 'Equip Animation', please follow the documentation below!",
                            MessageType.Error);
                        GUI.backgroundColor = Color.white;

                        GUILayout.Space(3);

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Open Documentation", GUILayout.Width(150), GUILayout.Height(25)))
                        {
                            Application.OpenURL(
                                "https://aidocs.darkingassets.com/ai-system-and-modules/weapon-equipping#setting-up-the-animations");
                        }

                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        AITarget.StartWithError = true;
                        AITarget.ErrorCombat = true;
                        AITarget.ErrorMovement = false;
                        AITarget.ErrorCombatMovement = false;
                    }
                }
                else if(AITarget.Combat.IsWeapon && AITarget.Combat.UseEquipAnimations && AITarget.Combat.UnEquipAnimation == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Un Equip' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.UnEquipAnimation != null && AITarget.Combat.IsWeapon && AITarget.Combat.UseEquipAnimations)
                {
                    bool EventFound = false;
                    if (AITarget.Combat.UnEquipAnimation.events.Length > 0)
                    {
                        foreach (var Event in AITarget.Combat.UnEquipAnimation.events)
                        {
                            if (Event.functionName == "DisableWeapon")
                            {
                                EventFound = true;
                                break;
                            }
                        }
                    }

                    if (!EventFound || AITarget.Combat.UnEquipAnimation.events.Length <= 0)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.HelpBox("The 'DisableWeapon' event couldn't be found on the 'Un Equip Animation', please follow the documentation below!", MessageType.Error);
                        GUI.backgroundColor = Color.white;
 
                        GUILayout.Space(3);
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
 
                        if (GUILayout.Button("Open Documentation", GUILayout.Width(150),GUILayout.Height(25)))
                        {
                            Application.OpenURL("https://aidocs.darkingassets.com/ai-system-and-modules/weapon-equipping#setting-up-the-animations");
                        }
                
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                
                        EditorGUILayout.Space();
                        
                        AITarget.StartWithError = true;
                        AITarget.ErrorCombat = true;
                        AITarget.ErrorMovement = false;
                        AITarget.ErrorCombatMovement = false;
                    }
                }
                else if (AITarget.Combat.IsWeapon && AITarget.Combat.UseBlockingSystem && AITarget.Combat.BlockAnimations.Length <= 0)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Block' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.IsWeapon && AITarget.Combat.UseBlockingSystem && AITarget.Combat.BlockAnimations[0].AnimationClip == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Block' animations are empty, please assign at least one animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else if(AITarget.Combat.IsWeapon && AITarget.Combat.IsShooterWeapon && AITarget.Combat.ReloadAnimation == null)
                {
                    EditorGUILayout.Space();
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("The 'Reload' animation is empty, please assign an animation!", MessageType.Error);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    AITarget.StartWithError = true;
                    AITarget.ErrorCombat = true;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                else
                {
                    AITarget.StartWithError = false;
                    AITarget.ErrorCombat = false;
                    AITarget.ErrorMovement = false;
                    AITarget.ErrorCombatMovement = false;
                }
                
                #endregion
            }
            
            
            AITarget.Open = true;
            
           
            if (AITarget.Open)
            {
                GUILayout.Space(12);
                AITarget.CurrentTab =
                    GUILayout.Toolbar(AITarget.CurrentTab, new string[] {"MOVEMENT", "COMBAT MOVEMENT", "COMBAT"});
                AITarget.OnValidate();
                if (AITarget.CurrentTab == 1)
                {
                    GUILayout.Space(6);
                    if (GUILayout.Button("COPY FROM MOVEMENT", GUILayout.Height(25)))
                    {
                        if (AITarget.ErrorMovement)
                        {
                            EditorUtility.DisplayDialog("WARNING!",
                                "There are some problems in your movement tab, please fix them first and try copying again after!",
                                "Okay");
                            return;
                        }
                        AITarget.CopyFromMovement();
                    }
                }
               
                GUILayout.Space(6);
            }
            
            GUILayout.Space(6);
            
        
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();

            if (AITarget.Open)
            {
#if UNITY_EDITOR
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(3);
                if (GUILayout.Button("RESET", GUILayout.Height(22),GUILayout.Width(90)))
                {
                    EditorGUI.BeginChangeCheck();
                    if (EditorUtility.DisplayDialog("WARNING!",
                            "Are you sure to reset animations you changed?", "Go Ahead", "Cancel"))
                    {
                        AITarget.Reset();
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Reset Animator");
                        }
                    }
                }


                GUILayout.Space(3);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Import", GUILayout.Height(22),GUILayout.Width(90)))
                {
                    EditorGUI.BeginChangeCheck();
                    if (EditorUtility.DisplayDialog("Select File!",
                            "You need to select your 'Animation Profile' that you exported before.", "Ok"))
                    {
                        string path = EditorUtility.OpenFilePanel("Import Profile", "", "");
                        if (File.Exists(path))
                        {
                            string newPath = path.Replace(Application.dataPath, "Assets/");
                            AnimationStorage storage = AssetDatabase.LoadAssetAtPath<AnimationStorage>(newPath);
                            
                            if(storage != null)
                                Import(storage);
                        }
                    }
                }
                if (GUILayout.Button("Export", GUILayout.Height(22),GUILayout.Width(90)))
                {
                    string FilePath = EditorUtility.SaveFilePanelInProject("Save as AnimationProfile",
                        "New AnimationProfile",
                        "asset", "Please enter a file name to save the file to");

                    if (FilePath != String.Empty)
                    {
                        AnimationStorage storagecreated = ScriptableObject.CreateInstance<AnimationStorage>();
                        Export(storagecreated);
                        AssetDatabase.CreateAsset(storagecreated, FilePath);
                        AssetDatabase.SaveAssets();
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(3);
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                EditorGUILayout.HelpBox(
                    "Do not forget to Create Controller!",
                    MessageType.Info);
                if (GUILayout.Button("CREATE CONTROLLER", GUILayout.Height(25)))
                {
                    if (AITarget.StartWithError)
                    {
                        EditorUtility.DisplayDialog("WARNING!",
                            "There are some problems in your animator creator setup, please fix the errors above the inspector, and try again!",
                            "Okay");
                        return;
                    }
                    string FilePath = EditorUtility.SaveFilePanelInProject("Save as OverrideController",
                        "New OverrideController",
                        "overrideController", "Please enter a file name to save the file to");

                    if (FilePath != string.Empty)
                    {
                        string UserFilePath = FilePath;
                        string SourceFilePath = AssetDatabase.GetAssetPath(Resources.Load(AITarget.Combat.IsWeapon ? "Animator Controllers/Universal AI Weapon Controller" : "Animator Controllers/Universal AI Controller"));
                        
                        AssetDatabase.CopyAsset(SourceFilePath, UserFilePath);
                        RuntimeAnimatorController runtimeAnimatorController = AssetDatabase.LoadAssetAtPath(UserFilePath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
                        serializedObject.Update();
                        EditorUtility.SetDirty(AITarget);
                        AITarget.Apply(runtimeAnimatorController,UserFilePath);
                    }
                }
             
#endif
            }
        }
#if UNITY_EDITOR
        public void Export(AnimationStorage storage)
        {
            storage.Idles = AITarget.Movement.Idles;
            storage.WalkForward = AITarget.Movement.WalkForward;
            storage.ForwardSpeed = AITarget.Movement.ForwardSpeed;
            storage.MirrorForwardAnimation = AITarget.Movement.MirrorForwardAnimation;
            storage.WalkLeft = AITarget.Movement.WalkLeft;
            storage.LeftSpeed = AITarget.Movement.LeftSpeed;
            storage.MirrorLeftAnimation = AITarget.Movement.MirrorLeftAnimation;
            storage.WalkRight = AITarget.Movement.WalkRight;
            storage.RightSpeed = AITarget.Movement.RightSpeed;
            storage.MirrorRightAnimation = AITarget.Movement.MirrorRightAnimation;
            storage.WalkBackwards = AITarget.Movement.WalkBackwards;
            storage.BackwardsSpeed = AITarget.Movement.BackwardsSpeed;
            storage.MirrorBackwardsAnimation = AITarget.Movement.MirrorBackwardsAnimation;
            storage.RunForward = AITarget.Movement.RunForward;
            storage.RunForwardSpeed = AITarget.Movement.RunForwardSpeed;
            storage.MirrorRunForwardAnimation = AITarget.Movement.MirrorRunForwardAnimation;
            storage.RunLeft = AITarget.Movement.RunLeft;
            storage.RunLeftSpeed = AITarget.Movement.RunLeftSpeed;
            storage.MirrorRunLeftAnimation = AITarget.Movement.MirrorRunLeftAnimation;
            storage.RunRight = AITarget.Movement.RunRight;
            storage.RunRightSpeed = AITarget.Movement.RunRightSpeed;
            storage.MirrorRunRightAnimation = AITarget.Movement.MirrorRunRightAnimation;
            storage.UseTurnAnimations = AITarget.Movement.UseTurnAnimations;
            storage.TurnLeft = AITarget.Movement.TurnLeft;
            storage.TurnLeftSpeed = AITarget.Movement.TurnLeftSpeed;
            storage.MirrorTurnLeft = AITarget.Movement.MirrorTurnLeft;
            storage.TurnRight = AITarget.Movement.TurnRight;
            storage.TurnRightSpeed = AITarget.Movement.TurnRightSpeed;
            storage.MirrorTurnRight= AITarget.Movement.MirrorTurnRight;
            
            
            storage.CombatIdles = AITarget.CombatMovement.CombatIdles;
            storage.CombatWalkForward = AITarget.CombatMovement.CombatWalkForward;
            storage.WalkForwardSpeed = AITarget.CombatMovement.WalkForwardSpeed;
            storage.MirrorCombatForward = AITarget.CombatMovement.MirrorCombatForward;
            storage.CombatWalkLeft = AITarget.CombatMovement.CombatWalkLeft;
            storage.WalkLeftSpeed = AITarget.CombatMovement.WalkLeftSpeed;
            storage.MirrorCombatLeft = AITarget.CombatMovement.MirrorCombatLeft;
            storage.CombatWalkRight = AITarget.CombatMovement.CombatWalkRight;
            storage.WalkRightSpeed = AITarget.CombatMovement.WalkRightSpeed;
            storage.MirrorCombatRight = AITarget.CombatMovement.MirrorCombatRight;
            storage.CombatWalkBackwards = AITarget.CombatMovement.CombatWalkBackwards;
            storage.WalkBackwardsSpeed = AITarget.CombatMovement.WalkBackwardsSpeed;
            storage.MirrorCombatBackwards = AITarget.CombatMovement.MirrorCombatBackwards;
            storage.CombatRunForward = AITarget.CombatMovement.CombatRunForward;
            storage.CombatRunForwardSpeed = AITarget.CombatMovement.RunForwardSpeed;
            storage.MirrorCombatRunForward = AITarget.CombatMovement.MirrorCombatRunForward;
            storage.CombatRunLeft = AITarget.CombatMovement.CombatRunLeft;
            storage.CombatRunLeftSpeed = AITarget.CombatMovement.RunLeftSpeed;
            storage.MirrorRunCombatLeft = AITarget.CombatMovement.MirrorRunCombatLeft;
            storage.CombatRunRight = AITarget.CombatMovement.CombatRunRight;
            storage.CombatRunRightSpeed = AITarget.CombatMovement.RunRightSpeed;
            storage.MirrorRunCombatRight = AITarget.CombatMovement.MirrorRunCombatRight;
            storage.UseCombatTurnAnimations = AITarget.CombatMovement.UseCombatTurnAnimations;
            storage.CombatTurnLeft = AITarget.CombatMovement.CombatTurnLeft;
            storage.CombatTurnLeftSpeed = AITarget.CombatMovement.CombatTurnLeftSpeed;
            storage.MirrorCombatTurnLeft = AITarget.CombatMovement.MirrorCombatTurnLeft;
            storage.CombatTurnRight = AITarget.CombatMovement.CombatTurnRight;
            storage.CombatTurnRightSpeed = AITarget.CombatMovement.CombatTurnRightSpeed;
            storage.MirrorCombatTurnRight= AITarget.CombatMovement.MirrorCombatTurnRight;

            
            storage.AttackAnimations = AITarget.Combat.AttackAnimations;
            storage.UseHitReactions = AITarget.Combat.UseHitReactions;
            storage.HitReactionAnimations = AITarget.Combat.HitReactionAnimations;
            storage.IsWeapon = AITarget.Combat.IsWeapon;
            storage.UseEquipAnimations = AITarget.Combat.UseEquipAnimations;
            storage.EquipAnimation = AITarget.Combat.EquipAnimation;
            storage.EquipAnimationSpeed = AITarget.Combat.EquipAnimationSpeed;
            storage.UnEquipAnimation = AITarget.Combat.UnEquipAnimation;
            storage.UnEquipAnimationSpeed = AITarget.Combat.UnEquipAnimationSpeed;
            storage.UseBlockingSystem = AITarget.Combat.UseBlockingSystem;
            storage.BlockAnimations = AITarget.Combat.BlockAnimations;
            storage.IsShooterWeapon = AITarget.Combat.IsShooterWeapon;
            storage.ReloadAnimation = AITarget.Combat.ReloadAnimation;
            storage.ReloadAnimationSpeed = AITarget.Combat.ReloadAnimationSpeed;
            storage.DeathAnimations = AITarget.Combat.DeathAnimations;

        }
        
        public void Import(AnimationStorage storage)
        {
            AITarget.Movement.Idles = storage.Idles;
            AITarget.Movement.WalkForward = storage.WalkForward;
            AITarget.Movement.ForwardSpeed = storage.ForwardSpeed;
            AITarget.Movement.MirrorForwardAnimation = storage.MirrorForwardAnimation;
            AITarget.Movement.WalkLeft = storage.WalkLeft;
            AITarget.Movement.LeftSpeed = storage.LeftSpeed;
            AITarget.Movement.MirrorLeftAnimation = storage.MirrorLeftAnimation;
            AITarget.Movement.WalkRight = storage.WalkRight;
            AITarget.Movement.RightSpeed = storage.RightSpeed;
            AITarget.Movement.MirrorRightAnimation = storage.MirrorRightAnimation;
            AITarget.Movement.WalkBackwards = storage.WalkBackwards;
            AITarget.Movement.BackwardsSpeed = storage.BackwardsSpeed;
            AITarget.Movement.MirrorBackwardsAnimation= storage.MirrorBackwardsAnimation;
            AITarget.Movement.RunForward = storage.RunForward;
            AITarget.Movement.RunForwardSpeed = storage.RunForwardSpeed;
            AITarget.Movement.MirrorRunForwardAnimation= storage.MirrorRunForwardAnimation;
            AITarget.Movement.RunLeft = storage.RunLeft;
            AITarget.Movement.RunLeftSpeed = storage.RunLeftSpeed;
            AITarget.Movement.MirrorRunLeftAnimation = storage.MirrorRunLeftAnimation;
            AITarget.Movement.RunRight = storage.RunRight;
            AITarget.Movement.RunRightSpeed = storage.RunRightSpeed;
            AITarget.Movement.MirrorRunRightAnimation = storage.MirrorRunRightAnimation;
            AITarget.Movement.UseTurnAnimations = storage.UseTurnAnimations;
            AITarget.Movement.TurnLeft = storage.TurnLeft;
            AITarget.Movement.TurnLeftSpeed = storage.TurnLeftSpeed;
            AITarget.Movement.MirrorTurnLeft = storage.MirrorTurnLeft;
            AITarget.Movement.TurnRight = storage.TurnRight;
            AITarget.Movement.TurnRightSpeed = storage.TurnRightSpeed;
            AITarget.Movement.MirrorTurnRight = storage.MirrorTurnRight;
            
            
            AITarget.CombatMovement.CombatIdles = storage.CombatIdles;
            AITarget.CombatMovement.CombatWalkForward = storage.CombatWalkForward;
            AITarget.CombatMovement.WalkForwardSpeed = storage.WalkForwardSpeed;
            AITarget.CombatMovement.MirrorCombatForward = storage.MirrorCombatForward;
            AITarget.CombatMovement.CombatWalkLeft = storage.CombatWalkLeft;
            AITarget.CombatMovement.WalkLeftSpeed = storage.WalkLeftSpeed;
            AITarget.CombatMovement.MirrorCombatLeft = storage.MirrorCombatLeft;
            AITarget.CombatMovement.CombatWalkRight = storage.CombatWalkRight;
            AITarget.CombatMovement.WalkRightSpeed = storage.WalkRightSpeed;
            AITarget.CombatMovement.MirrorCombatRight = storage.MirrorCombatRight;
            AITarget.CombatMovement.CombatWalkBackwards = storage.CombatWalkBackwards;
            AITarget.CombatMovement.WalkBackwardsSpeed = storage.WalkBackwardsSpeed;
            AITarget.CombatMovement.MirrorCombatBackwards =  storage.MirrorCombatBackwards;
            AITarget.CombatMovement.CombatRunForward = storage.CombatRunForward;
            AITarget.CombatMovement.RunForwardSpeed = storage.CombatRunForwardSpeed;
            AITarget.CombatMovement.MirrorCombatRunForward = storage.MirrorCombatRunForward;
            AITarget.CombatMovement.CombatRunLeft = storage.CombatRunLeft;
            AITarget.CombatMovement.RunLeftSpeed = storage.CombatRunLeftSpeed;
            AITarget.CombatMovement.MirrorRunCombatLeft = storage.MirrorRunCombatLeft;
            AITarget.CombatMovement.CombatRunRight = storage.CombatRunRight;
            AITarget.CombatMovement.RunRightSpeed = storage.CombatRunRightSpeed;
            AITarget.CombatMovement.MirrorRunCombatRight = storage.MirrorRunCombatRight;
            AITarget.CombatMovement.UseCombatTurnAnimations = storage.UseCombatTurnAnimations;
            AITarget.CombatMovement.CombatTurnLeft = storage.CombatTurnLeft;
            AITarget.CombatMovement.CombatTurnLeftSpeed = storage.CombatTurnLeftSpeed;
            AITarget.CombatMovement.MirrorCombatTurnLeft = storage.MirrorCombatTurnLeft;
            AITarget.CombatMovement.CombatTurnRight = storage.CombatTurnRight;
            AITarget.CombatMovement.CombatTurnRightSpeed = storage.CombatTurnRightSpeed;
            AITarget.CombatMovement.MirrorCombatTurnRight = storage.MirrorCombatTurnRight;

            
            AITarget.Combat.AttackAnimations = storage.AttackAnimations;
            AITarget.Combat.UseHitReactions = storage.UseHitReactions;
            AITarget.Combat.HitReactionAnimations = storage.HitReactionAnimations;
            AITarget.Combat.IsWeapon = storage.IsWeapon;
            AITarget.Combat.UseEquipAnimations = storage.UseEquipAnimations;
            AITarget.Combat.EquipAnimation = storage.EquipAnimation;
            AITarget.Combat.EquipAnimationSpeed = storage.EquipAnimationSpeed;
            AITarget.Combat.UnEquipAnimation = storage.UnEquipAnimation;
            AITarget.Combat.UnEquipAnimationSpeed = storage.UnEquipAnimationSpeed;
            AITarget.Combat.UseBlockingSystem = storage.UseBlockingSystem;
            AITarget.Combat.BlockAnimations = storage.BlockAnimations;
            AITarget.Combat.IsShooterWeapon = storage.IsShooterWeapon;
            AITarget.Combat.ReloadAnimation = storage.ReloadAnimation;
            AITarget.Combat.ReloadAnimationSpeed = storage.ReloadAnimationSpeed;
            AITarget.Combat.DeathAnimations = storage.DeathAnimations;

        }
#endif
    }
}