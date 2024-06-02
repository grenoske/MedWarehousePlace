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
        WarehouseDTO GetWarehouseLevelById(int id, int level);
        void CreateWarehouse(WarehouseDTO warehouseDto);
        void CreateAisle(AisleDTO aisleDto);
        void CreateRack(RackDTO rackDto);
        void UpdateWarehouse(WarehouseDTO warehouseDto);

        IEnumerable<RackDTO> GetRacks(int warehouseId = 1);
        IEnumerable<ShelfDTO> GetShelves(int rackId);
        IEnumerable<BinDTO> GetBins(int shelfId, bool empty = true);

        void Dispose();
    }
}
