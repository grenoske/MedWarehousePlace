using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IInventoryService
    {
        // Item methods
        IEnumerable<ItemDTO> GetItems(int page);
        ItemDTO GetItemById(int id);
        void CreateItem(ItemDTO item);
        void UpdateItem(ItemDTO item);
        void DeleteItem(int id);

        // Category methods
        IEnumerable<CategoryDTO> GetCategories(int page = 1);
        CategoryDTO GetCategoryById(int id);
        void CreateCategory(CategoryDTO category);
        void UpdateCategory(CategoryDTO category);
        void DeleteCategory(int id);
    }
}
