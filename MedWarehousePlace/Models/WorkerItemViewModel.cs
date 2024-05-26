using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Models
{
    public class WorkerItemViewModel
    {
        public int Id { get; set; }
        public InventoryItemViewModel InventoryItem { get; set; }
        public string LocationTo { get; set; }
        public string ShelfId { get; set; } // це поле прив'язане до Shelf
        public string BinId { get; set; } // це поле прив'язане до Bin
        public IEnumerable<SelectListItem> Racks { get; set; }
        public IEnumerable<SelectListItem> Shelves { get; set; }
        public IEnumerable<SelectListItem> Bins { get; set; }
    }
}
