namespace PL.Models.Location
{
    public class Aisle
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public List<Rack> Racks { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
