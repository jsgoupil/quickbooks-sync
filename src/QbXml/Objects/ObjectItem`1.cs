using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItem<U>
        where U : struct, IConvertible
    {
        private readonly object instance;
        private readonly PropertyInfo itemProperty;
        private readonly PropertyInfo itemElementNameProperty;
        private readonly object? itemValue;
        private readonly U? itemElementNameValue;
        private ObjectItemValue property;

        public ObjectItem(object instance, string name)
        {
            this.instance = instance;
            property = new ObjectItemValue();
            itemProperty = instance.GetType().GetProperty(name) ?? throw new ArgumentException("The name must match to a property.", nameof(name));
            itemElementNameProperty = instance.GetType().GetProperty(name + "ElementName") ?? throw new ArgumentException("The name must match to a property ending with \"ElementName\".", nameof(name));
            itemValue = itemProperty.GetValue(instance, null) as object;
            itemElementNameValue = (U?)itemElementNameProperty.GetValue(instance, null);

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

        [return: MaybeNull]
        public T GetItem<T>(U name)
        {
            if (property.Name == name.ToString() && property.Value != null)
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
                itemElementNameProperty.SetValue(instance, (U)Enum.Parse(typeof(U), property.Name), null);
            }
        }
    }
}