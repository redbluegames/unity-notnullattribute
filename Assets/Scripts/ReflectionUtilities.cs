using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public static class ReflectionUtilities
{
	public static List<FieldInfo> GetMonoBehaviourFieldsWithAttribute<T> (MonoBehaviour mb, 
	                                                                      BindingFlags reflectionFlags = BindingFlags.Default)
	{
		List<FieldInfo> fieldsWithAttribute = new List<FieldInfo> ();
		Type mbType = mb.GetType ();
		FieldInfo[] allFields;
		if (reflectionFlags == BindingFlags.Default) {
			allFields = mbType.GetFields ();
		} else {
			allFields = mbType.GetFields (reflectionFlags);
		}
		foreach (FieldInfo fieldInfo in allFields) {
			foreach (Attribute attribute in Attribute.GetCustomAttributes (fieldInfo)) {
				if (attribute.GetType () == typeof(T)) {
					fieldsWithAttribute.Add (fieldInfo);
					break;
				}
			}
		}

		return fieldsWithAttribute;
	}
}
