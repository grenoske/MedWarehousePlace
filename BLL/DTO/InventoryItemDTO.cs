using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class InventoryItemDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemDTO Item { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Container { get; set; }
        public string Location { get; set; }
        public string? LocationDest { get; set; }
        public string Status { get; set; }

        public int? BinId { get; set; }
        public BinDTO Bin { get; set; }
    }
}
