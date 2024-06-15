namespace DAL.Entities
{
    public class Rack
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int? AisleId { get; set; }
        public Aisle Aisle { get; set; }
        public ICollection<Shelf> Shelves { get; set; }
        public ICollection<Cell> Cells { get; set; }
    }
}
