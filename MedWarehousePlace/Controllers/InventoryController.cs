using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Models;
using PL.Models.Location;
using System.ComponentModel;

namespace PL.Controllers
{
    public class InventoryController : Controller
    {
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = GetInventoryItems(),
                Title = "Inventory"
            };
            return View("InventoryView", model);
        }

        public IActionResult Receiving()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = GetReceivingItems(),
                Title = "Receiving"
            };
            return View("InventoryView", model);
        }

        public IActionResult Packing()
        {
            var model = new InventoryListViewModel
            {
                InventoryItems = GetPackingItems(),
                Title = "Packing"
            };
            return View("InventoryView", model);
        }

        public IActionResult PickUpItem(int itemId)
        {
            var item = GetInventoryItems()[itemId-2];
            if (item != null)
            {
                var model = new WorkerItemViewModel
                {
                    Id = _workerItems.Count + 1,
                    InventoryItem = item,
                    Racks = GetRackSelectList(),
                    Shelves = new List<SelectListItem>(), // Спочатку порожній список
                    Bins = new List<SelectListItem>() // Спочатку порожній список
                };
                return View(model);
            }
            return RedirectToAction("Inventory");
        }

        [HttpPost]
        public IActionResult ConfirmPickUp(WorkerItemViewModel model)
        {
            /*           if (ModelState.IsValid)
                       {
                           model.LocationTo = "Worker's Hand"; // або інша логіка для визначення кінцевої локації
                           _workerItems.Add(model);
                           return RedirectToAction("WorkerItems");
                       }
                       return View("PickUpItem", model);*/
            _workerItems.Add(model);
            model.Racks = GetRackSelectList();
            model.Shelves = new List<SelectListItem>(); // Перезавантажуємо список полиць
            model.Bins = new List<SelectListItem>();
            return RedirectToAction("WorkerItems");
        }

        // Метод для отримання списку стелажів
        private IEnumerable<SelectListItem> GetRackSelectList()
        {
            return _warehouse.Racks.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"Rack {r.Number}"
            });
        }

        // Метод для отримання списку полиць на основі стелажа
        public JsonResult GetShelves(int rackId)
        {
            var shelves = _warehouse.Racks.FirstOrDefault(r => r.Id == rackId)?.Shelves.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"Shelf {s.Number}"
            });
            return Json(shelves);
        }

        // Метод для отримання списку кошиків на основі полиці
        public JsonResult GetBins(int shelfId)
        {
            var bins = _warehouse.Racks.SelectMany(r => r.Shelves)
                .FirstOrDefault(s => s.Id == shelfId)?.Bins.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = $"Bin {b.Number}"
                });
            return Json(bins);
        }

        public IActionResult PlaceItem(int workerItemId)
        {
            var workerItem = _workerItems.FirstOrDefault(wi => wi.Id == workerItemId);
            if (workerItem != null)
            {
                // Logic to move item from worker's hand to target location (Inventory or PackingArea)
                _workerItems.Remove(workerItem);
            }
            return RedirectToAction("WorkerItems");
        }

        public IActionResult WorkerItems()
        {
            var model = new WorkerItemListViewModel
            {
                WorkerItems = _workerItems
            };
            return View(model);
        }


        private static Warehouse _warehouse = new Warehouse
        {
            Id = 1,
            Name = "Main Warehouse",
            Location = "123 Warehouse St",
            Width = 100,
            Length = 200,
            Height = 30,
            CellSize = 10,
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
