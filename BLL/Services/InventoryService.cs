using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure.AdditionalFunction;
using BLL.Infrastructure.SD;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ItemDTO> GetItems(int page = 1)
        {
            var items = _unitOfWork.Items.GetAll(page: page, includeProperties: "Category");
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public ItemDTO GetItemById(int id)
        {
            var item = _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id, includeProperties: "Category");
            return _mapper.Map<ItemDTO>(item);
        }

        public void CreateItem(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            _unitOfWork.Items.Add(item);
            _unitOfWork.Save();
        }

        public void UpdateItem(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            _unitOfWork.Items.Update(item);
            _unitOfWork.Save();
        }

        public void DeleteItem(int id)
        {
            var item = _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _unitOfWork.Items.Remove(item);
                _unitOfWork.Save();
            }
        }

        // Category methods
        public IEnumerable<CategoryDTO> GetCategories(int page = 1)
        {
            var categories = _unitOfWork.Categories.GetAll(page: page);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public CategoryDTO GetCategoryById(int id)
        {
            var category = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public void CreateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Categories.Add(category);
            _unitOfWork.Save();
        }

        public void UpdateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Categories.Update(category);
            _unitOfWork.Save();
        }

        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Categories.Remove(category);
                _unitOfWork.Save();
            }
        }

        // InventoryItem methods
        public IEnumerable<InventoryItemDTO> GetInventoryItems(string status = null)
        {
            IEnumerable<InventoryItem> inventoryItems;
            if (string.IsNullOrEmpty(status))
            {
                inventoryItems = _unitOfWork.InventoryItems.GetAll(includeProperties:"Item");
            }
            else
            {
                inventoryItems = _unitOfWork.InventoryItems.GetAll(i => i.Status == status, includeProperties: "Item");
            }

            return _mapper.Map<IEnumerable<InventoryItemDTO>>(inventoryItems);
        }

        public InventoryItemDTO GetInventoryItemById(int id)
        {
            var inventoryItem = _unitOfWork.InventoryItems.GetFirstOrDefault(ii => ii.Id == id, includeProperties: "Item");
            return _mapper.Map<InventoryItemDTO>(inventoryItem);
        }

        public void CreateInventoryItem(InventoryItemDTO inventoryItemDto)
        {
            var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemDto);
            _unitOfWork.InventoryItems.Add(inventoryItem);
            _unitOfWork.Save();
        }

        public void UpdateInventoryItem(InventoryItemDTO inventoryItemDto)
        {
            var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemDto);
            if (inventoryItemDto.BinId != null || inventoryItemDto.BinId != 0)
            {
                Bin bin = _unitOfWork.Bins.GetFirstOrDefault(bin => bin.Id == inventoryItemDto.BinId);
                Shelf shelf = _unitOfWork.Shelves.GetFirstOrDefault(shelf => shelf.Id == bin.ShelfId);
                Rack rack = _unitOfWork.Racks.GetFirstOrDefault(rack => rack.Id == shelf.RackId);
                inventoryItem.LocationDest = bin.Number + "-" + shelf.Number + "-" + rack.Number;
                inventoryItem.Location = SD.LocationTransferring;
                inventoryItem.Status = SD.StatusTransferring;
            }
            _unitOfWork.InventoryItems.Update(inventoryItem);
            _unitOfWork.Save();
        }

        public void DeleteInventoryItem(int id)
        {
            var inventoryItem = _unitOfWork.InventoryItems.GetFirstOrDefault(ii => ii.Id == id);
            if (inventoryItem != null)
            {
                _unitOfWork.InventoryItems.Remove(inventoryItem);
                _unitOfWork.Save();
            }
        }

        public void PlaceInventoryItemToWarehouse(int id)
        {
            var inventoryItem = _unitOfWork.InventoryItems.GetFirstOrDefault(ii => ii.Id == id);
            if (inventoryItem != null)
            {
                inventoryItem.Status = SD.StatusInventory;
                inventoryItem.Location = inventoryItem.LocationDest;
                inventoryItem.LocationDest = null;
                _unitOfWork.InventoryItems.Update(inventoryItem);
                _unitOfWork.Save();
            }
        }

        public InventoryItemDTO RecommendItemPlacement(int itemId, int warehouseId)
        {
            var inventoryItem = _unitOfWork.InventoryItems.GetFirstOrDefault(ii => ii.Id == itemId, includeProperties: "Item");
            if (inventoryItem == null)
            {
                throw new ValidationException($"Item with ID {itemId} not found.", "");
            }

            var warehouse = _unitOfWork.Warehouses.GetFirstOrDefault(w => w.Id == warehouseId);
            if (warehouse == null)
            {
                throw new Exception($"Warehouse with ID {warehouseId} not found.");
            }

            var racks = _unitOfWork.Racks.GetAll(
                r => r.WarehouseId == warehouseId,
                includeProperties: "Shelves.Bins.Cell"
            ).ToList();
            if (!racks.Any())
            {
                throw new Exception($"No racks found for warehouse ID {warehouseId}.");
            }

            Bin recommendBin = null;

            if (inventoryItem.Quantity * inventoryItem.Item.Weight > warehouse.MaximumWeightOnUpperShelves)
            {
                // Виключити верхні полиці, залишивши тільки полиці першого поверху
                foreach (var rack in racks)
                {
                    rack.Shelves = rack.Shelves.Where(s => s.Number == 1).ToList();
                }
            }

            // Отримати всі Bins із всіх Shelves, які є доступними
            var availableBins = racks.SelectMany(r => r.Shelves)
                                     .SelectMany(s => s.Bins)
                                     .Where(b => b.InventoryItemId == null)
                                     .ToList();
            if (!availableBins.Any())
            {
                throw new Exception("No available bins found.");
            }

            // Відсортувати Bins на основі близькості до PackingArea
            var sortedBins = availableBins.OrderBy(bin => CalculateDistance(bin.Cell, SD.LocationPacking, warehouse.Width)).ToList();

            if ((inventoryItem.ExpiryDate - DateTime.Now).TotalDays < 30)
            {
                // Якщо термін придатності менше 30 днів, вибрати перший Bin
                recommendBin = sortedBins.FirstOrDefault();
            }
            else if (inventoryItem.Item.TurnoverRate > SD.HighTurnOverRatePerDay)
            {
                // Якщо високий показник оборотності, вибрати середній Bin
                int middleIndex = sortedBins.Count / 2;
                recommendBin = sortedBins.ElementAtOrDefault(middleIndex);
            }
            else
            {
                // Якщо жодна з вищевказаних умов не виконана, вибрати дальній Bin
                recommendBin = sortedBins.LastOrDefault();
            }

            if (recommendBin == null)
            {
                throw new Exception("No suitable bin found based on the given conditions.");
            }

            // Використати метод FindLocation для отримання розташування
            string locationDest = FindBinLocation(racks, recommendBin);

            InventoryItemDTO inventoryItemDTO = new InventoryItemDTO
            {
                BinId = recommendBin.Id,
                LocationDest = locationDest
            };

            return inventoryItemDTO;
        }

        private double CalculateDistance(Cell cell, string packingArea, int width)
        {
            // Обчислити відстань між коміркою та зоною пакування, використовуючи координати
            var cellCoord = AdditionalFunction.ToCoord(cell, width);
            var packingCoord = (cellCoord.Row, Column: 0);
            double dx = cellCoord.Row - packingCoord.Row; 
            double dy = cellCoord.Column - packingCoord.Column;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private string FindBinLocation(List<Rack> racks, Bin recommendBin)
        {
            foreach (var rack in racks)
            {
                foreach (var shelf in rack.Shelves)
                {
                    if (shelf.Bins.Contains(recommendBin))
                    {
                        return $"{rack.Number}-{shelf.Number}-{recommendBin.Number}";
                    }
                }
            }
            throw new Exception("Unable to find the location for the recommended bin.");
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
