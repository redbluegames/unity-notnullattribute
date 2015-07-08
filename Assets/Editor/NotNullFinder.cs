using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace RedBlueTools
{
	public class NotNullFinder : EditorWindow
	{
		static bool outputLogs = false;
		
		[MenuItem ("RedBlueTools/Not Null Finder")]
		public static void  SearchForAndErrorForNotNullViolations ()
		{
			Debug.Log ("Searching for null NotNull fields");
			// Search for and error for prefabs with null RequireWire fields
			string[] guidsForAllGameObjects = AssetDatabase.FindAssets ("t:GameObject");
			foreach (string guid in guidsForAllGameObjects) {
				Log ("Loading GUID: " + guid);
				string pathToGameObject = AssetDatabase.GUIDToAssetPath (guid);
				Log ("Loading Asset for guid at path: " + pathToGameObject);
				GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath (pathToGameObject, typeof(GameObject));
				
				ErrorForNullRequiredWiresOnGameObject (gameObject, pathToGameObject);
			}


			// Search the scene objects (only need root game objects since children will be searched)
			GameObject[] sceneGameObjects = (GameObject[])GameObject.FindObjectsOfType (typeof(GameObject));
			List<GameObject> rootSceneGameObjects = new List<GameObject> ();
			foreach (GameObject sceneGameObject in sceneGameObjects) {
				if (sceneGameObject.transform.parent == null) {
					rootSceneGameObjects.Add (sceneGameObject);
				}
			}
			foreach (GameObject rootGameObjectInScene in rootSceneGameObjects) {
				ErrorForNullRequiredWiresOnGameObject (rootGameObjectInScene, "In current scene.");
			}

			Debug.Log ("NotNull search complete");
		}

		static void ErrorForNullRequiredWiresOnGameObject (GameObject gameObject, string pathToAsset)
		{
			List<ErrorGameObject> erroringObjects = new List<ErrorGameObject> ();
			TraverseGameObjectHierarchyForErrors (gameObject, pathToAsset, ref erroringObjects);
			foreach (ErrorGameObject errorObject in erroringObjects) {
				errorObject.OutputError ();
			}
			return;
		}

		static void TraverseGameObjectHierarchyForErrors (GameObject obj, string assetPath, ref List<ErrorGameObject> errorings)
		{ 
			if (ErrorGameObject.ObjectHasErrors (obj)) {
				errorings.Add (new ErrorGameObject (obj, assetPath));
			}
			foreach (Transform child in obj.transform) {
				TraverseGameObjectHierarchyForErrors (child.gameObject, assetPath, ref errorings);
			}
		}

		static void Log (string log)
		{
			if (outputLogs == false) {
				return;
			}
			Debug.Log (log);
		}

		class ErrorGameObject
		{
			public GameObject errorObject;
			public List<ErrorMonoBehaviour> errorMonoBehaviours;
			public string AssetPath;
			public string FullName
			{
				get {
					Transform currentParent = errorObject.transform.parent;
					string fullName = errorObject.name;
					while (currentParent != null) {
						fullName = currentParent.gameObject.name + "/" + fullName;
						currentParent = currentParent.transform.parent;
					}
					return fullName;
				}
			}

			public int NumErroringMonoBehaviours {
				get {
					if (errorMonoBehaviours == null) {
						return 0;
					} else {
						return errorMonoBehaviours.Count;
					}
				}
			}

			public ErrorGameObject (GameObject sourceObject, string assetPath)
			{
				this.errorObject = sourceObject;
				this.AssetPath = assetPath;

				MonoBehaviour[] monobehaviours = errorObject.GetComponents<MonoBehaviour> ();
				this.errorMonoBehaviours = new List<ErrorMonoBehaviour> ();
				for (int i = 0; i < monobehaviours.Length; i++) {
					ErrorMonoBehaviour errors = new ErrorMonoBehaviour (monobehaviours [i]);
					if (errors.NumErrorFields > 0) {
						this.errorMonoBehaviours.Add (errors);
					}
				}
			}

			public static bool ObjectHasErrors (GameObject gameObject)
			{
				MonoBehaviour[] monobehaviours = gameObject.GetComponents<MonoBehaviour> ();
				for (int i = 0; i < monobehaviours.Length; i++) {
					if (ErrorMonoBehaviour.MonoBehaviourHasErrors ( monobehaviours[i])) {
						return true;
					}
				}

				return false;
			}

			public void OutputError ()
			{
				foreach (ErrorMonoBehaviour errorMB in errorMonoBehaviours) {
					foreach( ErrorField error in errorMB.errors) {
						Debug.LogError (string.Format ("NotNull field: {0} " +
						                               "has not been assigned on object: {1}\nPath: {2}",
						                               error.fieldInfo.Name, FullName, AssetPath), this.errorObject);
					}
				}
			}
		}

		class ErrorMonoBehaviour
		{
			public MonoBehaviour sourceBehaviour;
			public bool IsMissing;
			public List<ErrorField> errors;

			public int NumErrorFields {
				get {
					if (errors == null) {
						return 0;
					} else {
						return errors.Count;
					}
				}
			}

			public ErrorMonoBehaviour (MonoBehaviour sourceMB)
			{
				this.sourceBehaviour = sourceMB;
				this.IsMissing = sourceMB == null;

				if (sourceMB != null) {
					this.errors = new List<ErrorField> ();
					List<FieldInfo> erroringFields = GetErroringFields (sourceMB);
					foreach (FieldInfo erroringField in erroringFields) {
						this.errors.Add (new ErrorField (erroringField, sourceMB));
					}
				}
			}

			static List<FieldInfo> GetErroringFields (MonoBehaviour sourceMB)
			{
				List<FieldInfo> erroringFields = new List<FieldInfo> ();
				List<FieldInfo> notNullFields = ReflectionUtilities.GetMonoBehaviourFieldsWithAttribute<NotNullAttribute> (sourceMB);
				foreach (FieldInfo notNullField in notNullFields) {
					object fieldObject = notNullField.GetValue (sourceMB);
					if (fieldObject == null || fieldObject.Equals (null)) {
						erroringFields.Add (notNullField);
					}
				}

				return erroringFields;
			}

			public static bool MonoBehaviourHasErrors (MonoBehaviour mb)
			{
				return GetErroringFields (mb).Count > 0;
			}
		}

		class ErrorField
		{
			public FieldInfo fieldInfo;
			public MonoBehaviour sourceBehaviour;

			public ErrorField (FieldInfo fieldInfo, MonoBehaviour sourceMB)
			{
				this.fieldInfo = fieldInfo;
				this.sourceBehaviour = sourceMB;
			}
		}
	}
}