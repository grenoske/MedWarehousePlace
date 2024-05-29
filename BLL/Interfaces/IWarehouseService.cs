using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWarehouseService
    {
        IEnumerable<WarehouseDTO> GetAllWarehouses();
        WarehouseDTO GetWarehouseById(int id);
        void CreateWarehouse(WarehouseDTO warehouseDto);
        void CreateAisle(AisleDTO aisleDto);
        void CreateRack(RackDTO rackDto);
        void UpdateWarehouse(WarehouseDTO warehouseDto);
    }
}
