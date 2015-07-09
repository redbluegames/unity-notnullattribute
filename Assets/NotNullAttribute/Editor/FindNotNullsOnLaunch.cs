using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class FindNotNullsOnLaunch {
	
	static FindNotNullsOnLaunch ()
	{
		if(Debug.isDebugBuild) {
			RedBlueTools.NotNullFinder.SearchForAndErrorForNotNullViolations ();
		}
	}
}