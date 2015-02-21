using System;
using System.Reflection;

namespace QbSync.QbXml.Wrappers
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
            RemoveItem(name);

            itemValue = value;
            itemElementNameValue = name;

            SetItemsOnInstance();
        }

        public T GetItem<T>(U name)
        {
            if (itemElementNameValue.Equals(name))
            {
                return (T)itemValue;
            }

            return default(T);
        }

        private void RemoveItem(U name)
        {
            if (itemElementNameValue.Equals(name))
            {
                itemValue = null;
                itemElementNameValue = default(U);
                SetItemsOnInstance();
            }
        }

        private void SetItemsOnInstance()
        {
            itemProperty.SetValue(instance, itemValue, null);
            itemElementNameProperty.SetValue(instance, itemElementNameValue, null);
        }
    }
}
