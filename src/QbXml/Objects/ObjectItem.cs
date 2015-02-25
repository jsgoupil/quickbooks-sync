using System.Collections.Generic;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItem
    {
        private object instance;
        private PropertyInfo itemProperty;
        private object itemValue;
        private ObjectItemValue property;
        private Dictionary<System.Type, string> typeMapping;

        public ObjectItem(object instance, string name, Dictionary<System.Type, string> typeMapping)
        {
            this.instance = instance;
            this.typeMapping = typeMapping;
            property = new ObjectItemValue();
            itemProperty = instance.GetType().GetProperty(name);
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

        public T GetItem<T>(string name)
        {
            if (property.Name == name)
            {
                return (T)property.Value;
            }

            return default(T);
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