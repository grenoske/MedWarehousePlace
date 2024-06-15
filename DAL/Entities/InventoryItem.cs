namespace DAL.Entities
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Container { get; set; }
        public string Location { get; set; }
        public string? LocationDest { get; set; }
        public string Status { get; set; }


        public int? BinId {  get; set; }
        public Bin Bin { get; set; }
    }
}
