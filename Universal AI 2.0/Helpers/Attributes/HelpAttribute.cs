using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniversalAI
{

    public enum HelpBoxMessageType
    {
        None,
        Info,
        Warning,
        Error,
        BigInfo,
        BigWarning,
        BigError,
    }

    public class HelpAttribute : PropertyAttribute
    {

        public string text;
        public HelpBoxMessageType messageType;

        public HelpAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
        {
            this.text = text;
            this.messageType = messageType;
        }
    }


}