using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Models
{
    public class WorkerItemViewModel
    {
        public int Id { get; set; }
        public InventoryItemViewModel InventoryItem { get; set; }
        public string LocationTo { get; set; }
        public string ShelfId { get; set; } 
        public string BinId { get; set; } 
        public double TotalWeight { get; set; }

        public bool useRecommendation { get; set; }
        public string? RecommendLocation { get; set; }  
        public int? RecommendBinId { get; set; }
        public IEnumerable<SelectListItem> Racks { get; set; }
        public IEnumerable<SelectListItem> Shelves { get; set; }
        public IEnumerable<SelectListItem> Bins { get; set; }
    }
}
