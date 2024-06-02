using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class CellDTO
    {
        public int Id { get; set; }
        public int? InventoryItemId { get; set; }
        public bool IsRack { get; set; }
        public bool IsAisle { get; set; }
        public bool IsNotEmpty { get; set; }
        public string LabeledText { get; set; }
    }
}
