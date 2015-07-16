using UnityEngine;
using System.Collections;

namespace RedBlueTools
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