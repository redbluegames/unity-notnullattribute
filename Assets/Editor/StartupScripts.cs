using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class StartupScripts {
	
	static StartupScripts ()
	{
		// Comment out return to run on save.
		return;
		if(Debug.isDebugBuild) {
			// Running immediately seemed to execute before references were wired up
			EditorApplication.update += RunOnce;
		}
	}
	
	static void RunOnce ()
	{
		RedBlueTools.NotNullFinder.SearchForAndErrorForNotNullViolations ();
		EditorApplication.update -= RunOnce;
	}
}