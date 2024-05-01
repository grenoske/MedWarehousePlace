using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class LocationWarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
                    cells.Add(new Cell { Id = i + 1, Row = i / wh.Length, Column = i % wh.Length });
                }
                wh.Cells = cells;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cell not null");
                var occupiedCells = wh.Cells.Where(c => c.IsOccupied).ToList();
                foreach (var cell in wh.Cells)
                {
                    if (cell.IsOccupied)
                        System.Diagnostics.Debug.WriteLine(cell.Id);
                }
            }

            return View(wh);
        }
    }
}
