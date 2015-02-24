using System.Reflection;

namespace QbSync.QbXml.Objects
{
    internal class ObjectItem
    {
        private object instance;
        private PropertyInfo itemProperty;
        private object itemValue;

        public ObjectItem(object instance, string name)
        {
            this.instance = instance;
            itemProperty = instance.GetType().GetProperty(name);
            itemValue = itemProperty.GetValue(instance, null) as object;
        }

        public void SetItem<T>(T value)
        {
            RemoveItem<T>();

            itemValue = value;

            SetItemOnInstance();
        }

        public T GetItem<T>()
        {
            return (T)itemValue;
        }

        private void RemoveItem<T>()
        {
            itemValue = default(T);
            SetItemOnInstance();
        }

        private void SetItemOnInstance()
        {
            itemProperty.SetValue(instance, itemValue, null);
        }
    }
}