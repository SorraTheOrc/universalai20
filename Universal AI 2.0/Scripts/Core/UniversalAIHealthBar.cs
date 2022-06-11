using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalAI
{
    public class UniversalAIHealthBar : MonoBehaviour
    {
        public UniversalAIEnums.HealthBarDisplay HealthBarDisplay;
        
        [HideInInspector] public UniversalAISystem system;

        [HideInInspector] public Image HealthBar;

        [HideInInspector] public Text NameText;

         public string AIName = "AI";
        
         public Color HealthBarColor = Color.red;

         public Color EnemyNameColor = Color.red;
       
         public Color FriendlyNameColor = Color.green;

        [HideInInspector] public CanvasGroup CG;
        private void Awake()
        {
            if (system == null) 
            {
                if (transform.parent != null)
                {
                    if (transform.parent.GetComponent<UniversalAISystem>() != null)
                    {
                        system = transform.parent.GetComponent<UniversalAISystem>();
                    }   
                }
            }
            
            if(system == null)
                return;
            
            system.UniversalAIHealthBar = this;
            
            if(CG == null)
                CG = transform.GetChild(0).GetComponent<CanvasGroup>();
            
            CG.alpha = 0;
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.OnlyHealth)
            {
                NameText.enabled = false;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.OnlyName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(false);
            }
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.HealthAndName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }

            HealthBar.color = HealthBarColor;
        }

        public void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (system == null) 
            {
                if (transform.parent != null)
                {
                    if (transform.parent.GetComponent<UniversalAISystem>() != null)
                    {
                        system = transform.parent.GetComponent<UniversalAISystem>();
                    }   
                }
            }
            
            if(system == null)
                return;
            
            system.UniversalAIHealthBar = this;
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.OnlyHealth)
            {
                NameText.enabled = false;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.OnlyName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(false);
            }
            
            if (HealthBarDisplay == UniversalAIEnums.HealthBarDisplay.HealthAndName)
            {
                NameText.enabled = true;
                HealthBar.transform.parent.gameObject.SetActive(true);
            }

            if (system.Stats == null)
            {
                system.Stats = new UniversalAISystem.stats();
            }

            if (CG == null)
            {
                CG = transform.GetChild(0).GetComponent<CanvasGroup>();
            }
            
            HealthBar.fillAmount = system.Stats.StartHealth;

            NameText.text = AIName;
        }

       
        private void FixedUpdate()
        {

            if (system.General.AIType == UniversalAIEnums.AIType.Enemy)
            {
                NameText.color = EnemyNameColor;
            }
            else
            {
                NameText.color = FriendlyNameColor;
            }
           
            HealthBar.fillAmount = system.Health / system.Stats.StartHealth;

            NameText.text = AIName;

            if (system.Target == null && CG.alpha > 0.9f)
            {
                FadeOut();
            }
            
            if (system.Target != null && CG.alpha < 0.1f)
            {
                FadeIn();
            }

            if (system.Target != null)
            {
                transform.LookAt(transform.parent.position + system.Target.transform.rotation * Vector3.forward,
                    system.Target.transform.rotation * Vector3.up);
            }
          
        }
        
        public void FadeOut()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(FadeTo(0f, 1f));
            }
        }
        
        public void FadeIn()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(FadeTo(1f, 1f));
            }
        }
        
        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = CG.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                if (aValue.Equals(0f))
                {
                    if (CG.alpha <= 0.05f)
                    {
                        CG.alpha = 0;
                        break;
                    }
                }
                if (aValue.Equals(1f))
                {
                    if (CG.alpha >= 0.95f)
                    {
                        CG.alpha = 1;
                        break;
                    }
                }
                
                CG.alpha = Mathf.Lerp(alpha, aValue, t);
                yield return null;
            }
        }
    }
    
}