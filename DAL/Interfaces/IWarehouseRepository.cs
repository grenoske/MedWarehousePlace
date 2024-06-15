using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        void Update(Warehouse obj);
    }
}
