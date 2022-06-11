using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalAI
{

public class UniversalAIIntegrationsManager : MonoBehaviour
{
    private void Start()
    {
        AddIntegration();
    }

    private void AddIntegration()
    {
        
#if UniversalAI_Integration_INVECTOR && (INVECTOR_SHOOTER || INVECTOR_MELEE)

        if(gameObject.GetComponent<UniversalAIInvectorIntegration>() == null)
            gameObject.AddComponent<UniversalAIInvectorIntegration>();

        UniversalAIDamageableObject[] damageableObjects = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        
        foreach (var damageableObject in damageableObjects)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIInvectorIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIInvectorIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_HQFPS && HQ_FPS_TEMPLATE

        if(gameObject.GetComponent<UniversalAIHqfpsIntegration>() == null)
            gameObject.AddComponent<UniversalAIHqfpsIntegration>();

        UniversalAIDamageableObject[] damageableObjectsHQFPS = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsHQFPS)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIHqfpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIHqfpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_STP && SURVIVAL_TEMPLATE_PRO

        if(gameObject.GetComponent<UniversalAISTPIntegration>() == null)
            gameObject.AddComponent<UniversalAISTPIntegration>();

        UniversalAIDamageableObject[] damageableObjectsSTP = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsSTP)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAISTPIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAISTPIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_GKC

        if(gameObject.GetComponent<UniversalAIGKCIntegration>() == null)
            gameObject.AddComponent<UniversalAIGKCIntegration>();

        UniversalAIDamageableObject[] damageableObjectsGKC = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsGKC)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIGKCIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIGKCIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_MMFPSE && INTEGRATION_FPV2NEWER

        if(gameObject.GetComponent<UniversalAIMmfpseIntegration>() == null)
            gameObject.AddComponent<UniversalAIMmfpseIntegration>();

        UniversalAIDamageableObject[] damageableObjectsMMFPSE = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsMMFPSE)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIMmfpseIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIMmfpseIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_NEOFPS && NEOFPS

        if(gameObject.GetComponent<UniversalAINeofpsIntegration>() == null)
            gameObject.AddComponent<UniversalAINeofpsIntegration>();

        UniversalAIDamageableObject[] damageableObjectsNEOFPS = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsNEOFPS)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAINeofpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAINeofpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_OPSIVE && (THIRD_PERSON_CONTROLLER || FIRST_PERSON_CONTROLLER)

        if(gameObject.GetComponent<UniversalAIOpsiveIntegration>() == null)
            gameObject.AddComponent<UniversalAIOpsiveIntegration>();

        UniversalAIDamageableObject[] damageableObjectsOPSIVE = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsOPSIVE)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIOpsiveIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIOpsiveIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_HORRORFPS

        if(gameObject.GetComponent<UniversalAIHorrorfpsIntegration>() == null)
            gameObject.AddComponent<UniversalAIHorrorfpsIntegration>();

        UniversalAIDamageableObject[] damageableObjectsHORRORFPS = GetComponentsInChildren<UniversalAIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsHORRORFPS)
        {
            if(damageableObject.gameObject.GetComponent<UniversalAIHorrorfpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<UniversalAIHorrorfpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if UniversalAI_Integration_USK

        //Do This After 1.7 Update USK
#endif
    }
}
    
}