using UnityEngine;
using System.Collections;

namespace RedBlueGames.NotNull.Tests
{
	public class SimpleNotNull : MonoBehaviour
	{
		public GameObject ObjectReferenceNoAttribute;
		[NotNull]
		public GameObject
			RequiredObject;
	}
}