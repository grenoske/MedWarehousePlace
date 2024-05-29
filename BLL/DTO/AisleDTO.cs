using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class AisleDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public List<RackDTO> Racks { get; set; }
        public List<CellDTO> Cells { get; set; }
    }
}
