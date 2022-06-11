using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalAI
{

    [CreateAssetMenu(fileName = "TagStorage", menuName = "UniversalAI/TagStorage", order = 2)]
public class UniversalAITags : ScriptableObject
{

    public  List<string> AvailableAITags = new List<string>();
    
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
    
    public void ForceSerialization()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
    
}