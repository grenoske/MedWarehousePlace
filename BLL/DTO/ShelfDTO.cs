namespace BLL.DTO
{
    public class ShelfDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RackId { get; set; }
        public List<BinDTO> Bins { get; set; }
    }
}
