using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class TestReflectionUtilities : MonoBehaviour {

	public MonoBehaviour testBehaviour;

	void Start () {

		List<FieldInfo> fieldsWithAttribute = ReflectionUtilities.GetMonoBehaviourFieldsWithAttribute<SerializeField> (testBehaviour);
		LogFieldInfoList (fieldsWithAttribute);
	}

	void LogFieldInfoList (List<FieldInfo> list)
	{
		Debug.Log ("Logging List:");
		foreach (FieldInfo field in list) {
			Debug.Log ("Field: " + field.Name);
		}
	}
}
