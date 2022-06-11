using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniversalAI
{


    [RequireComponent(typeof(SphereCollider))]
public class UniversalAIRocketProjectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public UniversalAIShooterWeapon weapon;
    [HideInInspector] public GameObject damager;
    
    [Space] 
    [Header("SETTINGS")] 
    [Space] 
    
    public float ExplosionRadius = 6f;
    [Space] [Space] [Help("How Far Should The Explosion Affect?", HelpBoxMessageType.Info)]

    public UniversalAIEnums.YesNo ImpulseOnImpact = UniversalAIEnums.YesNo.Yes;
    [Space] [Space] [Help("Should The Explosion Affect Near Rigidbodies?", HelpBoxMessageType.Info)]
    
    public float ImpulseForce = 20f;
    [Space] [Space] [Help("Impulse Force That Will Be Applied On Near Rigidbodies.", HelpBoxMessageType.Info)]
    
    public GameObject ExplosionParticle = null;
    [Space] [Space] [Help("The Explosion Particle Object.", HelpBoxMessageType.Info)]
    
    [Space]
    [Header("EVENTS")]
    [Space]

    public UnityEvent OnExplode = new UnityEvent();
    [Space]
    
    public UnityEvent OnDamaged = new UnityEvent();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private List<UniversalAIDamageable> Damageables = new List<UniversalAIDamageable>();
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == gameObject ||
            other.transform.parent != null && other.transform.parent.gameObject == gameObject)
        {
            return;
        }
        
        Collider[] Affect = Physics.OverlapSphere(transform.position, ExplosionRadius);

        Damageables.Clear();
        foreach (var Obj in Affect)
        {
            
            if (Obj.GetComponent<UniversalAIDamageable>() != null ||
                Obj.GetComponent<UniversalAIPlayerReference>() != null)
            {
                UniversalAIDamageable AIDamageable = Obj.GetComponent<UniversalAIDamageable>();

                if(Damageables.Contains(AIDamageable))
                    continue;
                
                Damageables.Add(AIDamageable);
                
                if (AIDamageable != null)
                {
                    // if (AIDamageable is UniversalAISystem)
                    // {
                    //     if(AIDamageable.Equals(weapon.UniversalAISystem))
                    //         return;
                    // }
                    if (AIDamageable is UniversalAIDamageableObject)
                    {
                        UniversalAIDamageableObject AIDamageables = AIDamageable as UniversalAIDamageableObject;
                        if(AIDamageables.UniversalAISystem.Equals(weapon.UniversalAISystem))
                            return;
                    }
                    
                    float DamageValue = damage;
                    AIDamageable.TakeDamage(DamageValue, AttackerType.AI, damager);
                    OnDamaged.Invoke();
                    
                }
                else
                {
                    UniversalAIPlayerReference PlayerReference = Obj.GetComponent<UniversalAIPlayerReference>();
                    
                    if (PlayerReference != null)
                    {
                        float DamageValue = damage;
                        PlayerReference.TakeDamage(DamageValue, AttackerType.AI, damager);
                        OnDamaged.Invoke();
                        
                    }
                }
            }
            else if (Obj.transform.root.GetComponent<UniversalAIDamageable>() != null ||
                     Obj.transform.root.GetComponent<UniversalAIPlayerReference>() != null)
            {
                GameObject RootObj = Obj.transform.root.gameObject;
                
                UniversalAIDamageable AIDamageable = RootObj.GetComponent<UniversalAIDamageable>();

                if(Damageables.Contains(AIDamageable))
                    continue;
                
                Damageables.Add(AIDamageable);
                
                if (AIDamageable != null)
                {
                    if (AIDamageable is UniversalAISystem)
                    {
                        if(AIDamageable.Equals(weapon.UniversalAISystem))
                            return;
                    }
                    if (AIDamageable is UniversalAIDamageableObject)
                    {
                        UniversalAIDamageableObject AIDamageables = AIDamageable as UniversalAIDamageableObject;
                        if(AIDamageables.UniversalAISystem.Equals(weapon.UniversalAISystem))
                            return;
                    }
                    
                    float DamageValue = damage;
                    AIDamageable.TakeDamage(DamageValue, AttackerType.AI, damager);
                    OnDamaged.Invoke();
                    
                }
                else
                {
                    UniversalAIPlayerReference PlayerReference = RootObj.GetComponent<UniversalAIPlayerReference>();
                    
                    if (PlayerReference != null)
                    {
                        float DamageValue = damage;
                        PlayerReference.TakeDamage(DamageValue, AttackerType.AI, damager);
                        OnDamaged.Invoke();
                        
                    }
                }
            } 
            if(Obj.GetComponent<Rigidbody>() != null)
            {
                Obj.GetComponent<Rigidbody>().AddForce(ImpulseForce * (Vector3.up + Vector3.forward), ForceMode.Impulse);
            }
        }
        OnExplode.Invoke();

        if (ExplosionParticle != null)
        {
            ExplosionParticle.transform.SetParent(null);
            ExplosionParticle.SetActive(true);
        }
        
        Destroy(gameObject);
    }
}
    
}