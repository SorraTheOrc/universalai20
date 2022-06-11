#if INTEGRATION_FPV2NEWER && UniversalAI_Integration_MMFPSE
using MarsFPSKit;
using UnityEngine;


namespace UniversalAI
{

public class UniversalAIMmfpseIntegration : MonoBehaviour, IKitDamageable
{
   
    private UniversalAIDamageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<UniversalAIDamageable>();
    }
    
    public bool LocalDamage(float dmg, int gunID, Vector3 shotPos, Vector3 forward, float force, Vector3 hitPos, bool shotBot,
        int shotId)
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(dmg, AttackerType.Player, null);
            return true;
        }
        else
        {
            return false;
        }
    }
}
    
}
#endif