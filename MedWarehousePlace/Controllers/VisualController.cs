using Microsoft.AspNetCore.Mvc;
using PL.Models.Location;
using PL.Models;
using System.Diagnostics;
using BLL.Services;
using BLL.Interfaces;
using BLL.DTO;

namespace PL.Controllers
{
    public class VisualController : Controller
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IInventoryService _inventoryService;

        public VisualController(IWarehouseService warehouseService, IInventoryService inventoryService)
        {
            _warehouseService = warehouseService;
            _inventoryService = inventoryService;
        }
        public IActionResult Index(int wareHouseId = 1, int level = 1)
        {
            WarehouseViewModel2 warehouse = (WarehouseViewModel2)_warehouseService.GetWarehouseLevelById(wareHouseId, level);
            var viewModel = new VisualViewModel { Warehouse = warehouse, CurrentLevel = level };
            return View(viewModel);
        }

        public IActionResult Edit(int wareHouseId = 1)
        {
            var warehouseDto = _warehouseService.GetWarehouseById(wareHouseId);
            WarehouseViewModel2 warehouse = (WarehouseViewModel2)warehouseDto;
            var viewModel = new VisualViewModel { Warehouse = warehouse };
            return View(viewModel);
        }

        public IActionResult GetItemById(int id)
        {
            var itemViewModel = (InventoryItemViewModel)_inventoryService.GetInventoryItemById(id);
            return PartialView("_ItemDetailsPartial", itemViewModel);
        }

        [HttpPost]
        public IActionResult CreateRack(VisualViewModel viewModel)
        {
            var warehouse = viewModel.Warehouse;

            var occupiedCells = warehouse.Cells.Where(c => c.IsSelected).Select(c => new CellDTO() { Id = c.Id }).ToList();
            _warehouseService.CreateRack(new RackDTO { WarehouseId = warehouse.Id, Number = viewModel.Number, Cells = occupiedCells });

            return RedirectToAction("Edit", new { warehouseId = warehouse.Id });
        }

        [HttpPost]
        public IActionResult CreateAisle(VisualViewModel viewModel)
        {
            var warehouse = viewModel.Warehouse;
            Debug.WriteLine("SaveAisle");
            if (warehouse.Cells == null)
            {
                Debug.WriteLine("Cell null");
            }

            var occupiedCells = warehouse.Cells.Where(c => c.IsSelected).Select(c => new CellDTO() { Id = c.Id}).ToList();
            //warehouse.Aisles = new List<Aisle> { new Aisle { Id = 0, Number = 0, Cells = occupiedCells } };
            _warehouseService.CreateAisle(new AisleDTO { WarehouseId = warehouse.Id, Number = viewModel.Number, Cells = occupiedCells });
            return RedirectToAction("Edit", new { warehouseId = warehouse.Id });
        }
    }
}
