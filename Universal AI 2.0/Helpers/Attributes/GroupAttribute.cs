
using UnityEngine;

namespace UniversalAI
{

    public class GroupAttribute : PropertyAttribute
    {
      
        public bool OnlyShowPrefixIfNotExpanded { get; private set; } = true;

        public string PrefixText { get; private set; } = "";
        public GroupAttribute()
        {
            OnlyShowPrefixIfNotExpanded = false;
            PrefixText = "";
        }

        public GroupAttribute(string appendText, bool showPrefixOnlyWhenNotExpanded = true)
        {
            PrefixText = appendText;
            OnlyShowPrefixIfNotExpanded = showPrefixOnlyWhenNotExpanded;
        }
    }
    
}