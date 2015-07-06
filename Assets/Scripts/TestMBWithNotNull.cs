using UnityEngine;
using System.Collections;

public class TestMBWithNotNull : MonoBehaviour {

	public int IntegerFieldNoAttribute;
	[NotNullAttribute]
	public int IntegerField;
	public GameObject ObjectReferenceNoAttribute;
	[NotNull]
	public GameObject RequiredObject;
	[NotNullAttribute]
	public GameObject RequiredObjectInScene;
}
