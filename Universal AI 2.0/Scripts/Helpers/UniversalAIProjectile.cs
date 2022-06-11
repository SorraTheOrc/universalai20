using System;
using UnityEngine;


namespace UniversalAI
{
public class UniversalAIProjectile : MonoBehaviour
{

    public UniversalAIEnums.YesNo DebugHitObject = UniversalAIEnums.YesNo.No;
    [HideInInspector] public float damage;
    [HideInInspector] public UniversalAIShooterWeapon weapon;
    [HideInInspector] public GameObject damager;
    [HideInInspector] public LayerMask ProjectileHitLayers = 1;

    private bool canthit = false;
    private void Update()
    {
      if(canthit)
          return;
      
      RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.9f, ProjectileHitLayers,
                QueryTriggerInteraction.Ignore))
        {
            GameObject col = hit.transform.gameObject;
            if (DebugHitObject == UniversalAIEnums.YesNo.Yes)
            {
                Debug.Log("Projectile Hit: " + col.gameObject.name + " !!");
            }
        
            if (col.GetComponent<UniversalAIPlayerReference>() != null)
            {
                col.GetComponent<UniversalAIPlayerReference>().TakeDamage(damage, AttackerType.AI, damager);
            }

            if (col.GetComponent<UniversalAIDamageable>() != null)
            {
                if (col.GetComponent<UniversalAIDamageable>() is UniversalAISystem)
                {
                    if (col.GetComponent<UniversalAISystem>() == weapon.UniversalAISystem)
                    {
                        return;
                    }

                    // if (col.GetComponent<UniversalAISystem>().Detection.Factions.Factions
                    //     .Equals(weapon.UniversalAISystem.Detection.Factions.Factions))
                    // {
                    //     return;
                    // }
                }
                else if (col.GetComponent<UniversalAIDamageable>() is UniversalAIDamageableObject)
                {
                    if (col.GetComponent<UniversalAIDamageableObject>().UniversalAISystem == weapon.UniversalAISystem)
                    {
                        return;
                    }
                }
                col.GetComponent<UniversalAIDamageable>().TakeDamage(damage, AttackerType.AI, damager);
                weapon.UniversalAISystem.UniversalAIEvents.OnDealDamage.Invoke(damage);
                canthit = true;
            }

            if (weapon.VFXSettings.BulletSettings.ImpactEffects.Count > 0)
            {
                if (weapon.VFXSettings.BulletSettings.ImpactEffects.Count < 2)
                {
                    GameObject Effect = Instantiate(weapon.VFXSettings.BulletSettings.ImpactEffects[0].ImpactEffect,
                        hit.point + new Vector3(0.0f,0,0.0f), Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    if (weapon.VFXSettings.BulletSettings.ImpactEffects[0].SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                    {
                        Effect.transform.SetParent(null);
                    }
                    else
                    {
                        Effect.transform.SetParent(hit.transform);
                    }
                }
                else
                {
                    foreach (var effect in weapon.VFXSettings.BulletSettings.ImpactEffects)
                    {
                        if (hit.transform.tag.Equals(effect.ObjectTag))
                        {
                            GameObject Effect = Instantiate(effect.ImpactEffect,
                                hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                            if (effect.SetParent == UniversalAIMeleeWeapon.impactList.setParent.Null)
                            {
                                Effect.transform.SetParent(null);
                            }
                            else
                            {
                                Effect.transform.SetParent(hit.transform);
                            }
                            
                            break;
                        }
                    }   
                }
            }
            Destroy(gameObject, 0.07f);
        }
    }

}
}