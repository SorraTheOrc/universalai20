#if NEOFPS && UniversalAI_Integration_NEOFPS
using System;
using NeoFPS;
using NeoFPS.ModularFirearms;
using UnityEngine;

namespace UniversalAI
{
    /// <summary>
    /// This component will connect a Universal AI 2.0 to NeoFPS weapons to automatically enable sound detection without
    /// having to edit NeoFPS code.
    /// 
    /// To use add this component and `SoloPlayerCharacterEventWatcher` to your AI character.
    /// </summary>
    public class AINeoSoundDetection : MonoBehaviour, IPlayerCharacterSubscriber
    {
        [SerializeField, Tooltip("Will You Use Sound Detection?")]
        UniversalAIEnums.YesNo UseSoundDetection = UniversalAIEnums.YesNo.Yes;
        
        [SerializeField, Tooltip("The radius within which the AI may detect sound.")]
        float m_Radius = 25;

        private IPlayerCharacterWatcher m_Watcher;
        private BaseShooterBehaviour m_Shooter;
        private FpsInventoryBase inventory;
        private GameObject player;
        private UniversalAISystem _system;

        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (GetComponent<SoloPlayerCharacterEventWatcher>() == null)
            {
                gameObject.AddComponent<SoloPlayerCharacterEventWatcher>();
            }

            _system = GetComponent<UniversalAISystem>();
        }

        protected void Awake()
        {
            m_Watcher = GetComponentInParent<IPlayerCharacterWatcher>();
            _system = GetComponent<UniversalAISystem>();
        }

        protected void Start()
        {
            m_Watcher.AttachSubscriber(this);
        }

        private void OnDisable()
        {
            m_Watcher.ReleaseSubscriber(this);
            if (m_Shooter)
            {
                m_Shooter.onShoot -= DetectSound;
            }
        }

        public void OnPlayerCharacterChanged(ICharacter character)
        {
            if (inventory)
            {
                inventory.onSelectionChanged -= OnWieldableSelectionChanged;
                OnWieldableSelectionChanged(0, null);
            }

            if (character as Component != null)
            {
                inventory = character.GetComponent<FpsInventoryBase>();
            }
            else
            {
                inventory = null;
            }

            if(character != null && character.gameObject != null)
                player = character.gameObject;

            if (inventory != null)
            {
                inventory.onSelectionChanged += OnWieldableSelectionChanged;
                OnWieldableSelectionChanged(0, inventory.selected);
            }
        }

        private void OnWieldableSelectionChanged(int slot, IQuickSlotItem item)
        {
            if (m_Shooter)
            {
                m_Shooter.onShoot -= DetectSound;
            }
            
            if (item != null)
            {

                if (item.GetComponent<BaseShooterBehaviour>())
                {
                    m_Shooter = item.GetComponent<BaseShooterBehaviour>();
                    if (m_Shooter)
                    {
                        m_Shooter.onShoot += DetectSound;
                    }
                }
            }
        }

        float m_TimeOfNextCheck;
        float m_ResetSoundAwarenessAfter = 2;
        private void DetectSound(IModularFirearm source)
        {
            if (UseSoundDetection == UniversalAIEnums.YesNo.No
                || Time.timeSinceLevelLoad < m_TimeOfNextCheck)
            {
                return;
            }

            m_TimeOfNextCheck += m_ResetSoundAwarenessAfter;
            Check();
              
        }

        private void Check()
        {
            UniversalAIManager.SoundDetection(UniversalAIEnums.SoundType.ShootSound, m_Radius, player.transform);
        }
    }
}
#endif