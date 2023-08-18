using UnityEditor;
using UnityEngine;

namespace Tools.Extension
{
    public static class PropertyExtensions
    {
        static public SerializedProperty FindPropertyRelativeFix(this SerializedProperty sp, string name, ref SerializedObject objectToApplyChanges)
        {
            SerializedProperty result;
            if (typeof(ScriptableObject).IsAssignableFrom(sp.GetFieldType()) )
            {
                if (sp.objectReferenceValue == null) return null;
                if (objectToApplyChanges == null)
                    objectToApplyChanges = new SerializedObject(sp.objectReferenceValue);
                result = objectToApplyChanges.FindProperty(name);
            }
            else
            {
                objectToApplyChanges = null;
                result = sp.FindPropertyRelative(name);
            }
            return result;
        }


        static public System.Type GetFieldType(this SerializedProperty property)
        {
            if (property.serializedObject.targetObject == null) return null;
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetFieldViaPath(property.propertyPath);
            string path = property.propertyPath;
            if (fi == null)
                return null;
           
            return fi.FieldType;
        }
        
        public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type,string path)
        {
            System.Type parentType = type;
            System.Reflection.FieldInfo fi = type.GetField(path);
            string[] perDot = path.Split('.');
            foreach (string fieldName in perDot)
            {
                fi = parentType.GetField(fieldName);
                if (fi != null)
                    parentType = fi.FieldType;
                else
                    return null;
            }
            if (fi != null)
                return fi;
            
            return null;
        }
    }
}