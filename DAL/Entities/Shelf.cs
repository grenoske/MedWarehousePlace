namespace DAL.Entities
{
    public class Shelf
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RackId { get; set; }
        public Rack Rack { get; set; }
        public ICollection<Bin> Bins { get; set; }
    }
}
