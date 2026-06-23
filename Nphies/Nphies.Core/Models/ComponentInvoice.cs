using System;

namespace Nphies.Core.Models
{
    public class ComponentInvoice
    {
        public string ParentServiceCode { get; set; }
        public string ParentInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string ServiceCategory { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Quantity { get; set; }
        public DateTime ServiceStartDateTime { get; set; }
        public DateTime ServiceEndDateTime { get; set; }

    }
}
