using PL.Models.Location;

namespace PL.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Container { get; set; }
        public string Location { get; set; }
        public string Product {  get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
