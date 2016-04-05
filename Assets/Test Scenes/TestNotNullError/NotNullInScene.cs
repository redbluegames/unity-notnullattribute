using UnityEngine;
using System.Collections;

namespace RedBlueGames.NotNull.Tests
{
	public class NotNullInScene : MonoBehaviour
	{
		[NotNull (IgnorePrefab = true)]
		public GameObject
			RequiredObject;
	}
}