using UnityEngine;
using System.Collections;

public class NotNullAndNotNullInScene : MonoBehaviour {

	[NotNullAttribute]
	[NotNullInSceneAttribute]
	public GameObject RequiredObjectA;
}
