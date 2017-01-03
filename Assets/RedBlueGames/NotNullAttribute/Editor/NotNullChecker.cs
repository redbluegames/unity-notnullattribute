namespace RedBlueGames.NotNull
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// This class is responsible for checking objects for NotNull violations.
    /// </summary>
    public class NotNullChecker
    {
        /// <summary>
        /// Finds the erroring NotNull fields on a GameObject.
        /// </summary>
        /// <returns>The erroring fields.</returns>
        /// <param name="sourceObject">Source object.</param>
        public static List<NotNullViolation> FindErroringFields(GameObject sourceObject)
        {
            List<NotNullViolation> erroringFields = new List<NotNullViolation>();
            MonoBehaviour[] monobehaviours = sourceObject.GetComponents<MonoBehaviour>();
            for (int i = 0; i < monobehaviours.Length; i++)
            {
                try
                {
                    if (MonoBehaviourHasErrors(monobehaviours[i]))
                    {
                        List<NotNullViolation> violationsOnMonoBehaviour = FindErroringFields(monobehaviours[i]);
                        erroringFields.AddRange(violationsOnMonoBehaviour);
                    }
                }
                catch (System.ArgumentNullException)
                {
                    // TODO: Handle missing monobehaviours
                }
            }

            return erroringFields;
        }

        private static List<NotNullViolation> FindErroringFields(MonoBehaviour sourceMB)
        {
            if (sourceMB == null)
            {
                throw new System.ArgumentNullException("MonoBehaviour is null. It likely references" +
                    " a script that's been deleted.");
            }

            List<NotNullViolation> erroringFields = new List<NotNullViolation>();

            // Add null NotNull fields
            List<FieldInfo> notNullFields = 
                ReflectionUtility.GetFieldsWithAttributeFromType<NotNullAttribute>(
                    sourceMB.GetType(),
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo notNullField in notNullFields)
            {
                object fieldObject = notNullField.GetValue(sourceMB);
                if (fieldObject == null || fieldObject.Equals(null))
                {
                    erroringFields.Add(new NotNullViolation(notNullField, sourceMB));
                }
            }

            // Remove NotNullViolations for prefabs with IgnorePrefab
            bool isObjectAPrefab = PrefabUtility.GetPrefabType(sourceMB.gameObject) == PrefabType.Prefab;
            List<NotNullViolation> violationsToIgnore = new List<NotNullViolation>();
            if (isObjectAPrefab)
            {
                // Find all violations that should be overlooked.
                foreach (NotNullViolation errorField in erroringFields)
                {
                    FieldInfo fieldInfo = errorField.FieldInfo;
                    foreach (Attribute attribute in Attribute.GetCustomAttributes(fieldInfo))
                    {
                        if (attribute.GetType() == typeof(NotNullAttribute))
                        {
                            if (((NotNullAttribute)attribute).IgnorePrefab)
                            {
                                violationsToIgnore.Add(errorField);
                            }
                        }
                    }
                }

                foreach (NotNullViolation violation in violationsToIgnore)
                {
                    erroringFields.Remove(violation);
                }
            }

            return erroringFields;
        }

        private static bool MonoBehaviourHasErrors(MonoBehaviour mb)
        {
            return FindErroringFields(mb).Count > 0;
        }
    }
}