namespace PL.Models.Location
{
    public class Rack
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Shelf> Shelves { get; set; }
    }
}
