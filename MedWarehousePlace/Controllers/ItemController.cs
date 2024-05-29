using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Models;

namespace PL.Controllers
{
    public class ItemController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public ItemController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public IActionResult Index(int page = 1)
        {
            var itemsDTO = _inventoryService.GetItems(page);
            var itemsView = itemsDTO.Select(wh => (ItemViewModel)wh);
            return View(itemsView);
        }

        public IActionResult Upsert(int? itemId)
        {
            var item = new ItemViewModel
            {
                CategoryList = _inventoryService.GetCategories().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };

            if (itemId == null || itemId == 0)
            {
                // create
                return View(item);
            }
            else
            {
                // update
                var itemDto = _inventoryService.GetItemById(itemId.Value);
                var itemView = (ItemViewModel)itemDto;
                itemView.CategoryList = item.CategoryList;
                return View(itemView);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ItemViewModel item)
        {
            var itemDto = (ItemDTO)item;
            if (item.Id == 0)
            {
                _inventoryService.CreateItem(itemDto);
            }
            else
            {
                _inventoryService.UpdateItem(itemDto);
            }

            TempData["success"] = "Product created/updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int? itemId)
        {
            if (itemId == null || itemId == 0)
            {
                TempData["ErrorMessage"] = "Error while deleting";
                return BadRequest("product is null");
            }

            _inventoryService.DeleteItem(itemId.Value);
            TempData["SuccessMessage"] = "Delete Successful";
            return RedirectToAction("Index");
        }



        // Fake BLL
        public IEnumerable<ItemViewModel> GetItems(int page)
        {
            List<ItemViewModel> items = new List<ItemViewModel>();
            if (page == 1)
            {
                items = new List<ItemViewModel>()
                {
                    new ItemViewModel { Id = 1, Name = "Latex Gloves", CategoryId = 1, Category = new CategoryViewModel { Id = 1, Name = "Medical Supplies" }, Company = "MedicalCo", Cost = 10 },
                    new ItemViewModel { Id = 2, Name = "Surgical Mask", CategoryId = 2,Category = new CategoryViewModel { Id = 2, Name = "Personal Protective Equipment" }, Company = "SafetyGear Ltd", Cost = 5 },
                    new ItemViewModel { Id = 3, Name = "Digital Thermometer", CategoryId = 3, Category = new CategoryViewModel { Id = 3, Name = "Diagnostic Equipment" }, Company = "HealthTech", Cost = 30 }
                };
            }
            else if (page == 2)
            {
                items = new List<ItemViewModel>()
                {
                    new ItemViewModel { Id = 4, Name = "Stethoscope", CategoryId = 4,Category = new CategoryViewModel { Id = 3, Name = "Diagnostic Equipment" }, Company = "MedInnovate", Cost = 50 },
                    new ItemViewModel { Id = 5, Name = "Wheelchair", CategoryId = 5,Category = new CategoryViewModel { Id = 4, Name = "Mobility Aids" }, Company = "Mobility Solutions Inc", Cost = 300 }
                };
            }

            return items;
        }

        private IEnumerable<CategoryViewModel> GetCategories()
        {


            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel { Id = 1, Name = "Medical Supplies" },
                new CategoryViewModel { Id = 2, Name = "Personal Protective Equipment" },
                new CategoryViewModel { Id = 3, Name = "Diagnostic Equipment" },
                new CategoryViewModel { Id = 4, Name = "Mobility Aids" }
            };

            return categories;
        }

        public ItemViewModel GetItem(int? id)
        {
            return new ItemViewModel { Id = 1, Name = "Latex Gloves", CategoryId = 1, Category = new CategoryViewModel { Id = 1, Name = "Medical Supplies" }, Company = "MedicalCo", Cost = 10 };
        }

        public void UpdateItem(ItemViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine("Updating item with id: " + obj.Id);
            return;
        }
        public void AddItem(ItemViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine(obj.CategoryId);
            System.Diagnostics.Debug.WriteLine("Adding item with name: " + obj.Name);
            return;
        }
        public void RemoveItem(ItemViewModel obj)
        {
            System.Diagnostics.Debug.WriteLine("Removing item with id: " + obj.Id);
            return;
        }
    }
}
