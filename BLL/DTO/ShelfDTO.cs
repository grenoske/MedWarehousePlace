using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ShelfDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RackId { get; set; }
        public List<BinDTO> Bins { get; set; }
    }
}
