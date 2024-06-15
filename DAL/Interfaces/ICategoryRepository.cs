using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
