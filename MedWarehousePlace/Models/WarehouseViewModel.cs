using BLL.DTO;
using PL.Models.Location;

namespace PL.Models
{
    public class WarehouseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }

        public static explicit operator WarehouseViewModel(WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
                return null;

            return new WarehouseViewModel
            {
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
