using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RedBlueTools;

public class NotNullErrorTester : MonoBehaviour {

	#region Successes
	public GameObject NoNotNull_NoMonobehaviour;
	public GameObject NoNotNull_OneMonobehaviour;
	public GameObject NoNotNull_MultipleMonoBehaviours;

	public GameObject Wired_OneMonobehaviour;
	public GameObject Wired_ManyNotNulls;
	public GameObject Wired_TwoMonobehaviours;
	public GameObject Wired_InactiveObject;

	public GameObject Wired_ParentHasChild;
	public GameObject Wired_ParentWithDeepChild;
	public GameObject Wired_ParentAndChild;
	public GameObject Wired_ParentWithInactiveChild;
	#endregion

	[ContextMenu("Run Tests")]
	void RunTests ()
	{
		Test_NoNotNullAttributes_NoMonobehaviour ();
		Test_NoNotNullAttributes_OneMonobehaviour ();
		Test_NoNotNullAttributes_MultipleMonoBehaviours ();

		Test_Wired_OneMonobehaviour ();
		Test_Wired_ManyNotNulls ();
		Test_Wired_TwoMonobehaviours ();
		Test_Wired_InactiveGameObject ();

		Test_Wired_ParentHasChild ();
		Test_Wired_ParentAndChild ();
		Test_Wired_ParentWithDeepChild ();
		Test_Wired_ParentWithInactiveChild ();

	//	List<FieldInfo> fieldsWithAttribute = ReflectionUtilities.GetMonoBehaviourFieldsWithAttribute<SerializeField> (testBehaviour);
	//	LogFieldInfoList (fieldsWithAttribute);
	}

	#region Tests
	void Test_NoNotNullAttributes_NoMonobehaviour ()
	{
		GameObject testGameObject = NoNotNull_NoMonobehaviour;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_NoNotNullAttributes_NoMonobehaviour");
	}

	void Test_NoNotNullAttributes_OneMonobehaviour ()
	{
		GameObject testGameObject = NoNotNull_OneMonobehaviour;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_NoNotNullAttributes_OneMonobehaviour");
	}

	void Test_NoNotNullAttributes_MultipleMonoBehaviours ()
	{
		GameObject testGameObject = NoNotNull_MultipleMonoBehaviours;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_NoNotNullAttributes_MultipleMonoBehaviours");
	}
	
	void Test_Wired_OneMonobehaviour ()
	{
		GameObject testGameObject = Wired_OneMonobehaviour;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_OneMonobehaviour");
	}

	void Test_Wired_ManyNotNulls ()
	{
		GameObject testGameObject = Wired_ManyNotNulls;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_ManyNotNulls");
	}

	void Test_Wired_TwoMonobehaviours ()
	{
		GameObject testGameObject = Wired_TwoMonobehaviours;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_TwoMonobehaviours");
	}

	void Test_Wired_InactiveGameObject ()
	{
		GameObject testGameObject = Wired_InactiveObject;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_InactiveGameObject");
	}

	void Test_Wired_ParentHasChild ()
	{
		GameObject testGameObject = Wired_ParentHasChild;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_ParentHasChild");
	}
	
	void Test_Wired_ParentAndChild ()
	{
		GameObject testGameObject = Wired_ParentAndChild;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_ParentAndChild");
	}

	void Test_Wired_ParentWithDeepChild ()
	{
		GameObject testGameObject = Wired_ParentWithDeepChild;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_ParentWithDeepChild");
	}
	
	void Test_Wired_ParentWithInactiveChild ()
	{
		GameObject testGameObject = Wired_ParentWithInactiveChild;
		AssertNoErrors (RunTestsOnObject (testGameObject), 
		                "Test_Wired_ParentWithInactiveChild");
	}
	#endregion

	#region Test Helpers

	List<NotNullError> RunTestsOnObject (GameObject testObject)
	{
		List<NotNullError> errors = new List<NotNullError> ();
		NotNullError.TraverseGameObjectHierarchyForErrors (testObject, "TestScene", ref errors);
		return errors;
	}

	void AssertNoErrors (List<NotNullError> errorResults, string testName)
	{
		if (errorResults.Count > 0) {
			Debug.LogError ("Test failed: " + testName);
		} else {
			Debug.Log ("Test passed:" + testName);
		}
	}
	#endregion
}
