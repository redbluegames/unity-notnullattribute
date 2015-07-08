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
			List<NotNullError> erroringObjects = new List<NotNullError> ();
			TraverseGameObjectHierarchyForErrors (gameObject, pathToAsset, ref erroringObjects);
			foreach (NotNullError errorObject in erroringObjects) {
				errorObject.OutputError ();
			}
			return;
		}

		static void TraverseGameObjectHierarchyForErrors (GameObject obj, string assetPath, ref List<NotNullError> errorings)
		{ 
			if (NotNullError.ObjectHasErrors (obj)) {
				errorings.Add (new NotNullError (obj, assetPath));
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
	}
}