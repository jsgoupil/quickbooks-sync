using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDelRequest : DataExtRequest<DataExtDelRqType>
    {
        public string TxnID { get; set; }

        protected override void ProcessObj(DataExtDelRqType obj)
        {
            base.ProcessObj(obj);

            obj.DataExtDel = new DataExtDel
            {
                DataExtName = DataExtName,
                OwnerID = OwnerID.ToString(),
            };

            var items = new ItemWithName<ItemsChoiceType27>();
            items.AddNotNull(ItemsChoiceType27.ListDataExtType, ListDataExtType);
            items.AddNotNull(ItemsChoiceType27.ListObjRef, ListObjRef);
            items.AddNotNull(ItemsChoiceType27.OtherDataExtType, OtherDataExtType);
            items.AddNotNull(ItemsChoiceType27.TxnDataExtType, TxnDataExtType);
            items.AddNotNull(ItemsChoiceType27.TxnID, TxnID);
            items.AddNotNull(ItemsChoiceType27.TxnLineID, TxnLineID);
            obj.DataExtDel.ItemsElementName = items.GetNames();
            obj.DataExtDel.Items = items.GetItems();
        }
    }
}
