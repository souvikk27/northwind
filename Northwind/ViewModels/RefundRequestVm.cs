using System;

namespace Northwind.ViewModels
{
    public class RefundRequestVm
    {
        public int RefundId { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        
        public int ProcessingDays => ProcessedDate.HasValue ? (ProcessedDate.Value - CreatedDate).Days : 0;
    }
}
