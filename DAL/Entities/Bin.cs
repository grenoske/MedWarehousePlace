﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Bin
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }
    }
}