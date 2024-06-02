using BLL.DTO;
using BLL.Infrastructure.SD;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using PL.Models;
using PL.Models.Location;
using System.ComponentModel;

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

        // Mock data for demonstration purposes
        private List<ItemViewModel> _items = new List<ItemViewModel>
        {
            new ItemViewModel { Id = 1, Name = "Item 1", Company = "Company A", Cost = 10.0, CategoryId = 1 },
            new ItemViewModel { Id = 2, Name = "Item 2", Company = "Company B", Cost = 15.0, CategoryId = 2 }
        };

        private List<WorkerItemViewModel> _workerItems = new List<WorkerItemViewModel>()
        {
            new WorkerItemViewModel { Id = 1, InventoryItem = new InventoryItemViewModel { Id = 1, ItemId = 1, Item = new ItemViewModel { Id = 1, Name = "Item 1", Company = "Company A", Cost = 10.0, CategoryId = 1 }, Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6), Container = "Box", Location = "Receiving Area", Status = "Inventory" }, LocationTo = "Inventory 01-01-02-01"},
            new WorkerItemViewModel { Id = 1, InventoryItem = new InventoryItemViewModel { Id = 1, ItemId = 1, Item = new ItemViewModel { Id = 2, Name = "Item 2", Company = "Company B", Cost = 15.0, CategoryId = 2 }, Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6), Container = "Box", Location = "Shelf B", Status = "Inventory" }, LocationTo = "Packing Area"}
        };

        private List<InventoryItemViewModel> GetInventoryItems()
        {
            return new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = 1, ItemId = 1, Item = _items[0], Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6), Container = "Box", Location = "A1", Status = "Inventory" }
            };
        }

        private List<InventoryItemViewModel> GetReceivingItems()
        {
            return new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = 2, ItemId = 2,  Item = _items[1], Quantity = 50, ExpiryDate = DateTime.Now.AddMonths(3), Container = "Pallet", Location = "Receiving Area", Status = "Receiving" }
            };
        }

        private List<InventoryItemViewModel> GetPackingItems()
        {
            return new List<InventoryItemViewModel>
            {
                new InventoryItemViewModel { Id = 3, ItemId = 1, Item = _items[0], Quantity = 30, ExpiryDate = DateTime.Now.AddMonths(1), Container = "Carton", Location = "Picking Area", Status = "Picking" }
            };
        }

        private static WarehouseViewModel2 _warehouse = new WarehouseViewModel2
        {
            Id = 1,
            Name = "Main Warehouse",
            Location = "123 Warehouse St",
            Width = 100,
            Length = 200,
            Height = 30,
            Racks = new List<Rack>
            {
                new Rack
                {
                    Id = 1,
                    Number = 1,
                    WarehouseId = 1,
                    AisleId = 1,
                    Shelves = new List<Shelf>
                    {
                        new Shelf
                        {
                            Id = 1,
                            Number = 1,
                            RackId = 1,
                            Bins = new List<Bin>
                            {
                                new Bin { Id = 1, Number = 1, ShelfId = 1 },
                                new Bin { Id = 2, Number = 2, ShelfId = 1 }
                            }
                        },
                        new Shelf
                        {
                            Id = 2,
                            Number = 2,
                            RackId = 1,
                            Bins = new List<Bin>
                            {
                                new Bin { Id = 3, Number = 1, ShelfId = 2 },
                                new Bin { Id = 4, Number = 2, ShelfId = 2 }
                            }
                        }
                    }
                },
                new Rack
                {
                    Id = 2,
                    Number = 2,
                    WarehouseId = 1,
                    AisleId = 1,
                    Shelves = new List<Shelf>
                    {
                        new Shelf
                        {
                            Id = 3,
                            Number = 1,
                            RackId = 2,
                            Bins = new List<Bin>
                            {
                                new Bin { Id = 5, Number = 1, ShelfId = 3 },
                                new Bin { Id = 6, Number = 2, ShelfId = 3 }
                            }
                        },
                        new Shelf
                        {
                            Id = 4,
                            Number = 2,
                            RackId = 2,
                            Bins = new List<Bin>
                            {
                                new Bin { Id = 7, Number = 1, ShelfId = 4 },
                                new Bin { Id = 8, Number = 2, ShelfId = 4 }
                            }
                        }
                    }
                }
            }
        };
    }
}
