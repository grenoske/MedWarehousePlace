using BLL.DTO;

namespace PL.Models.Location
{
    public class Bin
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ShelfId { get; set; }

        public static explicit operator Bin(BinDTO binDto)
        {
            if (binDto == null)
                return null;

            return new Bin
            {
                Id = binDto.Id,
                Number = binDto.Number,
                ShelfId = binDto.ShelfId
            };
        }
    }
}
