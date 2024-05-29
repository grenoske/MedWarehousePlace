using BLL.DTO;
using PL.Models.Location;

namespace PL.Models
{
    public class WarehouseViewModel2
    {
        public List<CellViewModel> Cells { get; set; }
        public List<Aisle> Aisles { get; set; }
        public List<Rack> Racks { get; set; }
        //public List<Item> Items { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }

        public static explicit operator WarehouseViewModel2(WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
                return null;

            return new WarehouseViewModel2
            {
                Cells = warehouseDto.Cells?.Select(cellDto => (CellViewModel)cellDto).ToList(),
                Aisles = warehouseDto.Aisles?.Select(aisleDto => (Aisle)aisleDto).ToList(),
                Racks = warehouseDto.Racks?.Select(rackDto => (Rack)rackDto).ToList(),
                Id = warehouseDto.Id,
                Name = warehouseDto.Name,
                Location = warehouseDto.Location,
                Width = warehouseDto.Width,
                Length = warehouseDto.Length,
                Height = warehouseDto.Height,
            };
        }
    }
}
