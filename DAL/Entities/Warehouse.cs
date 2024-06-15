namespace DAL.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int MaximumWeightOnUpperShelves { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Aisle> Aisles { get; set; } 
        public ICollection<Rack> Racks { get; set; }  
        public ICollection<Item> Items { get; set; }  
    }
}
