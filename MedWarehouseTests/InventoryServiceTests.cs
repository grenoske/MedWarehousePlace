using BLL.Services;
using DAL.Interfaces;
using DAL.Entities;
using Moq;
using System.Linq.Expressions;
using FluentAssertions;

namespace BLL.Tests.Services
{
    public class InventoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _inventoryService = new InventoryService(_unitOfWorkMock.Object, null);
        }

        private void SetupInventoryItemRepositoryMock(int itemId, InventoryItem inventoryItem)
        {
            var inventoryItemRepositoryMock = new Mock<IInventoryItemRepository>();
            inventoryItemRepositoryMock.Setup(repo => repo.GetFirstOrDefault(
                                It.IsAny<Expression<Func<InventoryItem, bool>>>(),
                                It.IsAny<string>(),
                                It.IsAny<bool>()
                                )).Returns(inventoryItem);

            _unitOfWorkMock.SetupGet(uow => uow.InventoryItems)
                .Returns(inventoryItemRepositoryMock.Object);
        }

        private void SetupWarehouseRepositoryMock(int warehouseId, Warehouse warehouse)
        {
            var warehouseRepositoryMock = new Mock<IWarehouseRepository>();
            warehouseRepositoryMock.Setup(repo => repo.GetFirstOrDefault(
                                    It.IsAny<Expression<Func<Warehouse, bool>>>(),
                                    It.IsAny<string>(),
                                    It.IsAny<bool>()
                                    )).Returns(warehouse);

            _unitOfWorkMock.SetupGet(uow => uow.Warehouses)
                .Returns(warehouseRepositoryMock.Object);
        }

        private void SetupRackRepositoryMock(Rack[] racks)
        {
            var rackRepositoryMock = new Mock<IRackRepository>();
            rackRepositoryMock.Setup(repo => repo.GetAll(
                               It.IsAny<Expression<Func<Rack, bool>>>(),
                               It.IsAny<int>(),
                               It.IsAny<int>(),
                               It.IsAny<string>()
                               )).Returns(racks.AsQueryable);

            _unitOfWorkMock.SetupGet(uow => uow.Racks)
                .Returns(rackRepositoryMock.Object);
        }

        private Warehouse CreateTestWarehouse(int id)
        {
            return new Warehouse { Id = id, MaximumWeightOnUpperShelves = 100, Width = 50 };
        }

        private InventoryItem CreateTestInventoryItem(int itemId, int addMonth = 1, int turnoverRate = 30, int weight = 10)
        {
            var item = new Item { Id = itemId, Weight = weight, TurnoverRate = turnoverRate};
            return new InventoryItem { Id = itemId, Item = item, Quantity = 10, ExpiryDate = DateTime.Now.AddMonths(addMonth) };
        }

        private Rack CreateTestRack(int rackId, int warehouseId, bool notempty = false)
        {
            var rack = new Rack { Id = rackId, WarehouseId = warehouseId };
            var shelf = new Shelf { Id = 1, RackId = rack.Id, Number = 1 };
            var shelf2 = new Shelf { Id = 2, RackId = rack.Id, Number = 2 };
            var bin = new Bin { Id = 1, ShelfId = shelf.Id, InventoryItemId = null, Cell = new Cell { Number = 2 } };
            var bin2 = new Bin { Id = 2, ShelfId = shelf.Id, InventoryItemId = null, Cell = new Cell { Number = 11 } };
            var bin3 = new Bin { Id = 3, ShelfId = shelf.Id, InventoryItemId = null, Cell = new Cell { Number = 12 } };
            var bin4 = new Bin { Id = 4, ShelfId = shelf2.Id, InventoryItemId = null, Cell = new Cell { Number = 12 } };
            if (notempty)
            {
                bin.InventoryItemId = 1;
                bin2.InventoryItemId = 2;
                bin3.InventoryItemId = 3;
                bin4.InventoryItemId = 4;
            }
            shelf.Bins = new List<Bin> { bin, bin2, bin3 };
            shelf2.Bins = new List<Bin> { bin4 };
            rack.Shelves = new List<Shelf> { shelf, shelf2 };
            return rack;
        }

        [Fact]
        public void RecommendItemPlacement_InputShortExpireDate_ReturnsClosestBin()
        {
            // Arrange
            var warehouseId = 1;
            var warehouse = CreateTestWarehouse(warehouseId);
            SetupWarehouseRepositoryMock(warehouseId, warehouse);

            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var rack = CreateTestRack(1, warehouseId);
            SetupRackRepositoryMock(new[] { rack });

            // Act
            var result = _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            result.Should().NotBeNull();
            result.BinId.Should().Be(1); 
            result.LocationDest.Should().Be(rack.Number + "-" 
                + rack.Shelves.First().Number + "-" 
                + rack.Shelves.First().Bins.First().Number);
        }

        [Fact]
        public void RecommendItemPlacement_InputHighTurnoverRate_ReturnsMiddleBin()
        {
            // Arrange
            var warehouseId = 1;
            var middle = 3;
            var warehouse = CreateTestWarehouse(warehouseId);
            SetupWarehouseRepositoryMock(warehouseId, warehouse);

            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId, addMonth:3);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var rack = CreateTestRack(1, warehouseId);
            SetupRackRepositoryMock(new[] { rack });

            // Act
            var result = _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            result.Should().NotBeNull();
            result.BinId.Should().Be(middle);
            result.LocationDest.Should().Be(rack.Number + "-"
                + rack.Shelves.First().Number + "-"
                + rack.Shelves.First().Bins.ElementAtOrDefault(middle-1).Number);
        }

        [Fact]
        public void RecommendItemPlacement_InputLowTurnoverRateAndLongExpireDate_ReturnsFarthestBin()
        {
            // Arrange
            var warehouseId = 1;
            var warehouse = CreateTestWarehouse(warehouseId);
            SetupWarehouseRepositoryMock(warehouseId, warehouse);

            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId, turnoverRate:10, addMonth:3);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var rack = CreateTestRack(1, warehouseId);
            SetupRackRepositoryMock(new[] { rack });

            // Act
            var result = _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            result.Should().NotBeNull();
            result.BinId.Should().Be(4);
            result.LocationDest.Should().Be(rack.Number + "-"
                + rack.Shelves.Last().Number + "-"
                + rack.Shelves.First().Bins.Last().Number);
        }

        [Fact]
        public void RecommendItemPlacement_InputHeavyEquipment_ReturnsBinNotOnHigherShelves()
        {
            // Arrange
            var warehouseId = 1;
            var warehouse = CreateTestWarehouse(warehouseId);
            SetupWarehouseRepositoryMock(warehouseId, warehouse);

            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId, weight:50);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var rack = CreateTestRack(1, warehouseId);
            SetupRackRepositoryMock(new[] { rack });

            // Act
            var result = _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            result.Should().NotBeNull();
            result.BinId.Should().NotBe(4);
        }

        [Fact]
        public void RecommendItemPlacement_InputValidButNoPlace_ThrowsException()
        {
            // Arrange
            var warehouseId = 1;
            var warehouse = CreateTestWarehouse(warehouseId);
            SetupWarehouseRepositoryMock(warehouseId, warehouse);

            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var rack = CreateTestRack(1, warehouseId, notempty:true);
            SetupRackRepositoryMock(new[] { rack });

            // Act
            Action act = () => _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            act.Should().Throw<Exception>().WithMessage("No available bins found.");
        }

        [Fact]
        public void RecommendItemPlacement_ItemNotFound_ThrowsException()
        {
            // Arrange
            var itemId = 1;
            SetupInventoryItemRepositoryMock(itemId, null);

            var warehouseId = 1;

            // Act
            Action act = () => _inventoryService.RecommendItemPlacement(itemId, warehouseId);

            // Assert
            act.Should().Throw<Exception>().WithMessage($"Item with ID {itemId} not found.");
        }

        [Fact]
        public void RecommendItemPlacement_WarehouseNotFound_ThrowsException()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var warehouseId = 1;
            SetupWarehouseRepositoryMock(warehouseId, null);

            // Act
            Action act = () => _inventoryService.RecommendItemPlacement(1, warehouseId);

            // Assert
            act.Should().Throw<Exception>().WithMessage($"Warehouse with ID {warehouseId} not found.");
        }

        [Fact]
        public void RecommendItemPlacement_NoRacksFound_ThrowsException()
        {
            // Arrange
            var itemId = 1;
            var inventoryItem = CreateTestInventoryItem(itemId);
            SetupInventoryItemRepositoryMock(itemId, inventoryItem);

            var warehouseId = 1;
            SetupWarehouseRepositoryMock(warehouseId, new Warehouse());
            SetupRackRepositoryMock(new Rack[0]);

            // Act
            Action act = () => _inventoryService.RecommendItemPlacement(1, warehouseId);

            // Assert
            act.Should().Throw<Exception>().WithMessage($"No racks found for warehouse ID {warehouseId}.");
        }

        // Add more tests to cover other scenarios
    }
}
