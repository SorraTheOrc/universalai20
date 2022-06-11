#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniversalAI
{

[InitializeOnLoad]
public class DefineLoad : MonoBehaviour
{

    private static bool added = false;

    static DefineLoad()
    {
        if(!added)
            AddRemoveDefine("Universal_AI_20", true);
    }

    private static void AddRemoveDefine(string DefineName, bool add)
            {
            
                List<string> Symbols = new List<string>();
                Symbols.Add(DefineName);
          
                string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                List<string> allDefines = definesString.Split(';').ToList();
            
                if(add)
                    allDefines.AddRange(Symbols.Except(allDefines));
    
                if (!add && allDefines.Contains(DefineName))
                {
                    allDefines.Remove(DefineName);
                }
            
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", allDefines.ToArray()));
                added = true;
            }
}
    
}

#endif