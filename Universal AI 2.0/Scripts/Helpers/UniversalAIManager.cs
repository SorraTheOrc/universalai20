using System;
using System.Collections.Generic;

using UnityEngine;

namespace UniversalAI
{
    public class UniversalAIManager : MonoBehaviour
    {
        public static List<UniversalAISystem> AISystems = new List<UniversalAISystem>();

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            UniversalAIManager[] managers = FindObjectsOfType<UniversalAIManager>();

            foreach (var manager in managers)
            {
                if (manager != this)
                    DestroyImmediate(this);
            }
        }

        public static void SoundDetection(UniversalAIEnums.SoundType soundType, float soundRadius, Transform soundSource)
        {
            foreach (var AI in AISystems)
            {
                if(AI != null && AI.gameObject.activeInHierarchy && AI.transform != soundSource)
                    AI.CheckSound(soundRadius, soundSource);
            }
        }
    }
}