using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace RedBlueTools
{
	public class NotNullError
	{
		public GameObject ErrorGameObject;
		public List<ErrorMonoBehaviour> MonoBehavioursWithErrors;
		public string AssetPath;

		public string FullName {
			get {
				Transform currentParent = ErrorGameObject.transform.parent;
				string fullName = ErrorGameObject.name;
				while (currentParent != null) {
					fullName = currentParent.gameObject.name + "/" + fullName;
					currentParent = currentParent.transform.parent;
				}
				return fullName;
			}
		}
	
		public int NumErroringMonoBehaviours {
			get {
				if (MonoBehavioursWithErrors == null) {
					return 0;
				} else {
					return MonoBehavioursWithErrors.Count;
				}
			}
		}

		public int NumFieldsWithErrors {
			get {
				int sumOfErrors = 0;
				foreach (ErrorMonoBehaviour erroringMB in MonoBehavioursWithErrors) {
					sumOfErrors += erroringMB.NumErrorFields;
				}
				return sumOfErrors;
			}
		}
	
		public NotNullError (GameObject sourceObject, string assetPath)
		{
			this.ErrorGameObject = sourceObject;
			this.AssetPath = assetPath;
		
			MonoBehaviour[] monobehaviours = ErrorGameObject.GetComponents<MonoBehaviour> ();
			this.MonoBehavioursWithErrors = new List<ErrorMonoBehaviour> ();
			for (int i = 0; i < monobehaviours.Length; i++) {
				if (ErrorMonoBehaviour.MonoBehaviourHasErrors (monobehaviours [i])) {
					this.MonoBehavioursWithErrors.Add (new ErrorMonoBehaviour (monobehaviours [i]));
				}
			}
		}
		
		public static void TraverseGameObjectHierarchyForErrors (GameObject obj, string assetPath, ref List<NotNullError> errorsInHierarchy)
		{ 
			if (NotNullError.ObjectHasErrors (obj)) {
				errorsInHierarchy.Add (new NotNullError (obj, assetPath));
			}
			foreach (Transform child in obj.transform) {
				TraverseGameObjectHierarchyForErrors (child.gameObject, assetPath, ref errorsInHierarchy);
			}
		}
		
		public static bool ObjectHasErrors (GameObject gameObject)
		{
			MonoBehaviour[] monobehaviours = gameObject.GetComponents<MonoBehaviour> ();
			for (int i = 0; i < monobehaviours.Length; i++) {
				if (ErrorMonoBehaviour.MonoBehaviourHasErrors (monobehaviours [i])) {
					return true;
				}
			}
			
			return false;
		}

		public void OutputErrorForPrefabs ()
		{
			OutputError (true);
		}

		public void OutputErrorForSceneObjects ()
		{
			OutputError (false);
		}
	
		void OutputError (bool checkPrefabs)
		{
			foreach (ErrorMonoBehaviour errorMB in MonoBehavioursWithErrors) {
				foreach (ErrorField error in errorMB.ErrorFields) {
					bool overlookError = checkPrefabs && error.AllowNullAsPrefab;
					if (overlookError) {
						continue;
					}
					Debug.LogError (string.Format ("NotNull field: {0} " +
						"has not been assigned on object: {1}\nPath: {2}",
				        error.FieldInfo.Name, FullName, AssetPath), this.ErrorGameObject);
				}
			}
		}
	}

	public class ErrorMonoBehaviour
	{
		public MonoBehaviour SourceMonoBehaviour;
		public bool IsMissing;
		public List<ErrorField> ErrorFields;
		
		public int NumErrorFields {
			get {
				if (ErrorFields == null) {
					return 0;
				} else {
					return ErrorFields.Count;
				}
			}
		}
		
		public ErrorMonoBehaviour (MonoBehaviour sourceMB)
		{
			this.SourceMonoBehaviour = sourceMB;
			this.IsMissing = sourceMB == null;
			
			if (sourceMB != null) {
				this.ErrorFields = FindErroringFields (sourceMB);
			}
		}
		
		static List<ErrorField> FindErroringFields (MonoBehaviour sourceMB)
		{
			List<ErrorField> erroringFields = new List<ErrorField> ();

			// Add null NotNull fields
			List<FieldInfo> notNullFields = ReflectionUtilities.GetMonoBehaviourFieldsWithAttribute<NotNullAttribute> (sourceMB);
			foreach (FieldInfo notNullField in notNullFields) {
				object fieldObject = notNullField.GetValue (sourceMB);
				if (fieldObject == null || fieldObject.Equals (null)) {
					erroringFields.Add (new ErrorField (notNullField, sourceMB, false));
				}
			}

			// Flag notNullAttributes that are allowed to be null as prefabs
			foreach (ErrorField errorField in erroringFields) {
				FieldInfo fieldInfo = errorField.FieldInfo;
				foreach (Attribute attribute in Attribute.GetCustomAttributes (fieldInfo)) {
					if (attribute.GetType () == typeof(NotNullAttribute)) {
						NotNullAttribute notNullAttribute = (NotNullAttribute) attribute;
						errorField.AllowNullAsPrefab = notNullAttribute.IgnorePrefab;
					}
				}
			}

			return erroringFields;
		}
		
		public static bool MonoBehaviourHasErrors (MonoBehaviour mb)
		{
			if (mb == null) {
				return true;
			}
			return FindErroringFields (mb).Count > 0;
		}
	}
	
	public class ErrorField
	{
		public FieldInfo FieldInfo;
		public MonoBehaviour SourceMonoBehaviour;
		public bool AllowNullAsPrefab;
		
		public ErrorField (FieldInfo fieldInfo, MonoBehaviour sourceMB, bool allowNullAsPrefab = false)
		{
			this.FieldInfo = fieldInfo;
			this.SourceMonoBehaviour = sourceMB;
			this.AllowNullAsPrefab = allowNullAsPrefab;
		}
	}
}

