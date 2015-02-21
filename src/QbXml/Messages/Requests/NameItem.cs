using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    internal class ItemWithoutName
    {
        private List<object> items = new List<object>();

        public void AddNotNull(object item)
        {
            if (item != null)
            {
                Add(item);
            }
        }

        public void Add(object item)
        {
            if (item is ITypeWrapper)
            {
                items.Add(item.ToString());
            }
            else
            {
                items.Add(item);
            }
        }

        public object[] GetItems()
        {
            return items
                .ToArray();
        }
    }

    internal class ItemWithName<T>
    {
        private List<NameItem<T>> items = new List<NameItem<T>>();

        public void AddNotNull(T name, object item)
        {
            if (item != null)
            {
                Add(name, item);
            }
        }

        public void Add(T name, object item)
        {
            items.Add(new NameItem<T>
            {
                Name = name,
                Item = item
            });
        }

        public T[] GetNames()
        {
            return items
                .Select(m => m.Name)
                .ToArray();
        }

        public object[] GetItems()
        {
            return items
                .Select(m => m.Item)
                .ToArray();
        }
    }

    internal class NameItem<T>
    {
        private object _item;
        internal T Name { get; set; }
        internal object Item
        {
            get
            {
                return _item;
            }
            set
            {
                if (value is ITypeWrapper)
                {
                    _item = value.ToString();
                }
                else
                {
                    _item = value;
                }
            }
        }
    }
}
