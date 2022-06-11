
using System;
using UnityEngine;

namespace UniversalAI
{
	//Generic AI
	
	
	[AttributeUsage(AttributeTargets.Field)]
    public class Condition : PropertyAttribute
    {
        public readonly string m_PropertyName;
        public readonly float m_Indentation;

        public readonly bool m_RequiredBool = false;
		
		public readonly float m_RequiredFloat = -1f;
        public readonly string m_RequiredString = "s";
        public readonly Vector3 m_RequiredVector3 = new Vector3(1, 1, 1);

public readonly int m_RequiredInt = -1;
		public Condition(string propertyName, bool requiredValue, float indentation = 16)
        {
            m_PropertyName = propertyName;
            m_RequiredBool = requiredValue;

            m_Indentation = indentation;
        }

		public Condition(string propertyName, int requiredValue, float indentation = 16)
		{
			m_PropertyName = propertyName;
			m_RequiredInt = requiredValue;

			m_Indentation = indentation;
		}

		public Condition(string propertyName, float requiredValue, float indentation = 16)
		{
			m_PropertyName = propertyName;
			m_RequiredFloat = requiredValue;

			m_Indentation = indentation;
		}
			
        public Condition(string propertyName, string requiredValue, float indentation = 16)
        {
            m_PropertyName = propertyName;
            m_RequiredString = requiredValue;

            m_Indentation = indentation;
        }
			
		public Condition(string propertyName, Vector3 requiredValue, float indentation = 16)
        {
            m_PropertyName = propertyName;
            m_RequiredVector3 = requiredValue;

            m_Indentation = indentation;
        }
    }
}
