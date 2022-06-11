#if (INVECTOR_SHOOTER || INVECTOR_MELEE) && UniversalAI_Integration_INVECTOR

using Invector;
using UnityEngine;

namespace UniversalAI
{

    public class UniversalAIInvectorIntegration : MonoBehaviour, vIDamageReceiver
    {
        private UniversalAIDamageable _damageable;

        private void Start()
        {
            _damageable = GetComponent<UniversalAIDamageable>();
        }

        public OnReceiveDamage onStartReceiveDamage { get; }
        public OnReceiveDamage onReceiveDamage { get; }
        public void TakeDamage(vDamage damage)
        {
            _damageable.TakeDamage(damage.damageValue, AttackerType.Player, damage.sender.gameObject);
        }
    }
    
}
#endif