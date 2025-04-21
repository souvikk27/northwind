namespace Northwind.ViewModels
{
    public class SettingsViewModel
    {
        // Company Information
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyAddress { get; set; }
        
        // Regional Settings
        public string Currency { get; set; }
        public string DateFormat { get; set; }
        public string TimeZone { get; set; }
        public string Language { get; set; }
        
        // Appearance
        public string Theme { get; set; }
        
        // Notifications
        public bool NotificationsEnabled { get; set; }
        public bool EmailNotifications { get; set; }
        public bool PushNotifications { get; set; }
        
        // Security
        public bool TwoFactorAuth { get; set; }
        
        // Backup & Recovery
        public bool AutomaticBackups { get; set; }
        public string BackupFrequency { get; set; }
    }
}
