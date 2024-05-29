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

        public VisualController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        public IActionResult Index(int? wareHouseId = 1)
        {
            WarehouseViewModel2 warehouse = GetWarehouse(wareHouseId);
            var viewModel = new VisualViewModel { Warehouse = warehouse };
            return View(viewModel);
        }

        public IActionResult Edit(int wareHouseId = 1)
        {
            var warehouseDto = _warehouseService.GetWarehouseById(wareHouseId);
            WarehouseViewModel2 warehouse = (WarehouseViewModel2)warehouseDto;
            var viewModel = new VisualViewModel { Warehouse = warehouse };
            Debug.WriteLine("Cell Number: " + viewModel.Warehouse.Cells.Count);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateRack(VisualViewModel viewModel)
        {
            var warehouse = viewModel.Warehouse;
            Debug.WriteLine("Cells count when create rack: " + viewModel.Warehouse.Cells.Count);
            Debug.WriteLine("SaveRack");
            if (warehouse.Cells == null)
            {
                Debug.WriteLine("Cell null");
            }
            var occupiedCells = warehouse.Cells.Where(c => c.IsSelected).Select(c => new CellDTO() { Id = c.Id }).ToList();

            /*            foreach (var cell in occupiedCells)
                        {
                            cell.IsRack = true;
                            cell.IsSelected = false;
                        }

                        foreach (var cell in warehouse.Cells)
                        {
                            if (occupiedCells.Contains(cell))
                            {
                                cell.IsRack = true;
                                cell.IsSelected = false;
                            }
                        }
                        warehouse.Racks = new List<Rack> { new Rack { Cells = occupiedCells } };*/
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

        // Mock method to retrieve a warehouse (replace with actual data retrieval)
        private WarehouseViewModel2 GetWarehouse(int? id)
        {
            int length = 5;
            int width = 5;
            int height = 5;
            List<CellViewModel> cells = new List<CellViewModel>();

            for (int i = 0; i < length * width; i++)
            {
                cells.Add(new CellViewModel { Id = i + 1 });
            }

            List<Aisle> aisles = new List<Aisle>()
            {
                new Aisle { Id = 1, Number = 1, WarehouseId = 1},
                new Aisle { Id = 2, Number = 2, WarehouseId = 1},
                new Aisle { Id = 3, Number = 3, WarehouseId = 1}
            };

            List<Rack> racks = new List<Rack>()
            {
                 new Rack { Id = 1, Number = 1, WarehouseId = 1},
                 new Rack { Id = 2, Number = 2, WarehouseId = 1},
                 new Rack { Id = 3, Number = 3, WarehouseId = 1}
            };

            cells[0].IsRack = true;
            cells[0].LabeledText = "1 R1";
            cells[5].IsRack = true;
            cells[5].LabeledText = " 2 ";
            cells[10].IsRack = true;
            cells[10].IsNotEmpty = true;
            cells[10].LabeledText = " 3 ";
            cells[1].IsAisle = true;
            cells[1].LabeledText = " A1 ";
            cells[6].IsAisle = true;
            cells[11].IsAisle = true;

/*            List<Item> items = new List<Item>
            {
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-10", Quantity=122, ExpiryDate = new DateTime(2024, 12, 31)},
                new Item { Id = 2, Container="Europallet", Product="Antibiotics", Location="01-02-01-11", Quantity=123, ExpiryDate = new DateTime(2024, 05, 06)},
                new Item { Id = 3, Container="Europallet", Product="Antibiotics", Location="01-02-01-12", Quantity=144, ExpiryDate = new DateTime(2024, 01, 06)}
            };*/
            return new WarehouseViewModel2 { Id = 1, Name = "Warehouse 1", Location = "St. Ivana Franka 22a", Width = width, Height = height, Length = length, Cells = cells, Racks = racks, Aisles = aisles};
        }
    }
}
