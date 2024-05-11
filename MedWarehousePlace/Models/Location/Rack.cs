namespace PL.Models.Location
{
    public class Rack
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public int AisleId { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
