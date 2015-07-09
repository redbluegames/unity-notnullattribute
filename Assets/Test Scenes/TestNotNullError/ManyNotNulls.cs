using UnityEngine;
using System.Collections;

public class ManyNotNulls : MonoBehaviour
{
	public GameObject ObjectReferenceNoAttribute;
	[NotNull]
	public GameObject
		RequiredObjectA;
	[NotNull (IgnorePrefab = true)]
	public GameObject
		RequiredObjectB;
	GameObject privateReference;
	[NotNull]
	public Animator
		RequiredAnimator;
}
