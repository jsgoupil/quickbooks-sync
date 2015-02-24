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

        public ObjectItem(object instance)
        {
            this.instance = instance;
            itemProperty = instance.GetType().GetProperty("Item");
            itemElementNameProperty = instance.GetType().GetProperty("ItemElementName");
            itemValue = itemProperty.GetValue(instance, null) as object;
            itemElementNameValue = (U)itemElementNameProperty.GetValue(instance, null);
        }

        public void SetItem<T>(U name, T value)
        {
            RemoveItem<T>(name);

            itemValue = value;
            itemElementNameValue = name;

            SetItemOnInstance();
        }

        public T GetItem<T>(U name)
        {
            if (itemElementNameValue.Equals(name))
            {
                return (T)itemValue;
            }

            return default(T);
        }

        private void RemoveItem<T>(U name)
        {
            if (itemElementNameValue.Equals(name))
            {
                itemValue = default(T);
                itemElementNameValue = default(U);

                SetItemOnInstance();
            }
        }

        private void SetItemOnInstance()
        {
            itemProperty.SetValue(instance, itemValue, null);
            itemElementNameProperty.SetValue(instance, itemElementNameValue, null);
        }
    }
}