using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtAddRequest : DataExtRequest<DataExtAddRqType>
    {
        public string DataExtValue { get; set; }
        public DataExtAddTxnID TxnID { get; set; }

        protected override void ProcessObj(DataExtAddRqType obj)
        {
            base.ProcessObj(obj);

            obj.DataExtAdd = new DataExtAdd
            {
                DataExtName = DataExtName,
                DataExtValue = DataExtValue,
                OwnerID = OwnerID.ToString(),
            };

            var items = new ItemWithoutName();
            items.AddNotNull(ListDataExtType);
            items.AddNotNull(ListObjRef);
            items.AddNotNull(OtherDataExtType);
            items.AddNotNull(TxnDataExtType);
            items.AddNotNull(TxnID);
            items.AddNotNull(TxnLineID);
            obj.DataExtAdd.Items = items.GetItems();
        }
    }
}