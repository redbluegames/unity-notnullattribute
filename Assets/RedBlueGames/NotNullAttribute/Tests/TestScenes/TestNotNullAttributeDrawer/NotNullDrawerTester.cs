using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RedBlueGames.NotNull.Tests
{
	public class NotNullDrawerTester : MonoBehaviour
	{
		public int IntegerFieldNoAttribute;
		[NotNull]
		public int
			IntegerField;
		public GameObject ObjectReferenceNoAttribute;
		[NotNull]
		public GameObject
			RequiredObjectOne;
		[NotNull]
		public GameObject
			RequiredObjectTwo;
		[NotNull (IgnorePrefab = true)]
		public GameObject
			RequiredInScene;
		public string PublicString;
		[NotNull (IgnorePrefab = true)]
		public GameObject
			RequiredObjectInScene;
		[NotNullAttribute]
		public List<int> 
			AttributeOnList;
		[NotNullAttribute]
		public GameObject[]
			AttributeOnArray = new GameObject[5];
	}
}