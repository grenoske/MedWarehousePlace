namespace PL.Models.Location
{
    public class Aisle
    {
        int Id { get; set; }
        public int Number { get; set; }
        public List<Rack> Racks { get; set; }
    }
}
