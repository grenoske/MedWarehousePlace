using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure.AdditionalFunction
{
    public static class AdditionalFunction
    {
        public static (int Row, int Column) ToCoord(Cell cell, int width)
        {
            int row = (cell.Number - 1) / width;
            int column = (cell.Number - 1) % width;
            return (row, column);
        }
    }
}
