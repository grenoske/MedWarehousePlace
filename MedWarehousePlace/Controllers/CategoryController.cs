using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Models;

namespace PL.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            return View(GetCategories(page));
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
                category = GetCategory(id);
                return View(category);
            }

        }
        [HttpPost]
        public IActionResult Upsert(CategoryViewModel category)
        {
            if (category.Id == 0)
            {
                AddCategory(category);
            }
            else
            {
                UpdateCategory(category);
            }

            TempData["success"] = "Category created/updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var categoryToBeDeleted = GetCategory(id);
            if (categoryToBeDeleted == null)
            {
                TempData["ErrorMessage"] = "Error while deleting";
                return BadRequest("category is null");
            }
            else
            {
                RemoveCategory(categoryToBeDeleted);
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
