using UnityEngine;
using System.Collections;

public class ChildClassWithNotNulls : ParentClassWithNotNulls
{
	[Header ("Child Fields")]
	[NotNull]
	public GameObject
		ChildRequiredObject;
}
