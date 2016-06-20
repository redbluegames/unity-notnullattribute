namespace RedBlueGames.NotNull
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Class that contains logic to find NotNull violations when pressing Play from the Editor
    /// </summary>
    [InitializeOnLoad]
    public class FindNotNullsOnLaunch
    {
        static FindNotNullsOnLaunch()
        {
            if (Debug.isDebugBuild)
            {
                // Searching on first launch seemed to execute before references were wired up on scene objects.
                EditorApplication.update += RunOnce;
            }
        }

        private static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            RedBlueGames.NotNull.NotNullFinder.SearchForAndErrorForNotNullViolations();
        }
    }
}