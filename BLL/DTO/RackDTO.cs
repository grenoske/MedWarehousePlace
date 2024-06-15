namespace BLL.DTO
{
    public class RackDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public int AisleId { get; set; }
        public List<ShelfDTO> Shelves { get; set; }
        public List <CellDTO> Cells { get; set; }
    }
}
