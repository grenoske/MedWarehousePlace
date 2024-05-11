using PL.Models.Location;

namespace PL.Models
{
    public class Warehouse
    {
        public List<Cell> Cells { get; set; }
        public List<Aisle> Aisles { get; set; }
        public List<Rack> Racks { get; set; }
        public List<Item> Items { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
    }
}
