using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    [Authorize]
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

        protected override void Dispose(bool disposing)
        {
            _inventoryService.Dispose();
            base.Dispose(disposing);
        }

    }
}
