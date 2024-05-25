using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; } 
        public double Cost { get; set; }  
        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
