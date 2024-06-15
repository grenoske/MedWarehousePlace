using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class AisleRepository : Repository<Aisle>, IAisleRepository
    {
        private readonly ApplicationDbContext _db;
        public AisleRepository(ApplicationDbContext db) : base(db) => _db = db;
        public void Update(Aisle obj)
        {
            _db.Aisles.Update(obj);
        }
    }

    public class RackRepository : Repository<Rack>, IRackRepository
    {
        private readonly ApplicationDbContext _db;
        public RackRepository(ApplicationDbContext db) : base(db) => _db = db;
        public void Update(Rack obj)
        {
            _db.Racks.Update(obj);
        }
    }

    public class ShelfRepository : Repository<Shelf>, IShelfRepository
    {
        private readonly ApplicationDbContext _db;
        public ShelfRepository(ApplicationDbContext db) : base(db) => _db = db;
        public void Update(Shelf obj)
        {
            _db.Shelves.Update(obj);
        }
    }

    public class BinRepository : Repository<Bin>, IBinRepository
    {
        private readonly ApplicationDbContext _db;
        public BinRepository(ApplicationDbContext db) : base(db) => _db = db;
        public void Update(Bin obj)
        {
            _db.Bins.Update(obj);
        }
    }
}
