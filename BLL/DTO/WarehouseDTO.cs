using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class WarehouseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
        public List<CellDTO> Cells { get; set; }
        public List<AisleDTO> Aisles { get; set; }
        public List<RackDTO> Racks { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
