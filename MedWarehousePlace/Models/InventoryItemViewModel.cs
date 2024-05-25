using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Models
{
    public class InventoryItemViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemViewModel Item { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Container { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public IEnumerable<SelectListItem> ItemList { get; set; }
    }
}
