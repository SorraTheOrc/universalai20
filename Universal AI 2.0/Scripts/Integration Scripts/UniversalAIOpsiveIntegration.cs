#if (FIRST_PERSON_CONTROLLER || THIRD_PERSON_CONTROLLER) && UniversalAI_Integration_OPSIVE

using Opsive.UltimateCharacterController.Traits.Damage;
using UnityEngine;

namespace UniversalAI
{
    
public class UniversalAIOpsiveIntegration : MonoBehaviour, IDamageTarget
{
    [HideInInspector] public UniversalAIDamageable System;
    [HideInInspector] public GameObject Owner { get; }
    [HideInInspector] public GameObject HitGameObject { get; }

    private void Awake()
    {
        if(GetComponent<UniversalAIDamageable>() != null)
            System = GetComponent<UniversalAIDamageable>();
    }

    public void Damage(DamageData damageData)
    {
        if(System != null)
            System.TakeDamage(damageData.Amount, AttackerType.Player, null);
    }

    public bool IsAlive()
    {
        return true;
    }
}
    
}

#endif