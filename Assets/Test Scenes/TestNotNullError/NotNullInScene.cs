using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	public class NotNullInScene : MonoBehaviour
	{
		[NotNull (IgnorePrefab = true)]
		public GameObject
			RequiredObject;
	}
}