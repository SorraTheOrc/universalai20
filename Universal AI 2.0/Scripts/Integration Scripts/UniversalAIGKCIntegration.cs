#if UniversalAI_Integration_GKC

using UnityEngine;
using UniversalAI;

public class UniversalAIGKCIntegration : healthManagement
{
    public UniversalAIDamageable mainUniversalDamageable;

    bool damageComponentLocated;

    public override void setDamageWithHealthManagement(float damageAmount,
Vector3 fromDirection, Vector3 damagePos, GameObject attacker, GameObject projectile,
bool damageConstant,
bool searchClosestWeakSpot, bool ignoreShield, bool
ignoreDamageInScreen, bool damageCanBeBlocked, bool
canActivateReactionSystemTemporally, int damageReactionID, int damageTypeID)
    {
        if (!damageComponentLocated)
        {
            mainUniversalDamageable = GetComponent<UniversalAIDamageable>();

            damageComponentLocated = true;
        }

        if (damageComponentLocated)
        {
            mainUniversalDamageable.TakeDamage(damageAmount, AttackerType.Player, attacker);
        }
    }

    //public void
}

#endif