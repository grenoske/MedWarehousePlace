using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PL.Models;
using PL.Models.Location;
using System.ComponentModel.DataAnnotations;

namespace PL.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        public IActionResult Index()
        {
            IEnumerable<WarehouseDTO> warehouses = _warehouseService.GetAllWarehouses();
            var warehousesView = warehouses.Select(wh => (WarehouseViewModel)wh);
            return View(warehousesView);
        }
        public IActionResult Create()
        {
            System.Diagnostics.Debug.WriteLine("Create not post");
            return View();
        }
        [HttpPost]
        public IActionResult Create(WarehouseViewModel wh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _warehouseService.CreateWarehouse(new WarehouseDTO { Name = wh.Name, Width = wh.Width, Length = wh.Length, Height = wh.Height, Location = wh.Location });
                }
                catch (ValidationException ex)
                {
                    ViewBag.Message = (ex.Message);
                    return View(wh);
                }
                return RedirectToAction(nameof(Index));

            }
            return View(wh);
        }

        public IActionResult Edit(int? wareHouseId)
        {
            if (wareHouseId == null || wareHouseId == 0)
            {
                return NotFound();
            }

            WarehouseViewModel2? wh = GetWarehouse(wareHouseId);

            if (wh == null)
            {
                return NotFound();
            }
            return View(wh);
        }

        [HttpPost]
        public IActionResult Edit(WarehouseViewModel2 obj)
        {
            if (ModelState.IsValid)
            {
                UpdateWarehouse(obj);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }













        // TEMP BLL
        public IEnumerable<CellViewModel> GetCellsMap(int? WarehouseId)
        {
            int length = 5;
            int width = 5;
            List <CellViewModel> cells = new List <CellViewModel>();
            for (int i = 0; i < length*width; i++)
            {
                cells.Add(new CellViewModel { Id = i + 1});
            }
            return cells;
        }

        public WarehouseViewModel2 GetWarehouse(int? id)
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
            /*List<Item> items =
            [
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-10", Quantity=122, ExpiryDate = new DateTime(2024, 12, 31)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-11", Quantity=123, ExpiryDate = new DateTime(2024, 05, 06)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-12", Quantity=144, ExpiryDate = new DateTime(2024, 01, 06)}
            ];*/
            return new WarehouseViewModel2 { Id = 1, Name = "Warehouse 1", Location = "St. Ivana Franka 22a", Width = width, Height = height, Length = length, Cells=cells, Racks=racks, Aisles=aisles };

        }

        public void UpdateWarehouse(WarehouseViewModel2 wh)
        {
            
        }

        public IEnumerable<WarehouseViewModel2> GetAllWarehouse()
        {
            int length = 5;
            int width = 5;
            int height = 5;
            List<CellViewModel> cells = new List<CellViewModel>();
            for (int i = 0; i < length * width; i++)
            {
                cells.Add(new CellViewModel { Id = i + 1});
            }
            List<WarehouseViewModel2> warehouses =
            [
                new WarehouseViewModel2 { Id = 1, Name="Warehouse 1", Location = "St. Ivana Franka 22a", Width = width, Height = height, Length = length, Cells = cells },
                new WarehouseViewModel2 { Id = 2, Name="Warehouse 2", Location = "St. Khreshchatyk 1b", Width = width, Height = height, Length = length, Cells = cells }
            ];
            return warehouses;

        }

/*        public IEnumerable<Item> GetAllItem()
        {
            List<Item> items =
            [
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-10", Quantity=122, ExpiryDate = new DateTime(2024, 12, 31)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-11", Quantity=123, ExpiryDate = new DateTime(2024, 05, 06)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-12", Quantity=144, ExpiryDate = new DateTime(2024, 01, 06)}
            ];
            return items;
        }*/
    }
}
