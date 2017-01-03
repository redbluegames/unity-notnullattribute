namespace RedBlueGames.NotNull
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using RedBlueGames.NotNull;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// NotNullFinder fires off checks for NotNull violations in the scene and asset database
    /// and reports their errors.
    /// </summary>
    public class NotNullFinder : EditorWindow
    {
        private static bool outputLogs = false;

        /// <summary>
        /// Searchs for and error for not null violations in the scene and asset database
        /// </summary>
        [MenuItem("RedBlueTools/Not Null Finder")]
        public static void SearchForAndErrorForNotNullViolations()
        {
            // Debug.Log ("Searching for null NotNull fields");
            // Search for and error for prefabs with null RequireWire fields
            string[] guidsForAllGameObjects = AssetDatabase.FindAssets("t:GameObject");
            foreach (string guid in guidsForAllGameObjects)
            {
                Log("Loading GUID: " + guid);
                string pathToGameObject = AssetDatabase.GUIDToAssetPath(guid);

                // Skip test assets. This should be done using asset settings in the future.
                if (pathToGameObject.Contains("RedBlueGames/NotNullAttribute/Tests"))
                {
                    continue;
                }

                Log("Loading Asset for guid at path: " + pathToGameObject);
                GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(pathToGameObject, typeof(GameObject));

                ErrorForNullRequiredWiresOnGameObject(gameObject, pathToGameObject);
            }

            // Search the scene objects (only need root game objects since children will be searched)
            GameObject[] sceneGameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            List<GameObject> rootSceneGameObjects = new List<GameObject>();
            foreach (GameObject sceneGameObject in sceneGameObjects)
            {
                if (sceneGameObject.transform.parent == null)
                {
                    rootSceneGameObjects.Add(sceneGameObject);
                }
            }

            foreach (GameObject rootGameObjectInScene in rootSceneGameObjects)
            {
                ErrorForNullRequiredWiresOnGameObject(rootGameObjectInScene, "In current scene.");
            }

            // Debug.Log ("NotNull search complete");
        }

        private static void ErrorForNullRequiredWiresOnGameObject(GameObject gameObject, string pathToAsset)
        {
            List<NotNullViolation> errorsOnGameObject = NotNullChecker.FindErroringFields(gameObject);
            foreach (NotNullViolation violation in errorsOnGameObject)
            {
                Debug.LogError(violation + "\nPath: " + pathToAsset, violation.ErrorGameObject);
            }

            foreach (Transform child in gameObject.transform)
            {
                ErrorForNullRequiredWiresOnGameObject(child.gameObject, pathToAsset);
            }
        }

        private static void Log(string log)
        {
            if (outputLogs == false)
            {
                return;
            }

            Debug.Log(log);
        }
    }
}