namespace PL.Models.Location
{
    public class Shelf
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Bin> Bins { get; set; }
    }
}
