namespace PL.Models
{
    public class WorkerItemViewModel
    {
        public int Id { get; set; }
        public InventoryItemViewModel InventoryItem { get; set; }
        public string LocationTo { get; set; }
    }
}
