using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Models;

namespace PL.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public CategoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public IActionResult Index(int page = 1)
        {
            var categories = _inventoryService.GetCategories(page).Select(c => (CategoryViewModel)c);
            return View(categories);
        }

        public IActionResult Upsert(int? id)
        {
            CategoryViewModel category = new CategoryViewModel();
            if (id == null || id == 0)
            {
                // create
                return View(category);
            }
            else
            {
                // update
                var categoryDto = _inventoryService.GetCategoryById(id.Value);
                category = (CategoryViewModel)categoryDto;
                return View(category);
            }
        }

        [HttpPost]
        public IActionResult Upsert(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var categoryDto = (CategoryDTO)category;
                if (category.Id == 0)
                {
                    _inventoryService.CreateCategory(categoryDto);
                }
                else
                {
                    _inventoryService.UpdateCategory(categoryDto);
                }

                TempData["success"] = "Category created/updated successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var categoryDto = _inventoryService.GetCategoryById(id);
            if (categoryDto == null)
            {
                TempData["ErrorMessage"] = "Error while deleting";
                return BadRequest("category is null");
            }
            else
            {
                _inventoryService.DeleteCategory(id);
                TempData["SuccessMessage"] = "Delete Successful";
            }

            return RedirectToAction("Index");
        }



        // Fake BLL
        public IEnumerable<CategoryViewModel> GetCategories(int page)
        {
            List<CategoryViewModel> items = new List<CategoryViewModel>();
            if (page == 1)
            {
                items = new List<CategoryViewModel>()
                {
                    new CategoryViewModel { Id = 1, Name = "Medical Supplies" },
                    new CategoryViewModel { Id = 2, Name = "Personal Protective Equipment" },
                    new CategoryViewModel { Id = 3, Name = "Diagnostic Equipment"}
                };
            }
            else if (page == 2)
            {
                items = new List<CategoryViewModel>()
                {
                    new CategoryViewModel {Id = 3, Name = "Diagnostic Equipment"},
                    new CategoryViewModel { Id = 4, Name = "Mobility Aids"}
                };
            }

            return items;
        }

/*        private IEnumerable<CategoryViewModel> GetCategories()
        {


            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel { Id = 1, Name = "Medical Supplies" },
                new CategoryViewModel { Id = 2, Name = "Personal Protective Equipment" },
                new CategoryViewModel { Id = 3, Name = "Diagnostic Equipment" },
                new CategoryViewModel { Id = 4, Name = "Mobility Aids" }
            };

            return categories;
        }*/

        public CategoryViewModel GetCategory(int? id)
        {
            return new CategoryViewModel { Id = 1, Name = "Medical Supplies"};
        }

        public void UpdateCategory(CategoryViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine("Updating Category with id: " + obj.Id);
            return;
        }
        public void AddCategory(CategoryViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine("Adding Category with name: " + obj.Name);
            return;
        }
        public void RemoveCategory(CategoryViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine("Removing Category with id: " + obj.Id);
            return;
        }
    }
}
