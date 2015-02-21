using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Wrappers
{
    public class ErrorRecoveryWrapper : QbXmlWrapper
    {
        private ErrorRecovery qbXmlObject;
        private ObjectItem<ItemChoiceType1> objectItem;

        public ErrorRecoveryWrapper(ErrorRecovery qbXmlObject)
        {
            this.qbXmlObject = qbXmlObject;
        }

        public string TxnNumber
        {
            get
            {
                return qbXmlObject.TxnNumber;
            }
            set
            {
                qbXmlObject.TxnNumber = value;
            }
        }

        public string EditSequence
        {
            get
            {
                return qbXmlObject.EditSequence;
            }
            set
            {
                qbXmlObject.EditSequence = value;
            }
        }

        public GuidType ExternalGUID
        {
            get
            {
                return new GuidType(qbXmlObject.ExternalGUID);
            }
            set
            {
                qbXmlObject.ExternalGUID = value.ToString();
            }
        }

        public string ListID
        {
            get
            {
                return ObjectItem.GetItem<string>(ItemChoiceType1.ListID);
            }
            set
            {
                ObjectItem.SetItem(ItemChoiceType1.ListID, value);
            }
        }

        public string OwnerID
        {
            get
            {
                return ObjectItem.GetItem<string>(ItemChoiceType1.OwnerID);
            }
            set
            {
                ObjectItem.SetItem(ItemChoiceType1.OwnerID, value);
            }
        }

        public string TxnID
        {
            get
            {
                return ObjectItem.GetItem<string>(ItemChoiceType1.TxnID);
            }
            set
            {
                ObjectItem.SetItem(ItemChoiceType1.TxnID, value);
            }
        }

        private ObjectItem<ItemChoiceType1> ObjectItem
        {
            get
            {
                return objectItem ?? (objectItem = new ObjectItem<ItemChoiceType1>(qbXmlObject));
            }
        }
    }
}
