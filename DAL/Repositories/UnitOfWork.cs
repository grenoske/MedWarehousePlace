﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IInventoryItemRepository InventoryItems { get; private set; }
        public IItemRepository Items { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IWarehouseRepository Warehouses { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            InventoryItems = new InventoryItemRepository(_db);
            Items = new ItemRepository(_db);
            Categories = new CategoryRepository(_db);
            Warehouses = new WarehouseRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
