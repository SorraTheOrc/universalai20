
using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if INTEGRATION_FPV2NEWER && UniversalAI_Integration_MMFPSE
using MarsFPSKit;
#endif

// #if Universal_Shooter_Kit
// using GercStudio.USK.Scripts;
// #endif


#if HQ_FPS_TEMPLATE && UniversalAI_Integration_HQFPS
using HQFPSTemplate;
#endif

#if SURVIVAL_TEMPLATE_PRO && UniversalAI_Integration_STP
using SurvivalTemplatePro;
#endif

#if (INVECTOR_SHOOTER || INVECTOR_MELEE) && UniversalAI_Integration_INVECTOR
using Invector;
#endif


#if (FIRST_PERSON_CONTROLLER || THIRD_PERSON_CONTROLLER) && UniversalAI_Integration_OPSIVE
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Traits.Damage;
#endif

#if NEOFPS && UniversalAI_Integration_NEOFPS
using NeoFPS;
#endif

#if UniversalAI_Integration_HORRORFPS
using HFPS.Player;
#endif

namespace UniversalAI
{

    
    [Serializable]
    public class ThePrivateDamageEvent : UnityEvent<float, GameObject> {}

public class UniversalAIPlayerReference : MonoBehaviour, UniversalAIDamageable
{
    public Transform AimPosition = null;
    [Space] [Space] [Help("The Transform Enemies Will Aim To ", HelpBoxMessageType.Info)] 
    
    [Space]
    [Header("DAMAGE SETTINGS")]
    [Space]
    
    public UniversalAIEnums.YesNo PlayerCanReceiveDamage = UniversalAIEnums.YesNo.Yes;
    [Space] [Space] [Help("Can The Player Receive Damages ?", HelpBoxMessageType.Info)]
    
    public float DamageMultiplier = 1f;
    [Space] [Space] [Help("The Received Damage Multiplier !", HelpBoxMessageType.Info)]
    
    [Space]
    [Header("FACTION SETTINGS")]
    [Space]

    public UniversalAIEnums.Factions PlayerFaction = UniversalAIEnums.Factions.Player;
    
    [Space] 
    [Header("EVENTS")] 
    [Space]

    public TheFloatEvent OnTakeDamageEvent = new TheFloatEvent();
    [Space] [Space] [Help("The Take Damage Event !", HelpBoxMessageType.Info)] 
    
    public UnityEvent OnDeathEvent = new UnityEvent();
    [Space] [Space] [Help("The Death Event !", HelpBoxMessageType.Info)]

    [HideInInspector] public ThePrivateDamageEvent OnTakeDamageCheck = new ThePrivateDamageEvent();
    
    [Space]
    
    [UniversalAI.ReadOnly]
    public float CurrentHealth = 100f;
    

#if INTEGRATION_FPV2NEWER && UniversalAI_Integration_MMFPSE
    private Kit_PlayerHUD _mmfpseHealthGetter = null;
#endif

    private void OnValidate()
    {
        if (AimPosition == null)
        {
            AimPosition = transform;
        }
    }

    private void LateUpdate()
    {
        if(!gameObject.activeInHierarchy)
            return;

        #region Ready Integrations

        #if HQ_FPS_TEMPLATE && UniversalAI_Integration_HQFPS
        Player Player = GetComponent<Player>();
        if (Player != null)
        {
            CurrentHealth = Player.Health.Val;
        }
#endif
        
#if SURVIVAL_TEMPLATE_PRO && UniversalAI_Integration_STP
        Character Character = GetComponent<Character>();
        if (Character != null)
        {
            CurrentHealth = Character.HealthManager.Health;
        }
#endif
        
#if (FIRST_PERSON_CONTROLLER || THIRD_PERSON_CONTROLLER) && UniversalAI_Integration_OPSIVE
        Health Playerh = GetComponent<Health>();
        if (Playerh != null)
        {
            CurrentHealth = Playerh.Value;
        }
#endif
        
#if UniversalAI_Integration_GKC
        health healthManagement = GetComponent<health>();
        if (healthManagement != null)
        {
            CurrentHealth = healthManagement.getCurrentHealthAmount();
        }
#endif
        
#if NEOFPS && UniversalAI_Integration_NEOFPS
        BasicHealthManager health = GetComponent<BasicHealthManager>();
        if (health != null)
        {
            CurrentHealth = health.health;
        }
#endif
        
#if (INVECTOR_SHOOTER || INVECTOR_MELEE) && UniversalAI_Integration_INVECTOR
        
        vHealthController healthcontroller = GetComponent<vHealthController>();
        if (healthcontroller != null)
        {
            CurrentHealth = healthcontroller.currentHealth;
        }
        
#endif
        
#if UniversalAI_Integration_HORRORFPS

        HealthManager healthManagers = GetComponent<HealthManager>();
        if (healthManagers != null)
        {
            CurrentHealth = healthManagers.Health;
        }
        
#endif
        
#if INTEGRATION_FPV2NEWER && UniversalAI_Integration_MMFPSE

        if (_mmfpseHealthGetter == null)
        {
            if(FindObjectOfType<Kit_PlayerHUD>() != null)
            {
                _mmfpseHealthGetter = FindObjectOfType<Kit_PlayerHUD>();
            }
            else
            {
                Debug.LogError("The 'Player Hud' for MMFPSE couldn't be found, Player damage won't work!");
            }
        }
        else
        {
            CurrentHealth = int.Parse(_mmfpseHealthGetter.healthText.text); 
        }

#endif

        #endregion

    }

    public void TakeDamage(float damageamount, AttackerType attackerType, GameObject Attacker, bool BlockSuccess = true)
    {
        if(PlayerCanReceiveDamage == UniversalAIEnums.YesNo.No)
            return;
        
        if (CurrentHealth <= 0)
            return;
        
        float Damage = damageamount * DamageMultiplier;
        CurrentHealth -= Damage;
        
        OnTakeDamageCheck.Invoke(Damage, Attacker);
        OnTakeDamageEvent.Invoke(Damage);

        if (CurrentHealth <= 0)
        {
            OnDeathEvent.Invoke();
        }
        
        #region Ready Integrations

        #if HQ_FPS_TEMPLATE && UniversalAI_Integration_HQFPS
        Player Player = GetComponent<Player>();
        if (Player != null)
        {
            Player.ChangeHealth.Try(new DamageInfo(-Damage));
        }
#endif
        
#if SURVIVAL_TEMPLATE_PRO && UniversalAI_Integration_STP
        Character Character = GetComponent<Character>();
        if (Character != null)
        {
           Character.HealthManager.ReceiveDamage(new DamageInfo(-Damage));
        }
#endif
        
#if (FIRST_PERSON_CONTROLLER || THIRD_PERSON_CONTROLLER) && UniversalAI_Integration_OPSIVE
        Health Playerh = GetComponent<Health>() != null ? GetComponent<Health>() : GetComponentInParent<Health>();
        if (Playerh != null)
        {
            Playerh.Damage(new DamageData
            {
                Amount = Damage
            });
        }
#endif
      
#if UniversalAI_Integration_HORRORFPS

        HealthManager healthManagers = GetComponent<HealthManager>();
        if (healthManagers != null)
        {
            healthManagers.ApplyDamage((int)Damage);
        }
        
#endif
        
#if NEOFPS && UniversalAI_Integration_NEOFPS

        BasicHealthManager health = GetComponent<BasicHealthManager>();
        
        if(health != null)
            health.AddDamage(Damage);

#endif

#if INTEGRATION_FPV2NEWER && UniversalAI_Integration_MMFPSE

        Kit_PlayerBehaviour player = GetComponent<Kit_PlayerBehaviour>();
        
        if(player != null)
            player.ApplyDamageNetwork(Damage, false, 0, 0, Vector3.zero, Vector3.zero, 100f, Vector3.zero, 0);

#endif

#if UniversalAI_Integration_GKC

        health healthManagement = GetComponent<health>();
        
        if(healthManagement != null)
            healthManagement.setDamage(Damage, Vector3.zero,transform.position , Attacker, null, true, true, true, false,  false, true, 0, 0);

#endif
        
#if (INVECTOR_SHOOTER || INVECTOR_MELEE) && UniversalAI_Integration_INVECTOR
        
        vHealthController healthcontroller = GetComponent<vHealthController>();
        if (healthcontroller != null)
        {
            healthcontroller.TakeDamage(new vDamage
            {
                damageValue = (int) Damage
            });
        }
        
#endif

        #endregion
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UniversalAIPlayerReference))]
public class UniversalAIPlayerReferenceEditor : Editor
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
        EditorGUILayout.LabelField("Universal AI Player", style, GUILayout.ExpandWidth(true));
            
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