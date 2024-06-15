namespace DAL.Entities
{
    public class Cell
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public ICollection<Bin> Bins { get; set; }
    }
}
