using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	public class TestMonobehaviour : MonoBehaviour
	{
	#region int field test
		int noAttriuteIntPrivate;
		public int noAttributeIntegar;
		[SerializeField]
		int
			intPrivate;
		[SerializeField]
		public int
			IntPublic;
	
		// TODO: Multiple field test
		//[SerializeField]
		//public int IntMultipleFieldsPublic;
	#endregion

	#region Custom Class field test
		CustomClass noAttributeCustomClassPrivate;
		public CustomClass noAttributeCustomClassPublic;
		[SerializeField]
		CustomClass
			customClassPrivate;
		[SerializeField]
		public CustomClass
			CustomClassPublic;
	#endregion

		public void PublicMethodNoAttribute ()
		{
		}

		public class CustomClass
		{
			int x;
		}
	}
}