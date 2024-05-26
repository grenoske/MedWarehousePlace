using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAisleRepository : IRepository<Aisle> { void Update(Aisle obj); }
    public interface IRackRepository : IRepository<Rack> { void Update(Rack obj); }
    public interface IShelfRepository : IRepository<Shelf> { void Update(Shelf obj); }
    public interface IBinRepository : IRepository<Bin> { void Update(Bin obj); }
}
