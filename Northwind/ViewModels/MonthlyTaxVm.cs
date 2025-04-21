using System;

namespace Northwind.ViewModels
{
    public class MonthlyTaxVm
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Income { get; set; }
        public decimal FederalTax { get; set; }
        public decimal StateTax { get; set; }
        public decimal SocialSecurity { get; set; }
        public decimal Medicare { get; set; }
        public decimal Retirement401k { get; set; }
        public decimal HealthInsurance { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetIncome { get; set; }
        
        public string MonthName => new System.Globalization.DateTimeFormatInfo().GetMonthName(Month);
        public string MonthYear => $"{MonthName} {Year}";
        public decimal EffectiveTaxRate => Income > 0 ? (TotalTax / Income) * 100 : 0;
    }
}
