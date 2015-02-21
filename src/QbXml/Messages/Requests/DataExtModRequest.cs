using QbSync.QbXml.Objects;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtModRequest : DataExtRequest<DataExtModRqType>
    {
        public string DataExtValue { get; set; }
        public string TxnID { get; set; }

        protected override void ProcessObj(DataExtModRqType obj)
        {
            base.ProcessObj(obj);

            obj.DataExtMod = new DataExtMod
            {
                DataExtName = DataExtName,
                DataExtValue = DataExtValue,
                OwnerID = OwnerID.ToString(),
            };

            var items = new ItemWithName<ItemsChoiceType26>();
            items.AddNotNull(ItemsChoiceType26.ListDataExtType, ListDataExtType);
            items.AddNotNull(ItemsChoiceType26.ListObjRef, ListObjRef);
            items.AddNotNull(ItemsChoiceType26.OtherDataExtType, OtherDataExtType);
            items.AddNotNull(ItemsChoiceType26.TxnDataExtType, TxnDataExtType);
            items.AddNotNull(ItemsChoiceType26.TxnID, TxnID);
            items.AddNotNull(ItemsChoiceType26.TxnLineID, TxnLineID);
            obj.DataExtMod.ItemsElementName = items.GetNames();
            obj.DataExtMod.Items = items.GetItems();
        }
    }
}
