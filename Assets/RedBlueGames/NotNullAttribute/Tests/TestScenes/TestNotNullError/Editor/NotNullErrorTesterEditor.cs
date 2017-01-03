using RedBlueGames.NotNull;
using RedBlueGames.NotNull.Tests;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(NotNullErrorTester))]
public class NotNullErrorTesterEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		this.DrawDefaultInspector();

		if (GUILayout.Button("Run Tests"))
		{
			this.TestFindErroringFields();
		}
	}

	private void TestFindErroringFields ()
	{
		var tester = (NotNullErrorTester)this.target;
		string testName = "GetsCorrectNumErrors";

		TestAndAssertNumErroringFields (testName, tester.Empty, 0);
		TestAndAssertNumErroringFields (testName, tester.MissingScript, 0);

		TestAndAssertNumErroringFields (testName, tester.NoneWired, 3);
		TestAndAssertNumErroringFields (testName, tester.SomeWired, 2);
		TestAndAssertNumErroringFields (testName, tester.AllWired, 0);

		TestAndAssertNumErroringFields (testName, tester.TwoMBsUnwired, 2);
		TestAndAssertNumErroringFields (testName, tester.TwoMBsWired, 0);
		TestAndAssertNumErroringFields (testName, tester.MultiUnwired, 4);

		TestAndAssertNumErroringFields (testName, tester.NotNullInScenePrefab, 0);
		TestAndAssertNumErroringFields (testName, tester.WiredNotNullInScene, 0);
		TestAndAssertNumErroringFields (testName, tester.UnwiredNotNullInScene, 1);

		TestAndAssertNumErroringFields (testName, tester.UnwiredDerivedMB, 2);
		TestAndAssertNumErroringFields (testName, tester.WiredDerivedMB, 0);

		TestAndAssertNumErroringFields (testName, tester.WiredMultipleAttributesMB, 0);

		Debug.Log ("Test passed: " + testName);
	}

	private void TestAndAssertNumErroringFields (string testName, GameObject testObject, int expectedErrors)
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

	private void LogTestFailure (string testName, string failureMessage)
	{
		Debug.LogError ("Test Failed: " + testName + "\n" + failureMessage);
	}
}
