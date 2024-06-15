namespace DAL.Entities
{
    public class Aisle
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<Rack> Racks { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
