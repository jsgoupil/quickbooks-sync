using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Struct
{
    public enum TxnType
    {
        ARRefundCreditCard,
        Bill,
        BillPaymentCheck,
        BillPaymentCreditCard,
        BuildAssembly,
        Charge,
        Check,
        CreditCardCharge,
        CreditCardCredit,
        CreditMemo,
        Deposit,
        Estimate,
        InventoryAdjustment,
        Invoice,
        ItemReceipt,
        JournalEntry,
        LiabilityAdjustment,
        Paycheck,
        PayrollLiabilityCheck,
        PurchaseOrder,
        ReceivePayment,
        SalesOrder,
        SalesReceipt,
        SalesTaxPaymentCheck,
        Transfer,
        VendorCredit,
        YTDAdjustment
    }
}
