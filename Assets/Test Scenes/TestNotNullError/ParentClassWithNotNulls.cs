using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
	public class ParentClassWithNotNulls : MonoBehaviour
	{
		[Header ("Parent Fields")]
		[NotNull]
		public GameObject
			ParentRequiredObject;
	}
}