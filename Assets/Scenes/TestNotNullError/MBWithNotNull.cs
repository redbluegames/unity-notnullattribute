using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	public class MBWithNotNull : MonoBehaviour
	{
		public GameObject ObjectReferenceNoAttribute;
		[NotNull]
		public GameObject
			RequiredObject;
	}
}