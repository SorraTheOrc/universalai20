using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalAI
{


public class UniversalAIRagdollBone : MonoBehaviour
{
    [HideInInspector] public UniversalAISystem system;
    private Transform OldTarget = null;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void LateUpdate()
    {
        if (OldTarget == null && system.Target != null)
        {
            OldTarget = system.Target.transform;
            if (system.Target.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(col, system.Target.GetComponent<Collider>());
            }
        }
        else if(OldTarget != null && system.Target != null &&  OldTarget != system.Target.transform)
        {
            OldTarget = system.Target.transform;
            if (system.Target.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(col, system.Target.GetComponent<Collider>());
            }
        }
    }
}
    
}