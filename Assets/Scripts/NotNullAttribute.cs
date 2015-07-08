using UnityEngine;

[System.AttributeUsage (System.AttributeTargets.Field)]
public class NotNullAttribute : PropertyAttribute {
}

[System.AttributeUsage (System.AttributeTargets.Field)]
public class NotNullInSceneAttribute : PropertyAttribute {
}