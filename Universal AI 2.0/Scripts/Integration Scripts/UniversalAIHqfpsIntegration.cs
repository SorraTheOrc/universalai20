#if HQ_FPS_TEMPLATE && UniversalAI_Integration_HQFPS

using UnityEngine;
using HQFPSTemplate;

namespace UniversalAI
{


public class UniversalAIHqfpsIntegration : MonoBehaviour, IDamageable
{
   
    private UniversalAIDamageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<UniversalAIDamageable>();
    }
    
    public void TakeDamage(DamageInfo damageInfo)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(-damageInfo.Delta, AttackerType.Player, damageInfo.Source.gameObject);
        }
    }
}
    
}
#endif