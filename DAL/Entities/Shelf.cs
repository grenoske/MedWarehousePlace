using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Shelf
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RackId { get; set; }
        public Rack Rack { get; set; }
        public List<Bin> Bins { get; set; }
    }
}
