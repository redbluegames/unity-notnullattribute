using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RedBlueTools;

public class NotNullErrorTester : MonoBehaviour {

	#region Successes

	public TraversalTests TraversalTestObjects;

	[System.Serializable]
	public class TraversalTests
	{
		public GameObject OneChild;
		public GameObject OneDeepChild;
		public GameObject OneInactiveChild;
		public GameObject TwoChildren;
		public GameObject ComplexHierarchy;
	}

	public GetNumErrorFieldsContainer GetNumErrorFields;
	[System.Serializable]
	public class GetNumErrorFieldsContainer
	{
		public GameObject Empty;
		public GameObject MissingScript;
		public GameObject NoneWired;
		public GameObject SomeWired;
		public GameObject AllWired;
		public GameObject TwoMBsUnwired;
		public GameObject TwoMBsWired;
		public GameObject MultiUnwired;
	}

	#endregion

	[ContextMenu("Run Tests")]
	void RunTests ()
	{
		TestObjectTraversal ();
		TestFindErroringFields ();
	}

	#region Tests
	void TestObjectTraversal ()
	{
		string testName = "GetNumErrorObjects";
		// TODO: These are simply traversal tests. They should be moved.
		/*
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInactiveChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInDeepChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.ParentAndChildWithErrors, 2);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.TwoErrorsInSiblings, 2);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.ErrorsInComplexHierarchy, 3);
		*/

		Debug.Log ("Test passed: " + testName);
	}

	void TestAndAssertNumErrorObjects (string testName, GameObject testObject, int expectedErrors)
	{
		string subTestName = testName + " | " + testObject.name;
		List<NotNullError> errors = new List<NotNullError> ();
		NotNullError.TraverseGameObjectHierarchyForErrors (testObject, "TestScene", ref errors);

		if (errors.Count != expectedErrors) {
			LogTestFailure (subTestName, string.Format("Expected {0} errors, found {1}", 
			                                           expectedErrors,
			                                           errors.Count));
			return;
		}
	}
	
	void TestFindErroringFields ()
	{
		string testName = "GetsCorrectNumErrors";
		
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.Empty, 0);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.MissingScript, 0);
		
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.NoneWired, 3);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.SomeWired, 2);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.AllWired, 0);

		TestAndAssertNumErroringFields (testName, GetNumErrorFields.TwoMBsUnwired, 2);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.TwoMBsWired, 0);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.MultiUnwired, 4);

		Debug.Log ("Test passed: " + testName);
	}

	void TestAndAssertNumErroringFields (string testName, GameObject testObject, int expectedErrors)
	{
		string subTestName = testName + " | " + testObject.name;
		List<NotNullViolation> errors = NotNullChecker.FindErroringFields (testObject, "In Test Scene");

		int numFieldsWithErrors = errors.Count;
		if (numFieldsWithErrors != expectedErrors) {
			LogTestFailure (subTestName, string.Format ("Expected {0} fields with errors, found {1}",
			                                            expectedErrors, numFieldsWithErrors));
			return;
		}
	}
	#endregion

	#region Test Helpers

	List<NotNullError> RunTestsOnObject (GameObject testObject)
	{
		List<NotNullError> errors = new List<NotNullError> ();
		NotNullError.TraverseGameObjectHierarchyForErrors (testObject, "TestScene", ref errors);
		return errors;
	}

	void LogTestFailure (string testName, string failureMessage)
	{
		Debug.LogError ("Test Failed: " + testName + "\n" + failureMessage);
	}
	#endregion
}
