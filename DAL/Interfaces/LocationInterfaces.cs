using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IAisleRepository : IRepository<Aisle> { void Update(Aisle obj); }
    public interface IRackRepository : IRepository<Rack> { void Update(Rack obj); }
    public interface IShelfRepository : IRepository<Shelf> { void Update(Shelf obj); }
    public interface IBinRepository : IRepository<Bin> { void Update(Bin obj); }
}
