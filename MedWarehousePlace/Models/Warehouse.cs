using PL.Models.Location;

namespace PL.Models
{
    public class Warehouse
    {
        public List<Cell> Cells { get; set; }
        public List<Aisle> Aisles { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
    }
}
