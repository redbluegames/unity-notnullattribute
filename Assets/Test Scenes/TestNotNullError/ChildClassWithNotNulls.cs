using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
	public class ChildClassWithNotNulls : ParentClassWithNotNulls
	{
		[Header ("Child Fields")]
		[NotNull]
		public GameObject
			ChildRequiredObject;
	}
}