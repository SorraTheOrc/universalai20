using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    public enum RightLeft
    {
        Right,
        Left,
    }
    
    public enum AudioType
    {
        Idle,
        Attack,
        Damaged,
        TakeDamage,
        Alerted,
        LostTarget,
        Death,
    }
    
    public enum WeaponAudioType
    {
        Fire,
        Reload,
        Equip,
        UnEquip,
        Hit,
        Damaged,
    }
  
    public class UniversalAISounds : MonoBehaviour
    {
        [Space]
        [Header("MOVEMENT")]
        [Space] 
        [Help("You Need This Script To Use The AI Sound System !", HelpBoxMessageType.BigInfo)] 
        
        
        [Reorderable]
        public SoundsOverrideList IdleSounds = new SoundsOverrideList();

        [Space]
        
        [Reorderable]
        public SoundsOverrideList WalkSounds = new SoundsOverrideList();

        [Space]
        
        [Reorderable]
        public SoundsOverrideList RunSounds = new SoundsOverrideList();

        [Space]

        [Space]
        [Header("COMBAT")]
        [Space]
        
        [Reorderable]
        public SoundsOverrideList OnAttack = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Attacks !",HelpBoxMessageType.Info)]
        
        [Reorderable]
        public SoundsOverrideList OnDamagedTarget = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Attack Hits The Target !",HelpBoxMessageType.Info)]
        
        [Reorderable]
        public SoundsOverrideList OnTakeDamage = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Takes Damage !",HelpBoxMessageType.Info)]
        
        [Reorderable]
        public SoundsOverrideList OnDeath = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Dies !",HelpBoxMessageType.Info)]
        
       
        [Header("DETECTION")]
        [Space]
        [Space]
        
        [Reorderable]
        public SoundsOverrideList OnAlerted = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Is Alerted !",HelpBoxMessageType.Info)]
        
        [Reorderable]
        public SoundsOverrideList OnLostTarget = new SoundsOverrideList();
        [Space] [Space] [Help("The Sound That Will Play When The AI Losts The Target !",HelpBoxMessageType.Info)]
        
        [Header("WEAPON")]
        [Space]
        [Space]

        
        [ReadOnly] public AudioSource AudioSource;
        
        [HideInInspector] public GameObject SoundObject;

        [HideInInspector] public bool Initialize;

        private void OnValidate()
        {
            if(!Initialize)
                return;
            
            if(Application.isPlaying)
                return;

            if (AudioSource == null)
            {
                if (GetComponent<AudioSource>()!= null)
                {
                    AudioSource = GetComponent<AudioSource>();
                }

                AudioSource.playOnAwake = false;
            }

            if (SoundObject == null)
            {
                if (Resources.Load<GameObject>("SoundObject")!= null)
                {
                    SoundObject = Resources.Load<GameObject>("SoundObject");
                }
            }

            foreach (var sound in IdleSounds)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in WalkSounds)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in RunSounds)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnAttack)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnDamagedTarget)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnTakeDamage)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnDeath)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnAlerted)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
            
            foreach (var sound in OnLostTarget)
            {
                if (sound.Volume <= 0)
                {
                    sound.Volume = 1;
                }
            }
        }

        public void PlayWalkSound()
        {
            AudioSource.Stop();
            AudioSource.loop = false;
            
            int random = Random.Range(0, WalkSounds.Length);

            AudioSource.volume = WalkSounds[random].Volume;
            AudioSource.PlayOneShot(WalkSounds[random].Clip);
        }

        public void PlayRunSound()
        {
            AudioSource.Stop();
            AudioSource.loop = false;
            
            int random = Random.Range(0, RunSounds.Length);

            AudioSource.volume = RunSounds[random].Volume;
            AudioSource.PlayOneShot(RunSounds[random].Clip);
        }
        
        private void Awake()
        {
            if (AudioSource != null)
            {
                AudioSource.playOnAwake = false;
            }
        }

        public void PlaySound(AudioType type)
        {
            if (type == AudioType.Attack && OnAttack.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                int random = Random.Range(0, OnAttack.Length);

                AudioSource.volume = OnAttack[random].Volume;
                AudioSource.PlayOneShot(OnAttack[random].Clip);
            }
            
            if (type == AudioType.Idle && IdleSounds.Length > 0)
            {
                AudioSource.loop = true;
                int random = Random.Range(0, IdleSounds.Length);

                AudioSource.volume = IdleSounds[random].Volume;
                AudioSource.clip = IdleSounds[random].Clip;
                AudioSource.Play();
            }
            
            if (type == AudioType.Alerted && OnAlerted.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                int random = Random.Range(0, OnAlerted.Length);

                AudioSource.volume = OnAlerted[random].Volume;
                AudioSource.PlayOneShot(OnAlerted[random].Clip);
            }
            
            if (type == AudioType.LostTarget && OnLostTarget.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                int random = Random.Range(0, OnLostTarget.Length);

                AudioSource.volume = OnLostTarget[random].Volume;
                AudioSource.PlayOneShot(OnLostTarget[random].Clip);
            }

            if (type == AudioType.TakeDamage  && OnTakeDamage.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                int random = Random.Range(0, OnTakeDamage.Length);

                AudioSource.volume = OnTakeDamage[random].Volume;
                AudioSource.PlayOneShot(OnTakeDamage[random].Clip);
            }
            
            if (type == AudioType.Damaged  && OnDamagedTarget.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                if (AudioSource.isPlaying)
                {
                    List<AudioClip> clips = new List<AudioClip>();

                    
                    foreach (var clip in OnAttack)
                    {
                     clips.Add(clip.Clip);
                    }
                    
                    if(!clips.Contains(AudioSource.clip))
                        return;
                    
                    AudioSource.Stop();
                }
                
                int random = Random.Range(0, OnDamagedTarget.Length);

                AudioSource.volume = OnDamagedTarget[random].Volume;
                AudioSource.PlayOneShot(OnDamagedTarget[random].Clip);
            }

            if (type == AudioType.Death  && OnDeath.Length > 0)
            {
                if (AudioSource.loop)
                {
                    AudioSource.Stop();
                }
                
                AudioSource.loop = false;
                int random = Random.Range(0, OnDeath.Length);

                AudioSource.volume = OnDeath[random].Volume;
                AudioSource.PlayOneShot(OnDeath[random].Clip);
            }
        }

    }
    
    [Serializable]
    public class SoundsOverrideList : ReorderableArray<SoundsOverride>
    {
    }

    [Serializable]
    public class SoundsOverride
    {
        public AudioClip Clip;
        public float Volume = 1f;
    }
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(UniversalAISounds))]
    public class UniversalAISoundsEditor : Editor
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
            EditorGUILayout.LabelField("Universal AI Sounds", style, GUILayout.ExpandWidth(true));
            
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