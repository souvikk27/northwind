using System;

namespace Northwind.ViewModels
{
    public class MonthlyEarningVm
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal Commissions { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Taxes { get; set; }
        public decimal NetEarnings { get; set; }
        
        public string MonthName => new System.Globalization.DateTimeFormatInfo().GetMonthName(Month);
        public string MonthYear => $"{MonthName} {Year}";
        public decimal GrossEarnings => BaseAmount + Commissions + Bonuses;
    }
}
