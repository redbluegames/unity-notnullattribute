using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
	public class MockMBReflection : MonoBehaviour
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
		
		[SerializeField]
		[NotNullAttribute]
		public int IntMultipleFieldsPublic;
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