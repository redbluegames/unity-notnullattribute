using UnityEngine;
using System.Collections;

namespace RedBlueGames.NotNull.Tests
{
	public class ChildClassWithNotNulls : ParentClassWithNotNulls
	{
		[Header ("Child Fields")]
		[NotNull]
		public GameObject
			ChildRequiredObject;
	}
}