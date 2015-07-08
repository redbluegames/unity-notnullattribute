using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RedBlueTools;

public class NotNullErrorTester : MonoBehaviour {

	#region Successes

	public GetNumErrorObjectsContainer GetNumErrorObjects;

	[System.Serializable]
	public class GetNumErrorObjectsContainer
	{
		public GameObject Empty;
		public GameObject MissingScript;
		public GameObject NoNotNulls;
		public GameObject Wired;
		public GameObject OneError;
		public GameObject OneErrorTwoUnwiredMonoBehaviours;
		public GameObject OneErrorTwoMonoBehaviours;
		public GameObject OneErrorInChild;
		public GameObject OneErrorInDeepChild;
		public GameObject OneErrorInactiveChild;
		public GameObject ParentAndChildWithErrors;
		public GameObject TwoErrorsInSiblings;
		public GameObject ErrorsInComplexHierarchy;
	}

	public GetNumErrorFieldsContainer GetNumErrorFields;
	[System.Serializable]
	public class GetNumErrorFieldsContainer
	{
		public GameObject NoneWired;
		public GameObject SomeWired;
		public GameObject AllWired;
		public GameObject MultiUnwired;
	}

	#endregion

	[ContextMenu("Run Tests")]
	void RunTests ()
	{
		Test_GetNumErrorObjects ();
		Test_GetsCorrectNumErroringFields ();
	}

	#region Tests
	void Test_GetNumErrorObjects ()
	{
		string testName = "GetNumErrorObjects";
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.Empty, 0);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.MissingScript, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.NoNotNulls, 0);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.Wired, 0);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneError, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorTwoMonoBehaviours, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorTwoUnwiredMonoBehaviours, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInactiveChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.OneErrorInDeepChild, 1);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.ParentAndChildWithErrors, 2);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.TwoErrorsInSiblings, 2);
		TestAndAssertNumErrorObjects (testName, GetNumErrorObjects.ErrorsInComplexHierarchy, 3);

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
	
	void Test_GetsCorrectNumErroringFields ()
	{
		string testName = "GetsCorrectNumErrors";

		TestAndAssertNumErroringFields (testName, GetNumErrorFields.NoneWired, 3);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.SomeWired, 2);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.AllWired, 0);
		TestAndAssertNumErroringFields (testName, GetNumErrorFields.MultiUnwired, 4);

		Debug.Log ("Test passed: " + testName);
	}

	void TestAndAssertNumErroringFields (string testName, GameObject testObject, int expectedErrors)
	{
		string subTestName = testName + " | " + testObject.name;
		List<NotNullError> errors = new List<NotNullError> ();
		NotNullError.TraverseGameObjectHierarchyForErrors (testObject, "TestScene", ref errors);

		int numFieldsWithErrors = errors.Count == 0 ? 0 : errors [0].NumFieldsWithErrors;
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
