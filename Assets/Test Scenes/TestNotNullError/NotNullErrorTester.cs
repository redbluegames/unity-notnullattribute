using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RedBlue.NotNull.Tests
{
	public class NotNullErrorTester : MonoBehaviour
	{

	#region Test Objects
		public GameObject Empty;
		public GameObject MissingScript;
		public GameObject NoneWired;
		public GameObject SomeWired;
		public GameObject AllWired;
		public GameObject TwoMBsUnwired;
		public GameObject TwoMBsWired;
		public GameObject MultiUnwired;
		public GameObject NotNullInScenePrefab;
		public GameObject WiredNotNullInScene;
		public GameObject UnwiredNotNullInScene;
		public GameObject UnwiredDerivedMB;
		public GameObject WiredDerivedMB;
		public GameObject WiredMultipleAttributesMB;

	#endregion

		[ContextMenu("Run Tests")]
		void RunTests ()
		{
			TestFindErroringFields ();
		}

	#region Tests
		void TestFindErroringFields ()
		{
			string testName = "GetsCorrectNumErrors";
		
			TestAndAssertNumErroringFields (testName, Empty, 0);
			TestAndAssertNumErroringFields (testName, MissingScript, 0);
		
			TestAndAssertNumErroringFields (testName, NoneWired, 3);
			TestAndAssertNumErroringFields (testName, SomeWired, 2);
			TestAndAssertNumErroringFields (testName, AllWired, 0);

			TestAndAssertNumErroringFields (testName, TwoMBsUnwired, 2);
			TestAndAssertNumErroringFields (testName, TwoMBsWired, 0);
			TestAndAssertNumErroringFields (testName, MultiUnwired, 4);

			TestAndAssertNumErroringFields (testName, NotNullInScenePrefab, 0);
			TestAndAssertNumErroringFields (testName, WiredNotNullInScene, 0);
			TestAndAssertNumErroringFields (testName, UnwiredNotNullInScene, 1);
		
			TestAndAssertNumErroringFields (testName, UnwiredDerivedMB, 2);
			TestAndAssertNumErroringFields (testName, WiredDerivedMB, 0);

			TestAndAssertNumErroringFields (testName, WiredMultipleAttributesMB, 0);

			Debug.Log ("Test passed: " + testName);
		}

		void TestAndAssertNumErroringFields (string testName, GameObject testObject, int expectedErrors)
		{
			string subTestName = testName + " | " + testObject.name;
			List<NotNullViolation> errors = NotNullChecker.FindErroringFields (testObject);

			int numFieldsWithErrors = errors.Count;
			if (numFieldsWithErrors != expectedErrors) {
				LogTestFailure (subTestName, string.Format ("Expected {0} fields with errors, found {1}",
			                                            expectedErrors, numFieldsWithErrors));
				return;
			}
		}
	#endregion

	#region Test Helpers
		void LogTestFailure (string testName, string failureMessage)
		{
			Debug.LogError ("Test Failed: " + testName + "\n" + failureMessage);
		}
	#endregion
	}
}