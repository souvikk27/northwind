using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class EarningsViewModel
    {
        public decimal TotalEarnings { get; set; }
        public decimal TotalCommissions { get; set; }
        public decimal TotalBonuses { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal CurrentMonthEarnings { get; set; }
        public decimal PreviousMonthEarnings { get; set; }
        public decimal YearToDateEarnings { get; set; }
        public decimal ProjectedAnnualEarnings { get; set; }

        public List<MonthlyEarningVm>? MonthlyEarnings { get; set; }
        public List<CategoryEarningVm>? CategoryEarnings { get; set; }
        public List<CustomerEarningVm>? CustomerEarnings { get; set; }

        public decimal MonthOverMonthChange => CurrentMonthEarnings - PreviousMonthEarnings;
        public decimal MonthOverMonthPercentChange =>
            PreviousMonthEarnings != 0
                ? (CurrentMonthEarnings - PreviousMonthEarnings) / PreviousMonthEarnings * 100
                : 0;
    }
}
