using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItem
    {
        private readonly object instance;
        private readonly PropertyInfo itemProperty;
        private readonly object? itemValue;
        private readonly Dictionary<System.Type, string> typeMapping;
        private ObjectItemValue property;

        public ObjectItem(object instance, string name, Dictionary<System.Type, string> typeMapping)
        {
            this.instance = instance;
            this.typeMapping = typeMapping;
            property = new ObjectItemValue();
            itemProperty = instance.GetType().GetProperty(name) ?? throw new ArgumentException("The name must match to a property.", nameof(name));
            itemValue = itemProperty.GetValue(instance, null) as object;

            Initialize();
        }

        private void Initialize()
        {
            if (itemValue != null)
            {
                var valueType = itemValue.GetType();
                foreach (var kvp in typeMapping)
                {
                    if (valueType.Equals(kvp.Key) || valueType.IsSubclassOf(kvp.Key))
                    {
                        property.Name = kvp.Value;
                        break;
                    }
                }

                property.Value = itemValue;
            }
        }

        public void SetItem<T>(string name, T value)
        {
            property = new ObjectItemValue
            {
                Name = name,
                Value = value
            };

            SetItemOnInstance();
        }

        [return: MaybeNull]
        public T GetItem<T>(string name)
        {
            if (property.Name == name && property.Value != null)
            {
                return (T)property.Value;
            }

            return default;
        }

        private void SetItemOnInstance()
        {
            if (!string.IsNullOrEmpty(property.Name))
            {
                itemProperty.SetValue(instance, property.Value, null);
            }
        }
    }
}