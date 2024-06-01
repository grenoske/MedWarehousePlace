using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InventoryItemRepository : Repository<InventoryItem>, IInventoryItemRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InventoryItem obj)
        {
            var objFromDb = _db.InventoryItems.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Quantity = obj.Quantity;
                objFromDb.ExpiryDate = obj.ExpiryDate;
                objFromDb.Container = obj.Container;
                objFromDb.Location = obj.Location;
                objFromDb.LocationDest = obj.LocationDest;
                objFromDb.Status = obj.Status;
                objFromDb.BinId = obj.BinId;
                objFromDb.ItemId = obj.ItemId;

                if (obj.BinId != null)
                {
                    var binFromDb = _db.Bins.FirstOrDefault(b => b.Id == obj.BinId);
                    if (binFromDb != null)
                    {
                        binFromDb.InventoryItemId = obj.Id;
                        binFromDb.InventoryItem = objFromDb;
                    }
                }
            }


        }
    }
}
