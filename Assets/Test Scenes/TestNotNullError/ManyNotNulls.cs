using UnityEngine;
using System.Collections;

public class ManyNotNulls : MonoBehaviour
{
	public GameObject ObjectReferenceNoAttribute;
	[NotNull]
	public GameObject
		RequiredObjectA;
	[NotNull]
	public GameObject
		RequiredObjectB;
	GameObject privateReference;
	[NotNull]
	public Animator
		RequiredAnimator;
}
