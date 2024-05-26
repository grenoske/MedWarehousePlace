﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Rack
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int AisleId { get; set; }
        public Aisle Aisle { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<Cell> Cells { get; set; }
    }
}