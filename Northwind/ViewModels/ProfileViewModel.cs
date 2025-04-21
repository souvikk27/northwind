using System;

namespace Northwind.ViewModels
{
    public class ProfileViewModel
    {
        // Personal Information
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string ReportsTo { get; set; }
        public string Notes { get; set; }
        public string PhotoUrl { get; set; }
        
        // Performance Metrics
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string TopSellingCategory { get; set; }
        
        // Account Information
        public DateTime LastLoginDate { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        
        // Preferences
        public bool HasTwoFactorEnabled { get; set; }
        public string PreferredLanguage { get; set; }
        public string TimeZone { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool MarketingEmailsEnabled { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";
        public int YearsWithCompany => DateTime.Now.Year - HireDate.Year - (DateTime.Now.DayOfYear < HireDate.DayOfYear ? 1 : 0);
        public int Age => DateTime.Now.Year - BirthDate.Year - (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
    }
}
