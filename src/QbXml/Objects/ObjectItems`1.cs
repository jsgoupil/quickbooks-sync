using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItems<U>
        where U : struct, IConvertible
    {
        private object instance;
        private PropertyInfo itemsProperty;
        private PropertyInfo itemsElementNameProperty;
        private object[] itemsValue;
        private U[] itemsElementNameValue;

        public ObjectItems(object instance, string name)
        {
            this.instance = instance;
            itemsProperty = instance.GetType().GetProperty(name);
            itemsElementNameProperty = instance.GetType().GetProperty(name + "ElementName");
            itemsValue = itemsProperty.GetValue(instance, null) as object[];
            itemsElementNameValue = itemsElementNameProperty.GetValue(instance, null) as U[];
        }

        public void SetItem<T>(U name, T value)
        {
            RemoveItems(name);
            EnsureArray();

            var newSize = itemsValue.Length + 1;
            Array.Resize(ref itemsValue, newSize);
            Array.Resize(ref itemsElementNameValue, newSize);
            itemsValue[newSize - 1] = value;
            itemsElementNameValue[newSize - 1] = name;

            SetItemsOnInstance();
        }

        public void SetItems<T>(U name, T[] values)
        {
            RemoveItems(name);
            EnsureArray();

            var newSize = itemsValue.Length + values.Length;
            Array.Resize(ref itemsValue, newSize);
            Array.Resize(ref itemsElementNameValue, newSize);

            var j = 0;
            var i = newSize - values.Length;
            for (; j < values.Length; i++, j++)
            {
                itemsValue[i] = values[j];
                itemsElementNameValue[i] = name;
            }

            SetItemsOnInstance();
        }

        public T GetItem<T>(U name)
        {
            return GetItems<T>(name).FirstOrDefault();
        }

        public IEnumerable<T> GetItems<T>(U name)
        {
            EnsureArray();
            for (var i = 0; i < itemsElementNameValue.Length; i++)
            {
                if (itemsElementNameValue[i].Equals(name))
                {
                    yield return (T)itemsValue[i];
                }
            }
        }

        private void RemoveItems(U name)
        {
            EnsureArray();
            var newItemsValue = new List<object>();
            var newItemsElementNameValue = new List<U>();
            for (var i = 0; i < itemsElementNameValue.Length; i++)
            {
                if (!itemsElementNameValue[i].Equals(name))
                {
                    newItemsValue.Add(itemsValue[i]);
                    newItemsElementNameValue.Add(itemsElementNameValue[i]);
                }
            }

            itemsValue = newItemsValue.ToArray();
            itemsElementNameValue = newItemsElementNameValue.ToArray();

            SetItemsOnInstance();
        }

        private void SetItemsOnInstance()
        {
            if (itemsValue.Length == 0)
            {
                itemsValue = null;
            }

            itemsProperty.SetValue(instance, itemsValue, null);

            if (itemsElementNameValue.Length == 0)
            {
                itemsElementNameValue = null;
            }

            itemsElementNameProperty.SetValue(instance, itemsElementNameValue, null);
        }

        private void EnsureArray()
        {
            if (itemsValue == null)
            {
                itemsValue = new object[0];
            }

            if (itemsElementNameValue == null)
            {
                itemsElementNameValue = new U[0];
            }
        }
    }
}