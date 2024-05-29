using BLL.DTO;

namespace PL.Models.Location
{
    public class Rack
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public int AisleId { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<CellViewModel> Cells { get; set; }

        public static explicit operator Rack(RackDTO rackDto)
        {
            if (rackDto == null)
                return null;

            return new Rack
            {
                Id = rackDto.Id,
                Number = rackDto.Number,
                WarehouseId = rackDto.WarehouseId,
                AisleId = rackDto.AisleId,
                Shelves = rackDto.Shelves?.Select(shelfDto => (Shelf)shelfDto).ToList(),
                Cells = rackDto.Cells?.Select(cellDto => (CellViewModel)cellDto).ToList()
            };
        }
    }
}
