using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public double Cost { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
