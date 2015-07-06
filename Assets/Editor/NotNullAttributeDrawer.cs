using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

/*
 * Based off the tutorial found here:
 * http://blogs.unity3d.com/2012/09/07/property-drawers-in-unity-4/
 * Gives an intro into a RegEx attribute, that warns if not a valid expression.
 */
namespace RedBlueTools
{
	[CustomPropertyDrawer(typeof(NotNullAttribute))]
	public class NotNullAttributeDrawer : PropertyDrawer
	{
		int warningHeight = 30;
	
		// Returns the height that is passed into the rect in OnGUI
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			// The height for the object assignment is just whatever Unity would
			// do by default.
			float objectReferenceHeight = base.GetPropertyHeight (property, label);
			float calculatedHeight = objectReferenceHeight;
		
			bool shouldAddWarningHeight = property.propertyType != SerializedPropertyType.ObjectReference ||
				IsNotWiredUp (property);
			if (shouldAddWarningHeight) {
				// When it's not wired up we add in additional height for the warning text.
				calculatedHeight += warningHeight;
			}
		
			return calculatedHeight;
		}
	
		bool IsNotWiredUp (SerializedProperty property)
		{
			return property.objectReferenceValue == null;
		}
	
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			// Position is the DrawArea of the property to be rendered. Includes height from GetHeight()
			// BeginProperty used for objects that don't handle [SerializeProperty] attribute.
			EditorGUI.BeginProperty (position, label, property);
		
			// Calculate ObjectReference rect size
			Rect objectReferenceRect = position;
			// Use Unity's default height calculation for the reference rectangle
			float objectReferenceHeight = base.GetPropertyHeight (property, label);
			objectReferenceRect.height = objectReferenceHeight;
			BuildObjectField (objectReferenceRect, property, label);
		
			// Calculate warning rectangle's size
			Rect warningRect = new Rect (position.x, objectReferenceRect.y + objectReferenceHeight, 
		                            position.width, warningHeight);
			BuildWarningRectangle (warningRect, property);
		
			EditorGUI.EndProperty ();
		}
	
		void BuildObjectField (Rect drawArea, SerializedProperty property, GUIContent label)
		{	
			if (property.propertyType != SerializedPropertyType.ObjectReference) {
				EditorGUI.PropertyField (drawArea, property, label);
				return;
			}
			label.text = "* " + label.text;
			EditorGUI.ObjectField (drawArea, property, label);
		}
	
		void BuildWarningRectangle (Rect drawArea, SerializedProperty property)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference) {
				string warningString = "NotNullAttribute only works with ObjectReference fields";
				EditorGUI.HelpBox (drawArea, warningString, MessageType.Warning);
			} else if (IsNotWiredUp (property)) {
			
				string warningString = "Missing required object reference";
				EditorGUI.HelpBox (drawArea, warningString, MessageType.Error);
			}
		}
	}
}