using RedBlueGames.NotNull;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBlueGames.NotNull.Tests
{
	public class NotNullErrorTester : MonoBehaviour
	{
		public GameObject Empty;
		public GameObject MissingScript;
		public GameObject NoneWired;
		public GameObject SomeWired;
		public GameObject AllWired;
		public GameObject TwoMBsUnwired;
		public GameObject TwoMBsWired;
		public GameObject MultiUnwired;
		public GameObject NotNullInScenePrefab;
		public GameObject WiredNotNullInScene;
		public GameObject UnwiredNotNullInScene;
		public GameObject UnwiredDerivedMB;
		public GameObject WiredDerivedMB;
		public GameObject WiredMultipleAttributesMB;
	}
}