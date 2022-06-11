using UnityEngine;
using UnityEditor;

namespace UniversalAI
{

    [CustomPropertyDrawer(typeof(HelpAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer {
 
        public override float GetHeight() {
            var helpBoxAttribute = attribute as HelpAttribute;
            if (helpBoxAttribute == null) return base.GetHeight();
            var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
            
            if (helpBoxAttribute.messageType == HelpBoxMessageType.None ||
                helpBoxAttribute.messageType == HelpBoxMessageType.Error ||
                helpBoxAttribute.messageType == HelpBoxMessageType.Info ||
                helpBoxAttribute.messageType == HelpBoxMessageType.Warning) return 20;
            
            
            if (helpBoxStyle == null) return base.GetHeight();
            return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text), EditorGUIUtility.currentViewWidth) + 4);
        }
        
        
        public override void OnGUI(Rect position) {
            var helpBoxAttribute = attribute as HelpAttribute;
            if (helpBoxAttribute == null) return;

            Rect rect = position;

#if UNITY_2021_1_OR_NEWER
            if(helpBoxAttribute.messageType == HelpBoxMessageType.Error || 
               helpBoxAttribute.messageType == HelpBoxMessageType.Warning ||
               helpBoxAttribute.messageType == HelpBoxMessageType.Info)
                rect.y -= 17;
            else
                rect.y -= 8;      
#endif
            EditorGUI.HelpBox(rect , helpBoxAttribute.text, GetMessageType(helpBoxAttribute.messageType));
        }
 
        private MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType) {
            switch (helpBoxMessageType) {
                case HelpBoxMessageType.None: return MessageType.None;
                case HelpBoxMessageType.Info: return MessageType.Info;
                case HelpBoxMessageType.Warning: return MessageType.Warning;
                case HelpBoxMessageType.Error: return MessageType.Error;
                case HelpBoxMessageType.BigError: return MessageType.Error;
                case HelpBoxMessageType.BigInfo: return MessageType.Info;
                case HelpBoxMessageType.BigWarning: return MessageType.Warning;

            }

            return MessageType.None;
        }
    }    
}