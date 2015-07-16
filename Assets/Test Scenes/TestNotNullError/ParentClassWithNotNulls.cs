using UnityEngine;
using System.Collections;

public class ParentClassWithNotNulls : MonoBehaviour
{
	[Header ("Parent Fields")]
	[NotNull]
	public GameObject
		ParentRequiredObject;
}
