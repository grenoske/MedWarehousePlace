using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using PL.Models;
using PL.Models.Location;
using System.ComponentModel.DataAnnotations;

namespace PL.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            
            return View(GetAllWarehouse());
        }
        public IActionResult Create()
        {
            System.Diagnostics.Debug.WriteLine("Create not post");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Warehouse wh)
        {
            if (wh == null)
            {
                System.Diagnostics.Debug.WriteLine("wh null");
            }
            if (wh.Cells == null)
            {
                System.Diagnostics.Debug.WriteLine("Cell null");
                var cells = new List<Cell>();

                int totalCells = wh.Width * wh.Length;

                for (int i = 0; i < totalCells; i++)
                {
                    cells.Add(new Cell { Id = i + 1 });
                }
                wh.Cells = cells;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cell not null");
                var occupiedCells = wh.Cells.Where(c => c.IsSelected).ToList();
                foreach (var cell in wh.Cells)
                {
                    if (cell.IsSelected)
                        System.Diagnostics.Debug.WriteLine(cell.Id);
                }
            }

            return View(wh);
        }

        public IActionResult Edit(int? wareHouseId)
        {
            if (wareHouseId == null || wareHouseId == 0)
            {
                return NotFound();
            }

            Warehouse? wh = GetWarehouse(wareHouseId);

            if (wh == null)
            {
                return NotFound();
            }
            return View(wh);
        }

        [HttpPost]
        public IActionResult Edit(Warehouse obj)
        {
            if (ModelState.IsValid)
            {
                UpdateWarehouse(obj);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ItemList()
        {
            return View(GetAllItem());
        }

        public IActionResult Dashboard()
        {
            return View();
        }













        // TEMP BLL
        public IEnumerable<Cell> GetCellsMap(int? WarehouseId)
        {
            int length = 5;
            int width = 5;
            List <Cell> cells = new List <Cell>();
            for (int i = 0; i < length*width; i++)
            {
                cells.Add(new Cell { Id = i + 1});
            }
            return cells;
        }

        public Warehouse GetWarehouse(int? id)
        {
            int length = 5;
            int width = 5;
            int height = 5;
            List<Cell> cells = new List<Cell>();
            
            for (int i = 0; i < length * width; i++)
            {
                cells.Add(new Cell { Id = i + 1 });
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
            List<Item> items =
            [
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-10", Quantity=122, ExpiryDate = new DateTime(2024, 12, 31)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-11", Quantity=123, ExpiryDate = new DateTime(2024, 05, 06)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-12", Quantity=144, ExpiryDate = new DateTime(2024, 01, 06)}
            ];
            return new Warehouse { Id = 1, Name = "Warehouse 1", Location = "St. Ivana Franka 22a", Width = width, Height = height, Length = length, Cells=cells, Racks=racks, Aisles=aisles, Items = items };

        }

        public void UpdateWarehouse(Warehouse wh)
        {
            
        }

        public IEnumerable<Warehouse> GetAllWarehouse()
        {
            int length = 5;
            int width = 5;
            int height = 5;
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < length * width; i++)
            {
                cells.Add(new Cell { Id = i + 1});
            }
            List<Warehouse> warehouses =
            [
                new Warehouse { Id = 1, Name="Warehouse 1", Location = "St. Ivana Franka 22a", Width = width, Height = height, Length = length, Cells = cells },
                new Warehouse { Id = 2, Name="Warehouse 2", Location = "St. Khreshchatyk 1b", Width = width, Height = height, Length = length, Cells = cells }
            ];
            return warehouses;

        }

        public IEnumerable<Item> GetAllItem()
        {
            List<Item> items =
            [
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-10", Quantity=122, ExpiryDate = new DateTime(2024, 12, 31)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-11", Quantity=123, ExpiryDate = new DateTime(2024, 05, 06)},
                new Item { Id = 1, Container="Europallet", Product="Antibiotics", Location="01-02-01-12", Quantity=144, ExpiryDate = new DateTime(2024, 01, 06)}
            ];
            return items;
        }
    }
}
