using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItems
    {
        private object instance;
        private PropertyInfo itemsProperty;
        private object[] itemsValue;

        public ObjectItems(object instance, string name)
        {
            this.instance = instance;
            itemsProperty = instance.GetType().GetProperty(name);
            itemsValue = itemsProperty.GetValue(instance, null) as object[];
        }

        public void SetItem<T>(T value)
        {
            RemoveItems<T>();
            EnsureArray();

            var newSize = itemsValue.Length + 1;
            Array.Resize(ref itemsValue, newSize);
            itemsValue[newSize - 1] = value;

            SetItemsOnInstance();
        }

        public void SetItems<T>(T[] values)
        {
            RemoveItems<T>();
            EnsureArray();

            var newSize = itemsValue.Length + values.Length;
            Array.Resize(ref itemsValue, newSize);

            var j = 0;
            var i = newSize - values.Length;
            for (; j < values.Length; i++, j++)
            {
                itemsValue[i] = values[j];
            }

            SetItemsOnInstance();
        }

        public T GetItem<T>()
        {
            return GetItems<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetItems<T>()
        {
            EnsureArray();
            for (var i = 0; i < itemsValue.Length; i++)
            {
                if (itemsValue[i].GetType() == typeof(T))
                {
                    yield return (T)itemsValue[i];
                }
            }
        }

        private void RemoveItems<T>()
        {
            EnsureArray();
            var newItemsValue = new List<object>();
            for (var i = 0; i < itemsValue.Length; i++)
            {
                if (itemsValue[i].GetType() != typeof(T))
                {
                    newItemsValue.Add(itemsValue[i]);
                }
            }

            itemsValue = newItemsValue.ToArray();

            SetItemsOnInstance();
        }

        private void SetItemsOnInstance()
        {
            if (itemsValue.Length == 0)
            {
                itemsValue = null;
            }

            itemsProperty.SetValue(instance, itemsValue, null);
        }

        private void EnsureArray()
        {
            if (itemsValue == null)
            {
                itemsValue = new object[0];
            }
        }
    }
}
