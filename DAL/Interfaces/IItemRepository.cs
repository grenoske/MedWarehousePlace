using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        void Update(Item obj);
    }
}
