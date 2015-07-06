using UnityEngine;
using System.Collections;

public class TestMBWithNotNull : MonoBehaviour {

	public int IntegerFieldNoAttribute;
	[NotNullAttribute]
	public int IntegerField;
	public GameObject ObjectReferenceNoAttribute;
	[NotNullAttribute]
	public GameObject RequiredObject;
}
