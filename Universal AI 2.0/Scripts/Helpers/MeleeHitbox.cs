using System;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{
    
    [RequireComponent(typeof(UniversalAIMeleeWeapon))]
    [RequireComponent(typeof(BoxCollider))]
public class MeleeHitbox : MonoBehaviour
{
    [Space] 
    [HideInInspector] public UniversalAIMeleeWeapon MeleeWeapon;
    
    [Space] 
    public UniversalAIEnums.YesNo Enabled = UniversalAIEnums.YesNo.Yes;

    [HideInInspector]  public bool CanHit = true;
    
    [HideInInspector]  public bool CanDamage = true;
    private void Awake()
    {
        MeleeWeapon = GetComponent<UniversalAIMeleeWeapon>();
    }

    private void Update()
    {
        if (MeleeWeapon != null && MeleeWeapon.WeaponSettings != null && MeleeWeapon.WeaponSettings.AISystem != null &&
            MeleeWeapon.WeaponSettings.AISystem.Anim != null)
        {
            CanHit = MeleeWeapon.WeaponSettings.AISystem.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !MeleeWeapon.WeaponSettings.AISystem.GotHit;
        }

        if (!CanHit)
            CanDamage = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Enabled == UniversalAIEnums.YesNo.No)
            return;
        
        if(!CanHit)
            return;
        
        if(!CanDamage)
            return;

        if(MeleeWeapon.WeaponSettings.AISystem.Frozen || MeleeWeapon.WeaponSettings.AISystem.StopAIBehaviour)
            return;
        
        if(!MeleeWeapon.WeaponSettings.AISystem.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            return;

        if (other.GetComponent<UniversalAISystem>() || other.GetComponent<UniversalAIPlayerReference>())
        {
            
            var AISystem = other.GetComponent<UniversalAISystem>();
            var Player = other.GetComponent<UniversalAIPlayerReference>();

            int damage = Random.Range(MeleeWeapon.WeaponSettings.MinDamage, MeleeWeapon.WeaponSettings.MaxDamage);
        
            if (AISystem != null)
            {
                if(AISystem == MeleeWeapon.WeaponSettings.AISystem)
                    return;

                bool BlockSuccess = AISystem.IsBlocking &&
                                    Random.Range(0, 101) <= AISystem.Settings.Attack.BlockSuccessfulChance && AISystem.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Block");
                
                if (AISystem.IsBlocking && BlockSuccess)
                {
                    MeleeWeapon.WeaponEvents.OnHitBlocked.Invoke();
                    MeleeWeapon.PlaySound(WeaponAudioType.Hit);
                    GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.AttackBlockedEffect.ImpactEffect,
                        transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                    if (MeleeWeapon.WeaponSettings.AttackBlockedEffect.SetParent == UniversalAIMeleeWeapon.blockmpact.setParent.Null)
                    {
                        Effect.transform.SetParent(null);
                    }
                    else
                    {
                        Effect.transform.SetParent(other.transform);
                    }
                }
                else
                {
                    MeleeWeapon.PlaySound(WeaponAudioType.Damaged);
                    if (MeleeWeapon.WeaponSettings.ImpactEffects.Count > 0)
                    {
                        if (MeleeWeapon.WeaponSettings.ImpactEffects.Count < 2)
                        {
                            GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                                transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                            if (MeleeWeapon.WeaponSettings.ImpactEffects[0].SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                            {
                                Effect.transform.SetParent(null);
                            }
                            else
                            {
                                Effect.transform.SetParent(other.transform);
                            }
                        }
                        else
                        {
                            foreach (var effect in MeleeWeapon.WeaponSettings.ImpactEffects)
                            {
                                if (other.transform.tag.Equals(effect.ObjectTag))
                                {
                                    GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                                        transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                                    if (effect.SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                                    {
                                        Effect.transform.SetParent(null);
                                    }
                                    else
                                    {
                                        Effect.transform.SetParent(other.transform);
                                    }
                            
                                    break;
                                }
                            }   
                        }
                    }
                }
                
                if (BlockSuccess)
                    damage = 0;
                
                AISystem.TakeDamage(damage, AttackerType.AI, MeleeWeapon.WeaponSettings.AISystem.gameObject, BlockSuccess);
                MeleeWeapon.WeaponEvents.OnDealDamage.Invoke(damage);
                CanDamage = false;
            }
            else if (Player != null)
            {
                Player.TakeDamage(damage, AttackerType.AI, MeleeWeapon.WeaponSettings.AISystem.gameObject);
                CanDamage = false;
                MeleeWeapon.WeaponEvents.OnDealDamage.Invoke(damage);
                MeleeWeapon.PlaySound(WeaponAudioType.Damaged);
                if (MeleeWeapon.WeaponSettings.ImpactEffects.Count > 0)
                {
                    if (MeleeWeapon.WeaponSettings.ImpactEffects.Count < 2)
                    {
                        GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                            transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                        if (MeleeWeapon.WeaponSettings.ImpactEffects[0].SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                        {
                            Effect.transform.SetParent(null);
                        }
                        else
                        {
                            Effect.transform.SetParent(other.transform);
                        }
                    }
                    else
                    {
                        foreach (var effect in MeleeWeapon.WeaponSettings.ImpactEffects)
                        {
                            if (other.transform.tag.Equals(effect.ObjectTag))
                            {
                                GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                                    transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                                if (effect.SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                                {
                                    Effect.transform.SetParent(null);
                                }
                                else
                                {
                                    Effect.transform.SetParent(other.transform);
                                }
                            
                                break;
                            }
                        }   
                    }
                }
            }
            else
            {
                MeleeWeapon.PlaySound(WeaponAudioType.Hit);
                if (MeleeWeapon.WeaponSettings.ImpactEffects.Count > 0)
                {
                    if (MeleeWeapon.WeaponSettings.ImpactEffects.Count < 2)
                    {
                        GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                            transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                        if (MeleeWeapon.WeaponSettings.ImpactEffects[0].SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                        {
                            Effect.transform.SetParent(null);
                        }
                        else
                        {
                            Effect.transform.SetParent(other.transform);
                        }
                    }
                    else
                    {
                        foreach (var effect in MeleeWeapon.WeaponSettings.ImpactEffects)
                        {
                            if (other.transform.tag.Equals(effect.ObjectTag))
                            {
                                GameObject Effect = Instantiate(MeleeWeapon.WeaponSettings.ImpactEffects[0].ImpactEffect,
                                    transform.position, Quaternion.FromToRotation(Vector3.forward, transform.position));

                                if (effect.SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                                {
                                    Effect.transform.SetParent(null);
                                }
                                else
                                {
                                    Effect.transform.SetParent(other.transform);
                                }
                            
                                break;
                            }
                        }   
                    }
                }
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(MeleeHitbox))]
public class GenericAIHitboxEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
        style.fontSize = 13;
        EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
        EditorGUILayout.LabelField("Universal AI Hitbox", style, GUILayout.ExpandWidth(true));
            
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
            
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
    
}