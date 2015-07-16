using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
	public class NotNullInScene : MonoBehaviour
	{
		[NotNull (IgnorePrefab = true)]
		public GameObject
			RequiredObject;
	}
}