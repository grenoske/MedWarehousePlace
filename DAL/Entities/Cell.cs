using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Cell
    {
        public int Id { get; set; }
        public string LabeledText { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAisle { get; set; }
        public bool IsRack { get; set; }
        public bool IsNotEmpty { get; set; }
    }
}
