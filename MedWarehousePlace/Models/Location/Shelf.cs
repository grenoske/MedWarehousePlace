using BLL.DTO;

namespace PL.Models.Location
{
    public class Shelf
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RackId { get; set; }
        public List<Bin> Bins { get; set; }

        public static explicit operator Shelf(ShelfDTO shelfDto)
        {
            if (shelfDto == null)
                return null;

            return new Shelf
            {
                Id = shelfDto.Id,
                Number = shelfDto.Number,
                RackId = shelfDto.RackId,
                Bins = shelfDto.Bins?.Select(binDto => (Bin)binDto).ToList()
            };
        }
    }
}
