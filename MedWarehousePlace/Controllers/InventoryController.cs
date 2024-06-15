using BLL.DTO;
using BLL.Infrastructure.SD;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Models;

namespace PL.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IWarehouseService _warehouseService;

        public InventoryController(IInventoryService inventoryService, IWarehouseService warehouseService)
        {
            _inventoryService = inventoryService;
            _warehouseService = warehouseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = _inventoryService.GetInventoryItems(SD.StatusInventory).Select(c => (InventoryItemViewModel)c).ToList(),
                Title = SD.StatusInventory
            };
            return View("InventoryView", model);
        }

        public IActionResult Receiving()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = _inventoryService.GetInventoryItems(SD.StatusReceived).Select(c => (InventoryItemViewModel)c).ToList(),
                Title = SD.StatusReceived
            };
            return View("InventoryView", model);
        }

        public IActionResult Packing()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = _inventoryService.GetInventoryItems(SD.StatusPacking).Select(c => (InventoryItemViewModel)c).ToList(),
                Title = SD.StatusPacking
            };
            return View("InventoryView", model);
        }

        public IActionResult Transferring()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = _inventoryService.GetInventoryItems(SD.StatusTransferring).Select(c => (InventoryItemViewModel)c).ToList(),
                Title = SD.StatusTransferring
            };
            return View("InventoryView", model);
        }

        public IActionResult AddItem(string status)
        {
            var items = _inventoryService.GetItems().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });

            var model = new InventoryItemViewModel
            {
                Status = status,
                Location = status + "Area",
                ItemList = items
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(InventoryItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var inventoryItemDto = (InventoryItemDTO)model;
                _inventoryService.CreateInventoryItem(inventoryItemDto);
                return RedirectToAction(model.Status);
            }

            model.ItemList = _inventoryService.GetItems().Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            });

            return View(model);
        }

        public IActionResult PickUpItem(int itemId)
        {
            var item = _inventoryService.GetInventoryItemById(itemId);
            var rec = _inventoryService.RecommendItemPlacement(itemId, 1);
            if (item != null)
            {
                var model = new WorkerItemViewModel
                {
                    InventoryItem = (InventoryItemViewModel)item,
                    Racks = GetRackSelectList(),
                    RecommendBinId = rec.BinId,
                    RecommendLocation = rec.LocationDest,
                    TotalWeight = item.Item.Weight * item.Quantity,
                    Shelves = new List<SelectListItem>(), // Initially empty
                    Bins = new List<SelectListItem>() // Initially empty
                };
                return View(model);
            }
            return RedirectToAction("Inventory");
        }

        [HttpPost]
        public IActionResult ConfirmPickUp(WorkerItemViewModel model)
        {

            var itemDTO = (InventoryItemDTO)model.InventoryItem;
            
            if (model.useRecommendation)
            {
                itemDTO.BinId = model.RecommendBinId;
            }
            else
            {
                itemDTO.BinId = Int32.Parse(model.BinId);
            }
            _inventoryService.UpdateInventoryItem(itemDTO);
            model.Racks = GetRackSelectList();
            model.Shelves = new List<SelectListItem>(); // Reload shelves list
            model.Bins = new List<SelectListItem>();
            return RedirectToAction(model.InventoryItem.Status);
        }

        [HttpPost]
        public IActionResult PlaceItem(int itemId)
        {
            if (itemId == null || itemId == 0)
            {
                TempData["ErrorMessage"] = "Error while deleting";
                return BadRequest("inventoryItem is null");
            }

            _inventoryService.PlaceInventoryItemToWarehouse(itemId);
            TempData["SuccessMessage"] = "Placed Successful";
            return RedirectToAction("Inventory");
        }

        // Метод для отримання списку стелажів
        private IEnumerable<SelectListItem> GetRackSelectList()
        {
            var racks = _warehouseService.GetRacks();
            return racks.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"Rack {r.Number}"
            });
        }

        // Метод для отримання списку полиць на основі стелажа
        public JsonResult GetShelves(int rackId)
        {
            var shelves = _warehouseService.GetShelves(rackId);
            var selectList = shelves.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"Shelf {s.Number}"
            });
            return Json(selectList);
        }

        // Метод для отримання списку кошиків на основі полиці
        public JsonResult GetBins(int shelfId)
        {
            var bins = _warehouseService.GetBins(shelfId);
            var selectList = bins.Select(b => new SelectListItem
            {
                    Value = b.Id.ToString(),
                    Text = $"Bin {b.Number}"
            });
            return Json(selectList);
        }


        protected override void Dispose(bool disposing)
        {
            _inventoryService.Dispose();
            _warehouseService.Dispose();
            base.Dispose(disposing);
        }
    }
}
