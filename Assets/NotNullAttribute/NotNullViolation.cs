using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace RedBlueTools
{
	public class NotNullViolation
	{
		public FieldInfo FieldInfo;
		public GameObject ErrorGameObject;
		public MonoBehaviour SourceMonoBehaviour;
		public bool AllowNullAsPrefab;
		
		public NotNullViolation (FieldInfo fieldInfo, MonoBehaviour sourceMB, bool allowNullAsPrefab = false)
		{
			this.FieldInfo = fieldInfo;
			this.SourceMonoBehaviour = sourceMB;
			this.ErrorGameObject = sourceMB.gameObject;
			this.AllowNullAsPrefab = allowNullAsPrefab;
		}
		
		public string FullName {
			get {
				Transform currentParent = ErrorGameObject.transform.parent;
				string fullName = ErrorGameObject.name;
				while (currentParent != null) {
					fullName = currentParent.gameObject.name + "/" + fullName;
					currentParent = currentParent.transform.parent;
				}
				return fullName;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[NotNullViolation: Field={0}, FullName={1}]", FieldInfo.Name, FullName);
		}
	}
}

