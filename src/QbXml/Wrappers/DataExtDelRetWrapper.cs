using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Xml.Serialization;

namespace QbSync.QbXml.Wrappers
{
    public class DataExtDelRetWrapper : DataExtDelRet
    {
        private ObjectItems<ItemsChoiceType28> objectItems;

        [XmlIgnore]
        public new GuidType OwnerID
        {
            get
            {
                return new GuidType(base.OwnerID);
            }
            set
            {
                base.OwnerID = value.ToString();
            }
        }

        [XmlIgnore]
        public ListDataExtType? ListDataExt
        {
            get
            {
                return ObjectItem.GetItem<ListDataExtType>(ItemsChoiceType28.ListDataExtType);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.ListDataExtType, value);
            }
        }

        [XmlIgnore]
        public ListObjRef ListObjRef
        {
            get
            {
                return ObjectItem.GetItem<ListObjRef>(ItemsChoiceType28.ListObjRef);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.ListObjRef, value);
            }
        }
        
        [XmlIgnore]
        public OtherDataExtType? OtherDataExt
        {
            get
            {
                return ObjectItem.GetItem<OtherDataExtType>(ItemsChoiceType28.OtherDataExtType);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.OtherDataExtType, value);
            }
        }

        [XmlIgnore]
        public TxnDataExtType? TxnDataExtType
        {
            get
            {
                return ObjectItem.GetItem<TxnDataExtType>(ItemsChoiceType28.TxnDataExtType);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.TxnDataExtType, value);
            }
        }

        [XmlIgnore]
        public string TxnLineID
        {
            get
            {
                return ObjectItem.GetItem<string>(ItemsChoiceType28.TxnLineID);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.TxnLineID, value);
            }
        }

        [XmlIgnore]
        public string TxnID
        {
            get
            {
                return ObjectItem.GetItem<string>(ItemsChoiceType28.TxnID);
            }
            set
            {
                ObjectItem.SetItem(ItemsChoiceType28.TxnID, value);
            }
        }

        private ObjectItems<ItemsChoiceType28> ObjectItem
        {
            get
            {
                return objectItems ?? (objectItems = new ObjectItems<ItemsChoiceType28>(this));
            }
        }
    }
}
