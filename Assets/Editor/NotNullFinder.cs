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
			Log ("Searching for monobehaviours on game object: " + gameObject.name);
			MonoBehaviour[] monobehaviours = gameObject.GetComponentsInChildren<MonoBehaviour> (true);
			foreach (MonoBehaviour mb in monobehaviours) {
				if (mb == null) {
					Debug.LogWarning ("Found MonoBehaviour that can't be loaded on object or child object of: " + 
						gameObject.name + "\nPath: " + pathToAsset, gameObject);
				} else {
					// Get full path to child game object from root
					string mbPath = mb.gameObject.name;
					Transform currentParent = mb.transform.parent;
					while (currentParent != null) {
						mbPath = currentParent.gameObject.name + "/" + mbPath;
						currentParent = currentParent.transform.parent;
					}
					List<FieldInfo> notNullFields = ReflectionUtilities.GetMonoBehaviourFieldsWithAttribute<NotNullAttribute> (mb);
					foreach (FieldInfo notNullField in notNullFields) {
						object fieldObject = notNullField.GetValue (mb);
						if (fieldObject == null || fieldObject.Equals (null)) {
							Debug.LogError (string.Format ("NotNull field: {0} " +
								"has not been assigned on object: {1}\nPath: {2}",
						    notNullField.Name, mbPath, pathToAsset), mb.gameObject);
						}
					}
				}
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