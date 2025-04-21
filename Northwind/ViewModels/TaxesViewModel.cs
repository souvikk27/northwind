using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class TaxesViewModel
    {
        public int CurrentYear { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalFederalTax { get; set; }
        public decimal TotalStateTax { get; set; }
        public decimal TotalSocialSecurity { get; set; }
        public decimal TotalMedicare { get; set; }
        public decimal TotalRetirement401k { get; set; }
        public decimal TotalHealthInsurance { get; set; }
        public decimal TotalOtherDeductions { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public string FederalFilingStatus { get; set; }
        public int Allowances { get; set; }
        public decimal EstimatedAnnualIncome { get; set; }
        public decimal EstimatedAnnualTax { get; set; }
        public decimal YearToDateWithholding { get; set; }
        
        public List<MonthlyTaxVm> MonthlyTaxes { get; set; }
        public List<TaxDocumentVm> TaxDocuments { get; set; }
        
        public decimal TotalTax => TotalFederalTax + TotalStateTax + TotalSocialSecurity + TotalMedicare;
        public decimal TotalDeductions => TotalRetirement401k + TotalHealthInsurance + TotalOtherDeductions;
        public decimal NetIncome => TotalIncome - TotalTax - TotalDeductions;
    }
}
