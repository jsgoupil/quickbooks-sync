using System;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItem<U>
        where U : struct, IConvertible
    {
        private object instance;
        private PropertyInfo itemProperty;
        private PropertyInfo itemElementNameProperty;
        private object itemValue;
        private U itemElementNameValue;
        private ObjectItemValue property;

        public ObjectItem(object instance, string name)
        {
            this.instance = instance;
            property = new ObjectItemValue();
            itemProperty = instance.GetType().GetProperty(name);
            itemElementNameProperty = instance.GetType().GetProperty(name + "ElementName");
            itemValue = itemProperty.GetValue(instance, null) as object;
            itemElementNameValue = (U)itemElementNameProperty.GetValue(instance, null);

            Initialize();
        }

        private void Initialize()
        {
            property.Name = itemElementNameValue.ToString();
            property.Value = itemValue;
        }

        public void SetItem<T>(U name, T value)
        {
            property = new ObjectItemValue
            {
                Name = name.ToString(),
                Value = value
            };

            SetItemOnInstance();
        }

        public T GetItem<T>(U name)
        {
            if (property.Name == name.ToString())
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
                itemElementNameProperty.SetValue(instance, (U)Enum.Parse(typeof(U), property.Name), null);
            }
        }
    }
}