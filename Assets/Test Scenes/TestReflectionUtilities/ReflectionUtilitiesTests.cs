using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using RedBlue.Extensions;

namespace RedBlue.NotNull.Tests
{
	[ExecuteInEditMode]
	public class ReflectionUtilitiesTests : MonoBehaviour
	{
		public MonoBehaviour testBehaviour;
		
		[ContextMenu("Run Tests")]
		void RunTests ()
		{
			List<FieldInfo> fieldsWithAttribute = 
				ReflectionUtilities.GetFieldsWithAttributeFromType<SerializeField> (testBehaviour.GetType ());
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
}