using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        void Update(InventoryItem obj);
    }
}
