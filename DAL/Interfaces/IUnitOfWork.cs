namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IInventoryItemRepository InventoryItems { get; }
        IItemRepository Items { get; }
        ICategoryRepository Categories { get; }
        IWarehouseRepository Warehouses { get; }

        // Location
        IRackRepository Racks { get; }
        IAisleRepository Aisles { get; }
        IShelfRepository Shelves { get; }
        IBinRepository Bins { get; }

        // User
        IUserRepository Users { get; }
        void Save();
    }
}
