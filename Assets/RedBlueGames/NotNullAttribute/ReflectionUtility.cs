namespace RedBlueGames.NotNull
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Utility methods to help with Reflection
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// Gets all fields in a class that have a specified attribute. Default returns only public fields. Use binding flags
        /// to get NonPublic fields.
        /// </summary>
        /// <returns>A List of FieldInfo for all fields with the specified attribute.</returns>
        /// <param name="classToInspect">Class to inspect.</param>
        /// <param name="reflectionFlags">Reflection flags - supplying none uses default GetFields method.</param>
        /// <typeparam name="T">The Attribute type to search for.</typeparam>
        public static List<FieldInfo> GetFieldsWithAttributeFromType<T>(
            Type classToInspect,
            BindingFlags reflectionFlags = BindingFlags.Default)
        {
            List<FieldInfo> fieldsWithAttribute = new List<FieldInfo>();
            FieldInfo[] allFields;
            if (reflectionFlags == BindingFlags.Default)
            {
                allFields = classToInspect.GetFields();
            }
            else
            {
                allFields = classToInspect.GetFields(reflectionFlags);
            }

            foreach (FieldInfo fieldInfo in allFields)
            {
                foreach (var attribute in Attribute.GetCustomAttributes(fieldInfo))
                {
                    if (attribute.GetType() == typeof(T))
                    {
                        fieldsWithAttribute.Add(fieldInfo);
                        break;
                    }
                }
            }

            return fieldsWithAttribute;
        }
    }
}