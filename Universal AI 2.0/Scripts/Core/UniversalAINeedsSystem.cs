//Darking Assets

using System.Collections;
using System.Collections.Generic;

namespace UniversalAI
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

    [RequireComponent(typeof(UniversalAISystem))]
    public class UniversalAINeedsSystem : MonoBehaviour
    {
        [HideInInspector] public bool Health;
        [HideInInspector] public bool Ammo;
        [HideInInspector] public bool Searching;
        
        public enum SearchItems
        {
            Both,
            Health,
            Ammo,
        }
        
        [Space] 
        [Header("SETTINGS")] 
        [Space]
        
        public float AISearchRadius = 15f;
        [Space] [Space] [Help("The radius that AI will search the items in.", HelpBoxMessageType.Info)]
        
        public LayerMask ItemLayers = -1;
        [Space] [Space] [Help("The layer mask of the items that can be found.", HelpBoxMessageType.Info)]
        
        public SearchItems ItemsToSearch = SearchItems.Health;
        [Space] [Space] [Help("The items that can be searched.", HelpBoxMessageType.Info)]
        
        [Condition("Health", true, 0f)]
        [Range(1f, 999f)]
        public float HealthLowLimit = 25f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        [Range(1, 100)]
        public float AmmoLowLimit = 15f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        public UniversalAIShooterWeapon WeaponScript = null;
        [Space]

        [ReadOnly]
        public UniversalAISystem AISystem;


        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (AISystem == null)
            {
                AISystem = GetComponent<UniversalAISystem>();
            }

            if (ItemsToSearch == SearchItems.Both)
            {
                Health = true;
                Ammo = true;
            }
            else if (ItemsToSearch == SearchItems.Health)
            {
                Health = true;
                Ammo = false;
            }
            else
            {
                Health = false;
                Ammo = true;
            }
        }

        public void Pickup()
        {
            CurrentItem.Pickup();
        }

        private void LateUpdate()
        {
            if(Searching)
                return;
            
            if (AISystem.Health <= HealthLowLimit && ItemsToSearch != SearchItems.Ammo)
            {
                SearchItem(true);
            }
            
            if (WeaponScript != null && WeaponScript.AmmoSettings.StorageAmmo <= AmmoLowLimit && ItemsToSearch != SearchItems.Health)
            {
                SearchItem(false);
            }
        }

        private UniversalAIItem CurrentItem;
        private void SearchItem(bool Health)
        {
            Collider[] PossibleItems = Physics.OverlapSphere(transform.position, AISearchRadius, ItemLayers,
                QueryTriggerInteraction.Collide);

            List<UniversalAIItem> Items = new List<UniversalAIItem>();
            
            foreach (var item in PossibleItems)
            {
                UniversalAIItem Item = item.GetComponent<UniversalAIItem>();

                if (Item == null)
                    continue;

                if (Item.ItemCanBeFound == UniversalAIEnums.YesNo.Yes)
                {
                    if (Health && Item.ItemType == ItemType.HealthKit)
                    {
                        Items.Add(Item);   
                    }
                    
                    if (!Health && Item.ItemType == ItemType.AmmoBox)
                    {
                        Items.Add(Item);   
                    }
                }
            }

            if (Items.Count > 1)
            {
                float MinDistance = AISearchRadius;
                UniversalAIItem min = null;

                foreach (var Item in Items)
                {
                    if (Vector3.Distance(transform.position, Item.transform.position) <= MinDistance)
                    {
                        MinDistance = Vector3.Distance(transform.position, Item.transform.position);
                        min = Item;
                    }
                }

                if (min != null)
                {
                    CurrentItem = min;
                    min.GetComponent<SphereCollider>().radius = AISystem.Settings.Movement.StoppingDistance * 3f;
                    Searching = true;
                    AISystem.UniversalAICommandManager.SetDestination(min.transform.position);
                }
            }
            else if(Items.Count == 1)
            {
                CurrentItem = Items[0];
                Items[0].GetComponent<SphereCollider>().radius = AISystem.Settings.Movement.StoppingDistance * 3f;
                Searching = true;
                AISystem.UniversalAICommandManager.SetDestination(Items[0].transform.position);
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UniversalAINeedsSystem))]
    public class UniversalAINeedsSystemEditor : Editor
    {
        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.fontSize = 13;
            EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
            EditorGUILayout.LabelField("Universal AI Needs System", style, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}