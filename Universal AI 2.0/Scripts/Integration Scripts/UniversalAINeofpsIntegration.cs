#if NEOFPS && UniversalAI_Integration_NEOFPS


using NeoFPS;
using UnityEngine;

namespace UniversalAI
{


public class UniversalAINeofpsIntegration : MonoBehaviour, IDamageHandler
{
    private UniversalAIDamageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<UniversalAIDamageable>();
    }

    public DamageFilter inDamageFilter { get; set; }
    public DamageResult AddDamage(float damage)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(damage, AttackerType.Player, null);
            return DamageResult.Standard;
        }

        return DamageResult.Ignored;
    }

    public DamageResult AddDamage(float damage, RaycastHit hit)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(damage, AttackerType.Player, null);
            return DamageResult.Standard;
        }

        return DamageResult.Ignored;
    }

    public DamageResult AddDamage(float damage, IDamageSource source)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(damage, AttackerType.Player, source.controller.currentCharacter.gameObject);
            return DamageResult.Standard;
        }

        return DamageResult.Ignored;
    }

    public DamageResult AddDamage(float damage, RaycastHit hit, IDamageSource source)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(damage, AttackerType.Player, source.controller.currentCharacter.gameObject);
            return DamageResult.Standard;
        }

        return DamageResult.Ignored;
    }
}
    
}

#endif