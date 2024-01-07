using System;
using System.Reflection;



    public abstract class AbstractValueModifier<T> : IModifier
    {

        public string attributeName;
        
        public T amount;

        public abstract void Apply(GunSO Gun);

        
        protected FieldType GetAttribute<FieldType>(
            GunSO Gun,
            out object TargetObject,
            out FieldInfo Field)
        {
            string[] paths = attributeName.Split("/");
            string attribute = paths[paths.Length - 1];

            Type type = Gun.GetType();
            object target = Gun;

            for (int i = 0; i < paths.Length - 1; i++)
            {
                FieldInfo field = type.GetField(paths[i]);
                if (field == null)
                {
                    UnityEngine.Debug.LogError($"Unable to apply modifier" +
                        $" to attribute {attributeName} because it does not exist on gun {Gun}");
                    throw new InvalidPathSpecifiedException(attributeName);
                }
                else
                {
                    target = field.GetValue(target);
                    type = target.GetType();
                }
            }

            FieldInfo attributeField = type.GetField(attribute);
            if (attributeField == null)
            {
                UnityEngine.Debug.LogError($"Unable to apply modifier to attribute " +
                    $"{attributeName} because it does not exist on gun {Gun}");
                throw new InvalidPathSpecifiedException(attributeName);
            }

            Field = attributeField;
            TargetObject = target;
            return (FieldType)attributeField.GetValue(target);
        }
    }
