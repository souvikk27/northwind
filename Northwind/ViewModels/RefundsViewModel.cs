using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class RefundsViewModel
    {
        public int TotalRefunds { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public int ApprovedRefunds { get; set; }
        public int PendingRefunds { get; set; }
        public int RejectedRefunds { get; set; }
        public double AverageProcessingTime { get; set; }
        public string RefundPolicy { get; set; }
        
        public List<RefundRequestVm> RefundRequests { get; set; }
        public List<RefundByReasonVm> RefundsByReason { get; set; }
        
        public decimal ApprovalRate => TotalRefunds > 0 ? (decimal)ApprovedRefunds / TotalRefunds * 100 : 0;
        public decimal RejectionRate => TotalRefunds > 0 ? (decimal)RejectedRefunds / TotalRefunds * 100 : 0;
    }
}
