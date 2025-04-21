using System;

namespace Northwind.ViewModels
{
    public class TaxDocumentVm
    {
        public string DocumentName { get; set; }
        public int Year { get; set; }
        public DateTime DateIssued { get; set; }
        public string Status { get; set; }
        public string DownloadUrl { get; set; }
    }
}
