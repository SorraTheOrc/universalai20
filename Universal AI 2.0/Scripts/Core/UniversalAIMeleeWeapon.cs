using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UniversalAI
{
    
    [RequireComponent(typeof(Rigidbody))]
public class UniversalAIMeleeWeapon : MonoBehaviour
{
    
            
    [Group("Open ")] public weaponSettings WeaponSettings;

    [Serializable]
    public class weaponSettings
    {
        [Header("SETTINGS")]
        [Space] 
        [Help("You Can Edit Your 'Melee Weapon' Here.",HelpBoxMessageType.BigInfo)] 
        [Space]

        [ReadOnly] public UniversalAISystem AISystem = null;
        [Space] [Space] [Help("The AI System.", HelpBoxMessageType.Info)]
    
        public UniversalAIEnums.YesNo AlwaysEquipped = UniversalAIEnums.YesNo.No;
        [Space] [Space] [Help("Is The Weapon Is Always Equipped ?", HelpBoxMessageType.Info)]
        
        public float WeaponAttackDistance = 2f;
        [Space] [Space] [Help("The Attack Distance Of The AI For Start Attacking To The Target.", HelpBoxMessageType.Info)]
        public int MinDamage = 15;
        [Space] [Space] [Help("The Weapon's Minimum Damage.", HelpBoxMessageType.Info)]
    
        public int MaxDamage = 30;
        [Space] [Space] [Help("The Weapon's Maximum Damage.", HelpBoxMessageType.Info)]

        public blockmpact AttackBlockedEffect;
        [Space] [Space] [Help("The Impact Particle Effects To Be Played On Blocked.", HelpBoxMessageType.Info)] 

        public List<impactList> ImpactEffects = new List<impactList>();
        [Space] [Space] [Help("The Impact Particle Effects To Be Played.", HelpBoxMessageType.Info)] 
    
        [ReadOnly]
        public MeleeHitbox Hitbox;
    }

    [Serializable]
    public class impactList
    {
        public enum setParent
        {
            Null,
            HitObject,
        }
        
        [TagSelector]
        public string ObjectTag;
        public setParent SetParent = setParent.HitObject;
        public GameObject ImpactEffect;
    }
    
    [Serializable]
    public class blockmpact
    {
        public enum setParent
        {
            Null,
            HitObject,
        }
        public setParent SetParent = setParent.HitObject;
        public GameObject ImpactEffect;
    }

    [Space]
    
    [Group("Open ")] public weaponObjectSettings WeaponObjectSettings;

    [Serializable]
    public class weaponObjectSettings
    {
        [Header("WEAPON OBJECT SETTINGS")]
        [Space]
        [Help("This part is for making your Weapon Object Enabled When You Equip A Weapon, You Can Ignore This Part If Your Weapon Is Equipped Always", HelpBoxMessageType.BigInfo)]
        [Space]

        public List<Renderer> HolsteredWeaponObject;
        [Space] [Space] [Help("The Weapon Renderer(s) That Will Be Enabled When The Weapon Is Holstered.", HelpBoxMessageType.Info)]
                
        public List<Renderer> MainWeaponObject;
        [Space] [Space] [Help("The Weapon Renderer(s) That Will Be Enabled When The Weapon Is Equipped.", HelpBoxMessageType.Info)]
        
        [UniversalAI.ReadOnly]
        public string WeaponType = "Melee";
    }
    
    [Space]

    [Group("Open ")] public weaponSounds WeaponSounds;

    [Serializable]
    public class weaponSounds
    {
        [Header("SOUNDS")]
        [Space] 
        [Help("Weapon Sounds Part Allows You To Control The Weapon's Sounds", HelpBoxMessageType.BigInfo)]
        [Space]

        public AudioClip HitSoundEffect = null;
        [Range(0f,1f)]
        public float HitSoundVolume = 1f;
        [Space] [Space] [Help("The Hit Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
        public AudioClip DamagedSoundEffect = null;
        [Range(0f,1f)]
        public float DamagedSoundVolume = 1f;
        [Space] [Space] [Help("The Damaged Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
        public AudioClip EquipSoundEffect = null;
        [Range(0f,1f)]
        public float EquipSoundVolume = 1f;
        [Space] [Space] [Help("The Equip Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
        public AudioClip UnEquipSoundEffect = null;
        [Range(0f,1f)]
        public float UnEquipSoundVolume = 1f;
        [Space] [Space] [Help("The Un Equip Sound Effect Audio.", HelpBoxMessageType.Info)]
                    
        [UniversalAI.ReadOnly] public AudioSource WeaponAudioSource;
    }
    
    [Space]

    [Group("Open ")] public weaponEvents WeaponEvents;

    [Serializable]
    public class weaponEvents
    {
        [Header("EVENTS")]
        [Space] 
        [Help("Weapon Events Part Allows You To Control The Weapon's Events", HelpBoxMessageType.BigInfo)]
        [Space]

        public TheFloatEvent OnDealDamage = new TheFloatEvent();
        [Space] [Space] [Help("Plays After The Weapon Damages A Target.",HelpBoxMessageType.Info)]
    
        public UnityEvent OnHitBlocked = new UnityEvent();
    }
    
    public void PlaySound(WeaponAudioType weaponAudioType)
    {
        if (weaponAudioType == WeaponAudioType.Hit && WeaponSounds.HitSoundEffect != null)
        {
            WeaponSounds.WeaponAudioSource.volume = WeaponSounds.HitSoundVolume;
            WeaponSounds.WeaponAudioSource.PlayOneShot(WeaponSounds.HitSoundEffect);
        }
            
        if (weaponAudioType == WeaponAudioType.Damaged && WeaponSounds.DamagedSoundEffect != null)
        {
            WeaponSounds.WeaponAudioSource.volume = WeaponSounds.DamagedSoundVolume;
            WeaponSounds.WeaponAudioSource.PlayOneShot(WeaponSounds.DamagedSoundEffect);
        }
        
        if (weaponAudioType == WeaponAudioType.Equip && WeaponSounds.EquipSoundEffect != null)
        {
            WeaponSounds.WeaponAudioSource.volume = WeaponSounds.EquipSoundVolume;
            WeaponSounds.WeaponAudioSource.PlayOneShot(WeaponSounds.EquipSoundEffect);
        }
        
        if (weaponAudioType == WeaponAudioType.UnEquip && WeaponSounds.UnEquipSoundEffect != null)
        {
            WeaponSounds.WeaponAudioSource.volume = WeaponSounds.UnEquipSoundVolume;
            WeaponSounds.WeaponAudioSource.PlayOneShot(WeaponSounds.UnEquipSoundEffect);
        }
    }
    
    
    public void OnValidate()
    {
    
        if(WeaponSettings == null)
        {
            WeaponSettings = new weaponSettings();
        }
        
        if (WeaponSettings.Hitbox == null)
        {
            if (GetComponent<MeleeHitbox>() != null)
            {
                WeaponSettings.Hitbox = GetComponent<MeleeHitbox>();
            }
            else
            {
                WeaponSettings.Hitbox = gameObject.AddComponent<MeleeHitbox>();
            }
        }
        
        if (WeaponSettings.AISystem == null)
        {
            Transform parentt = transform.parent;

            while (parentt.GetComponent<UniversalAISystem>() == null)
            {
                parentt = parentt.parent;
            }

            WeaponSettings.AISystem = parentt.GetComponent<UniversalAISystem>();
        }
        
        if (WeaponSettings.AISystem != null)
        {
            WeaponSettings.AISystem.Settings.Attack.AttackDistance = WeaponSettings.WeaponAttackDistance;
            WeaponSettings.AISystem.IsWeapon = true;
            WeaponSettings.AISystem.IsShooter = false;
        }
        
        if (WeaponSounds.WeaponAudioSource == null)
        {
            if (GetComponent<AudioSource>() != null)
            {
                WeaponSounds.WeaponAudioSource = GetComponent<AudioSource>();
            }
            else
            {
                WeaponSounds.WeaponAudioSource = gameObject.AddComponent<AudioSource>();
            }

            WeaponSounds.WeaponAudioSource.playOnAwake = false;
        }
    }

    private void OnWeaponStateChanged(bool enable, bool first = false)
    {
        if(WeaponSettings.AlwaysEquipped == UniversalAIEnums.YesNo.Yes)
            return;

        if (!first)
        {
            if (enable)
            {
                PlaySound(WeaponAudioType.Equip);
            }
            else
            {
                PlaySound(WeaponAudioType.UnEquip);
            }    
        }
        
        foreach (var renderer in WeaponObjectSettings.MainWeaponObject)
        {
            renderer.enabled = enable;
        }
            
        foreach (var renderer in WeaponObjectSettings.HolsteredWeaponObject)
        {
            renderer.enabled = !enable;
        }
    }
    
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        WeaponSettings.AISystem.UniversalAIEvents.OnDeath.AddListener(OnDeath);
        WeaponSettings.AISystem.WeaponStateEvent.AddListener(OnWeaponStateChanged);
        
        if(WeaponSettings.AISystem.IsEquipping == false)
        {
            WeaponSettings.AlwaysEquipped = UniversalAIEnums.YesNo.Yes;
        }
        
        OnWeaponStateChanged(WeaponSettings.AlwaysEquipped == UniversalAIEnums.YesNo.Yes, true);
        WeaponSettings.AISystem.AlwaysEquipped = WeaponSettings.AlwaysEquipped == UniversalAIEnums.YesNo.Yes;
        CheckComponents();
    }
    
    [HideInInspector] public bool StartWithError;
    [HideInInspector] public string Reasons;
    private bool CheckComponents()
    {
        //Check The AI

        bool Failed = false;
        string Reason = String.Empty;
        
        
        if (StartWithError)
        {
            Failed = true;
            Reason = Reasons;
        }
        else if(Reasons != String.Empty)
        {
            UnityEngine.Debug.LogError(Reasons);
        }

        if (Failed)
        {
            UnityEngine.Debug.LogError(Reason);
            gameObject.SetActive(false);
            return false;
        }

        return true;

    }

    private void OnApplicationQuit()
    {
        WeaponSettings.AISystem.IsWeapon = false;
        WeaponSettings.AISystem.IsShooter = false;
    }

    public void OnDeath()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().isTrigger = false;
    }
    // public void SetHitbox(bool enable)
    // {
    //     Hitbox.CanDamage = enable;
    // }
}


#if UNITY_EDITOR
[CustomEditor(typeof(UniversalAIMeleeWeapon))]
public class UniversalAIMeleeWeaponEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
     
       
    public UniversalAIMeleeWeapon MeleeWeapon;
    
    private void OnEnable()
    {
        MeleeWeapon = (UniversalAIMeleeWeapon)target;
    }

    private bool IgnoreTriggerWarning;
    private bool IgnoreEnabledWarning;
    private bool IgnoreLayerWarning;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
        style.fontSize = 13;
        EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
        EditorGUILayout.LabelField("Universal AI Melee Weapon", style, GUILayout.ExpandWidth(true));
            
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
            
        if (!Application.isPlaying)
        {
            MeleeWeapon.OnValidate();
            #region Error Checker

            if(MeleeWeapon.WeaponSettings.WeaponAttackDistance <= MeleeWeapon.WeaponSettings.AISystem.Settings.Movement.TooCloseDistance)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Attack Distance' of the Weapon is smaller than AI 'Too Close Distance', make sure to increase it!", MessageType.Error);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                MeleeWeapon.Reasons =
                    "The 'Attack Distance' of the Weapon is smaller than AI 'Too Close Distance', make sure to increase it, disabling the AI: ' " +
                    MeleeWeapon.WeaponSettings.AISystem.gameObject.name + " ' !";
                MeleeWeapon.StartWithError = true;
            }
            else if(!MeleeWeapon.GetComponent<BoxCollider>().isTrigger && !IgnoreTriggerWarning)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Box Collider' of the Weapon isn't set to trigger, make sure to set it to damage enemies!", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
 
                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                {
                    IgnoreTriggerWarning = true;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space();

                MeleeWeapon.Reasons = String.Empty;
                MeleeWeapon.StartWithError = false;
            }
            else if(MeleeWeapon.WeaponSettings.Hitbox.Enabled == UniversalAIEnums.YesNo.No && !IgnoreEnabledWarning)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Melee Hitbox' of the Weapon isn't enabled, make sure to enable it to damage enemies!", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
 
                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                {
                    IgnoreEnabledWarning = true;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space();

                MeleeWeapon.Reasons = String.Empty;
                MeleeWeapon.StartWithError = false;
            }
            else if(Physics.GetIgnoreLayerCollision(MeleeWeapon.gameObject.layer, MeleeWeapon.WeaponSettings.AISystem.gameObject.layer) && !IgnoreLayerWarning)
            {
                EditorGUILayout.Space();
                GUI.backgroundColor = new Color(10f, 10f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The 'Weapon Layer' and 'AI Layer' can't collide with each other, please change it in order to damage enemies!", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
 
                if (GUILayout.Button("Ignore Warning", GUILayout.Width(130),GUILayout.Height(20)))
                {
                    IgnoreLayerWarning = true;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space();

                MeleeWeapon.Reasons = String.Empty;
                MeleeWeapon.StartWithError = false;
            }
            else
            {
                MeleeWeapon.Reasons = String.Empty;
                MeleeWeapon.StartWithError = false;
            }
            
            #endregion
        }
        
        EditorGUILayout.Space();
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

}