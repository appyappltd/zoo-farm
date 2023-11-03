using System.Reflection;

namespace Tools.Extension
{
    public static class ReflexExtension
    {
        public static TFieldType GetFieldValue<TFieldType, TObjectType>(this TObjectType obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);
            return (TFieldType)fieldInfo.GetValue(obj);
        }
        
        public static FieldInfo[] GetFields<TObjectType>(this TObjectType obj)
        {
            FieldInfo[] fieldInfo = obj.GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic);
            return fieldInfo;
        }
    }
}