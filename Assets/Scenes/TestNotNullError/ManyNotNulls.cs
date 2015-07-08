using UnityEngine;
using System.Collections;

public class ManyNotNulls : MonoBehaviour
{
	public GameObject ObjectReferenceNoAttribute;
	[NotNull]
	public GameObject
		RequiredObjectA;
	[NotNullInScene]
	public GameObject
		RequiredObjectB;
	GameObject privateReference;
	[NotNull]
	public Animator
		RequiredAnimator;
}
