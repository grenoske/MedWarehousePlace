using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ItemDTO> GetItems(int page = 1)
        {
            var items = _unitOfWork.Items.GetAll(page: page, includeProperties: "Category");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public ItemDTO GetItemById(int id)
        {
            var item = _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id, includeProperties: "Category");
            return _mapper.Map<ItemDTO>(item);
        }

        public void CreateItem(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            _unitOfWork.Items.Add(item);
            _unitOfWork.Save();
        }

        public void UpdateItem(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            _unitOfWork.Items.Update(item);
            _unitOfWork.Save();
        }

        public void DeleteItem(int id)
        {
            var item = _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _unitOfWork.Items.Remove(item);
                _unitOfWork.Save();
            }
        }

        // Category methods
        public IEnumerable<CategoryDTO> GetCategories(int page = 1)
        {
            var categories = _unitOfWork.Categories.GetAll(page: page);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public CategoryDTO GetCategoryById(int id)
        {
            var category = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public void CreateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Categories.Add(category);
            _unitOfWork.Save();
        }

        public void UpdateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Categories.Update(category);
            _unitOfWork.Save();
        }

        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Categories.Remove(category);
                _unitOfWork.Save();
            }
        }
    }
}
