using UnityEngine;
using System.Collections;

namespace RedBlueGames.NotNull.Tests
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