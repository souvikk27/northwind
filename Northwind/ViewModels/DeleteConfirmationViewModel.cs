namespace Northwind.ViewModels
{
#nullable disable
    public class DeleteConfirmationViewModel
    {
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public string DeleteAction { get; set; }
        public string DeleteController { get; set; }
        public string CancelAction { get; set; }
        public string CancelController { get; set; }
    }
}