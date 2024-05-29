using BLL.DTO;
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

        public static explicit operator ItemViewModel(ItemDTO itemDto)
        {
            return new ItemViewModel
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Company = itemDto.Company,
                Cost = itemDto.Cost,
                CategoryId = itemDto.CategoryId,
                Category = itemDto.Category != null ? (CategoryViewModel)itemDto.Category : null
            };
        }

        public static explicit operator ItemDTO(ItemViewModel itemViewModel)
        {
            return new ItemDTO
            {
                Id = itemViewModel.Id,
                Name = itemViewModel.Name,
                Company = itemViewModel.Company,
                Cost = itemViewModel.Cost,
                CategoryId = itemViewModel.CategoryId,
                //Category = itemViewModel.Category != null ? (CategoryDTO)itemViewModel.Category : null
            };
        }
    }
}
