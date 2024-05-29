using BLL.DTO;

namespace PL.Models.Location
{
    public class Aisle
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public List<Rack> Racks { get; set; }
        public List<CellViewModel> Cells { get; set; }

        public static explicit operator Aisle(AisleDTO aisleDto)
        {
            if (aisleDto == null)
                return null;

            return new Aisle
            {
                Id = aisleDto.Id,
                Number = aisleDto.Number,
                WarehouseId = aisleDto.WarehouseId,
                Racks = aisleDto.Racks?.Select(rackDto => (Rack)rackDto).ToList(),
                Cells = aisleDto.Cells?.Select(cellDto => (CellViewModel)cellDto).ToList()
            };
        }
    }
}
