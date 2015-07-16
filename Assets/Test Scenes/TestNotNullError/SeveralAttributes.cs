using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
	public class SeveralAttributes : MonoBehaviour
	{
		[Tooltip ("A Required Object Field")]
		[SerializeField]
		[NotNull]
		public GameObject
			RequiredObject;
	}
}