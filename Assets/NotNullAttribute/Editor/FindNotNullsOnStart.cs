using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class FindNotNullsOnStart {
	
	static FindNotNullsOnStart ()
	{
		if(Debug.isDebugBuild) {
			RedBlueTools.NotNullFinder.SearchForAndErrorForNotNullViolations ();
		}
	}
}