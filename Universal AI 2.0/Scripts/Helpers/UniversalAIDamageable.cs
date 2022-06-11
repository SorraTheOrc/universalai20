using UnityEngine;

namespace UniversalAI
{

    public enum AttackerType
    {
        Player,
        AI,
        Other,
    }
    public enum AttackerTypeCheck
    {
        All,
        Player,
        AI,
        Other,
    }
public interface UniversalAIDamageable
{
    void TakeDamage(float damageamount, AttackerType attackerType, GameObject Attacker, bool BlockSuccess = true);
}

}