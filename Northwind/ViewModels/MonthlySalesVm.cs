namespace Northwind.ViewModels
{
    public class MonthlySalesVm
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalSales { get; set; }
        
        public string MonthName => new System.Globalization.DateTimeFormatInfo().GetMonthName(Month);
        public string MonthYear => $"{MonthName} {Year}";
    }
}
