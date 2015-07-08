using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	public class SimpleNotNull : MonoBehaviour
	{
		public GameObject ObjectReferenceNoAttribute;
		[NotNull]
		public GameObject
			RequiredObject;
	}
}