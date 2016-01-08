using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class FindNotNullsOnLaunch {
	
	static FindNotNullsOnLaunch ()
	{
		if(Debug.isDebugBuild) {
			// Searching on first launch seemed to execute before references were wired up on scene objects.
			EditorApplication.update += RunOnce;
		}
	}

	static void RunOnce ()
	{
		EditorApplication.update -= RunOnce;
		RedBlue.EditorTools.NotNullFinder.SearchForAndErrorForNotNullViolations ();
	}
}