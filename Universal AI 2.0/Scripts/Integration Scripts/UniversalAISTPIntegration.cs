#if SURVIVAL_TEMPLATE_PRO && UniversalAI_Integration_STP

using UnityEngine;
using SurvivalTemplatePro;
using SurvivalTemplatePro.Surfaces;

namespace UniversalAI
{


public class UniversalAISTPIntegration : MonoBehaviour, IDamageReceiver
{
   
    private UniversalAIDamageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<UniversalAIDamageable>();
        gameObject.AddComponent<SurfaceIdentity>().Surface =  Resources.Load<SurfaceInfo>("Surfaces/Surface_Flesh");
    }


    public DamageResult HandleDamage(DamageInfo damageInfo)
    {
        _damageable.TakeDamage(damageInfo.Damage, AttackerType.Player, damageInfo.Source.gameObject);

        return DamageResult.Default;
    }
}
    
}
#endif