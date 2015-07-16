using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace RedBlue.NotNull
{
	public class NotNullViolation
	{
		public FieldInfo FieldInfo;
		public GameObject ErrorGameObject;
		public MonoBehaviour SourceMonoBehaviour;
		
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

		
		public NotNullViolation (FieldInfo fieldInfo, MonoBehaviour sourceMB)
		{
			this.FieldInfo = fieldInfo;
			this.SourceMonoBehaviour = sourceMB;
			this.ErrorGameObject = sourceMB.gameObject;
		}

		public override string ToString ()
		{
			return string.Format ("[NotNullViolation: Field={0}, FullName={1}]", FieldInfo.Name, FullName);
		}
	}
}

