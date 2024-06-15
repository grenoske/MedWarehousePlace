namespace DAL.Entities
{
    public class Bin
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }

        public int? CellId { get; set; }
        public Cell Cell { get; set; }

        public int? InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
