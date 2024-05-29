using BLL.DTO;

namespace PL.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static explicit operator CategoryViewModel(CategoryDTO catDto)
        {
            return new CategoryViewModel
            {
                Id = catDto.Id,
                Name = catDto.Name,
            };
        }

        public static explicit operator CategoryDTO(CategoryViewModel categoryViewModel)
        {
            return new CategoryDTO
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name
            };
        }

    }
}
