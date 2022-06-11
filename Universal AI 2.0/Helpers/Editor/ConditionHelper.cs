using UnityEngine;
using UnityEditor;

namespace UniversalAI
{
	
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionHelper : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
			if(CanShow(property))
			{
				var indentedRect = position;
				float indentation = ((Condition)attribute).m_Indentation;

				position.x += indentation;
				position.width -= indentation;
				EditorGUI.PropertyField(position, property, label, true);
			}
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
			if (!CanShow(property))
			
				return -EditorGUIUtility.standardVerticalSpacing;
            else
				return EditorGUI.GetPropertyHeight(property, true);
        }

		private bool CanShow(SerializedProperty property)
		{
			var attr = (Condition)attribute;

			string parentPath = string.Empty;
			parentPath = property.GetParentPath();
			SerializedProperty AIConditionProperty = property.serializedObject.FindProperty(parentPath + (parentPath != string.Empty ? "." : "") + attr.m_PropertyName);

			if(AIConditionProperty != null)
			{
				if(AIConditionProperty.propertyType == SerializedPropertyType.Boolean)
					return attr.m_RequiredBool == AIConditionProperty.boolValue;
				else if(AIConditionProperty.propertyType == SerializedPropertyType.ObjectReference)
					return attr.m_RequiredBool == (AIConditionProperty.objectReferenceValue != null);
				else if(AIConditionProperty.propertyType == SerializedPropertyType.Integer)
					return attr.m_RequiredInt == AIConditionProperty.intValue;
				else if(AIConditionProperty.propertyType == SerializedPropertyType.Enum)
					return attr.m_RequiredInt == AIConditionProperty.intValue;
				else if(AIConditionProperty.propertyType == SerializedPropertyType.Float)
					return attr.m_RequiredFloat == AIConditionProperty.floatValue;
				else if(AIConditionProperty.propertyType == SerializedPropertyType.String)
					return attr.m_RequiredString == AIConditionProperty.stringValue;
				else if(AIConditionProperty.propertyType == SerializedPropertyType.Vector3)
					return attr.m_RequiredVector3 == AIConditionProperty.vector3Value;
			}

			return false;
		}
    }
}

