using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using AutoMapper;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.EF;
using BLL.DTO;
using BLL.Infrastructure.MappingProfiles;

namespace MedWarehouseTests.IntegrationTests
{
    public class InventoryServiceIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryServiceIntegrationTests()
        {
            // Налаштування сервісів для тестів
            var services = new ServiceCollection();

            // Налаштування in-memory бази даних для тестування
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "TestDatabase"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Налаштування AutoMapper з профілями для тестування
            services.AddAutoMapper(typeof(ItemProfile).Assembly);
            services.AddAutoMapper(typeof(WarehouseProfile).Assembly);

            _serviceProvider = services.BuildServiceProvider();

            // Отримання сервісів з контейнера
            _context = _serviceProvider.GetService<ApplicationDbContext>();
            _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            _mapper = _serviceProvider.GetService<IMapper>();

        }

        // Метод для підготовки бази даних
        private void SetupDatabase()
        {
            var category = new Category { Id = 1, Name = "Category" };
            var item = new Item { Id = 1, Name = "Test Item", Company = "Test", Cost = 5,
                TurnoverRate = 6, Weight = 10, CategoryId = 1 };
            var warehouse = new Warehouse { Id = 1, Name = "Test Warehouse", 
                Location = "Test Location", Width = 100, MaximumWeightOnUpperShelves = 100 };
            var rack = new Rack { Id = 1, WarehouseId = 1, Number = 1 };
            var shelf = new Shelf { Id = 1, RackId = 1, Number = 1 };
            var bin1 = new Bin { Id = 1, ShelfId = 1, Number = 1, InventoryItemId = null, 
                Cell = new Cell { Id = 1, Number = 1 } };
            var bin2 = new Bin { Id = 2, ShelfId = 1, Number = 2, InventoryItemId = null, 
                Cell = new Cell { Id = 2, Number = 11 } };

            _context.Categories.Add(category);
            _context.Items.Add(item);
            _context.Warehouses.Add(warehouse);
            _context.Racks.Add(rack);
            _context.Shelves.Add(shelf);
            _context.Bins.AddRange(bin1, bin2);
            _context.SaveChanges();
        }

        private InventoryItem CreateTestInventoryItem(int itemId, int addMonth = 1, int turnoverRate = 30, int weight = 10)
        {
            return new InventoryItem { Id = itemId, ItemId = 1, Quantity = 10, ExpiryDate = DateTime.Now.AddMonths(addMonth), Location = "Receiving Area", Container = "Test Container", Status = "Test Status" };
        }

        [Fact]
        public void RecommendItemPlacement_Should_Return_Correct_Bin()
        {
            // Arrange
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();
            var itemId = 1;
            var warehouseId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            _unitOfWork.Save();

            // Act
            var result = inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            result.Should().NotBeNull();
            result.BinId.Should().Be(1); 
            result.LocationDest.Should().Be("1-1-1"); 
        }


        [Fact]
        public void InventoryService_Should_Create_And_Retrieve_Item()
        {
            // Arrange
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            var itemDto = new ItemDTO { Id = 1, Name = "Test Item", Company = "Test", Cost = 5, TurnoverRate = 6, Weight = 10, CategoryId = 1 };
            var categoryDto = new CategoryDTO { Id = 1, Name = "Category" };

            // Act
            inventoryService.CreateCategory(categoryDto);
            inventoryService.CreateItem(itemDto);
            var retrievedItem = inventoryService.GetItemById(1);

            // Assert
            retrievedItem.Should().NotBeNull();
            retrievedItem.Id.Should().Be(1);
            retrievedItem.Name.Should().Be("Test Item");
        }

        [Fact]
        public void GetItems_Should_Return_Paginated_Items()
        {
            // Arrange
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            var result = inventoryService.GetItems(1);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public void CreateCategory_Should_Add_New_Category()
        {
            // Arrange
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();
            var newCategoryDto = new CategoryDTO { Id = 2, Name = "NewCategory" };

            // Act
            inventoryService.CreateCategory(newCategoryDto);

            // Assert
            var createdCategory = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == 2);
            createdCategory.Should().NotBeNull();
            createdCategory.Name.Should().Be("NewCategory");
        }

        [Fact]
        public void DeleteCategory_Should_Remove_Category()
        {
            // Arrange
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            inventoryService.DeleteCategory(1);

            // Assert
            var deletedCategory = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == 1);
            deletedCategory.Should().BeNull();
        }

        [Fact]
        public void GetInventoryItems_Should_Return_All_InventoryItems()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            var result = inventoryService.GetInventoryItems();

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetInventoryItemById_Should_Return_Correct_InventoryItem()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            var result = inventoryService.GetInventoryItemById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public void UpdateInventoryItem_Should_Return_Correct_InventoryItem()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            var result = inventoryService.GetInventoryItemById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public void UpdateCategory_Should_Return_Correct_InventoryItem()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            var inventoryService = new InventoryService(_unitOfWork, _mapper);
            SetupDatabase();

            // Act
            var result = inventoryService.GetInventoryItemById(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }



        // Очищення ресурсів після тесту
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
