﻿using UnityEngine;
using System.Collections;

namespace RedBlue.NotNull.Tests
{
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
}