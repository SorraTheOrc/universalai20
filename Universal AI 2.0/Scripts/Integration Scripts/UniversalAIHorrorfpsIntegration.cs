#if UniversalAI_Integration_HORRORFPS
using UnityEngine;


namespace UniversalAI
{


public class UniversalAIHorrorfpsIntegration : MonoBehaviour
{
   
   private UniversalAIDamageable _damageable;

   private void Start()
   {
      _damageable = GetComponent<UniversalAIDamageable>();
   }
   
   public void ApplyDamage(float damage)
   {
      if (_damageable != null)
      {
         _damageable.TakeDamage(damage, AttackerType.Player, null);
      }
   }
}
   
}
#endif