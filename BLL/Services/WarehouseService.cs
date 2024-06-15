using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<WarehouseDTO> GetAllWarehouses()
        {
            var warehouses = _unitOfWork.Warehouses.GetAll();
            return _mapper.Map<IEnumerable<WarehouseDTO>>(warehouses);
        }

        public WarehouseDTO GetWarehouseById(int id)
        {
            var warehouse = _unitOfWork.Warehouses.GetFirstOrDefault(w => w.Id == id);
            var warehouseDTO = _mapper.Map<WarehouseDTO>(warehouse);
            warehouseDTO.Cells = FillCells(warehouse);
            return warehouseDTO;
        }

        public WarehouseDTO GetWarehouseLevelById(int id, int level)
        {
            var warehouse = _unitOfWork.Warehouses.GetFirstOrDefault(w => w.Id == id);
            var warehouseDTO = _mapper.Map<WarehouseDTO>(warehouse);
            warehouseDTO.Cells = FillCellsDetail(warehouse, level);
            return warehouseDTO;
        }

        public void CreateWarehouse(WarehouseDTO warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            _unitOfWork.Warehouses.Add(warehouse);
            _unitOfWork.Save();
        }

        public void CreateAisle(AisleDTO aisleDto)
        {
            var aisle = _mapper.Map<Aisle>(aisleDto);
            foreach (var cell in aisle.Cells)
            {
                cell.Number = cell.Id;

                cell.Id = 0;
            }
            _unitOfWork.Aisles.Add(aisle); 
            _unitOfWork.Save();
        }
        public void CreateRack(RackDTO rackDto)
        {
            var rack = _mapper.Map<Rack>(rackDto);
            rack.AisleId = null;
            foreach (var cell in rack.Cells)
            {
                cell.Number = cell.Id;

                cell.Id = 0; 
            }
            var wh = _unitOfWork.Warehouses.GetFirstOrDefault(w => w.Id == rack.WarehouseId);
            var shelves = CreateShelves(wh.Height);
            int numberOfBinsPerShelf = rack.Cells.Count();
            var cellsList = rack.Cells.ToList();
            
            foreach (var shelf in shelves)
            {
                var bins = CreateBins(numberOfBinsPerShelf).ToList();
                for (int i = 0; i < numberOfBinsPerShelf; i++)
                {
                    bins[i].Cell = cellsList[i];
                }
                shelf.Bins = bins;
            }
            rack.Shelves = shelves;
            _unitOfWork.Racks.Add(rack);
            _unitOfWork.Save();
        }


        public void UpdateWarehouse(WarehouseDTO warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            _unitOfWork.Warehouses.Update(warehouse);
            _unitOfWork.Save();
        }

        private List<Shelf> CreateShelves(int number)
        {
            var shelves = new List<Shelf>();
            for (int i = 0; i < number; i++)
            {
                shelves.Add(new Shelf() { Number = i + 1});
            }

            return shelves;
        }

        private List<Bin> CreateBins(int number)
        {
            var bins = new List<Bin>();
            for (int i = 0; i < number; i++)
            {
                bins.Add(new Bin() { Number = i + 1 });
            }

            return bins;
        }

        public IEnumerable<RackDTO> GetRacks(int warehouseId = 1)
        {
            var racks = _unitOfWork.Racks.GetAll(wh => wh.WarehouseId == warehouseId);
            return _mapper.Map<IEnumerable<RackDTO>>(racks);
        }

        public IEnumerable<ShelfDTO> GetShelves(int rackId)
        {
            var shelves = _unitOfWork.Shelves.GetAll(s => s.RackId == rackId);
            return _mapper.Map<IEnumerable<ShelfDTO>>(shelves);
        }

        public IEnumerable<BinDTO> GetBins(int shelfId, bool empty = true)
        {
            IEnumerable<Bin> bins;

            if (empty)
            {
                bins = _unitOfWork.Bins.GetAll(b => b.ShelfId == shelfId && b.InventoryItem == null);
            }
            else
            {
                bins = _unitOfWork.Bins.GetAll(b => b.ShelfId == shelfId);
            }
            return _mapper.Map<IEnumerable<BinDTO>>(bins);
        }

        private List<CellDTO> FillCells(Warehouse wh)
        {
            var cells = new List<CellDTO>();
            var aisles = _unitOfWork.Aisles.GetAll(w => w.WarehouseId == wh.Id, includeProperties:"Cells").ToList();
            var racks = _unitOfWork.Racks.GetAll(w => w.WarehouseId == wh.Id, includeProperties:"Cells").ToList();

            for (int i = 0; i < wh.Width * wh.Length; i++)
            {
                var cell = new CellDTO { Id = i + 1 };

                // Перевірка чи є ця комірка в Aisles
                foreach (var aisle in aisles)
                {
                    if (aisle.Cells.Any(c => c.Number == cell.Id))
                    {
                        cell.IsAisle = true;
                        break;
                    }
                }

                // Перевірка чи є ця комірка в Racks
                foreach (var rack in racks)
                {
                    if (rack.Cells.Any(c => c.Number == cell.Id))
                    {
                        cell.IsRack = true;
                        break;
                    }
                }

                cells.Add(cell);
            }

            return cells;
        }

        private List<CellDTO> FillCellsDetail(Warehouse wh, int level)
        {
            var cells = new List<CellDTO>();
            var aisles = _unitOfWork.Aisles.GetAll(w => w.WarehouseId == wh.Id, includeProperties: "Cells").ToList();
            var racks = _unitOfWork.Racks.GetAll(w => w.WarehouseId == wh.Id, includeProperties: "Shelves.Bins.Cell,Cells").ToList();

            foreach (var rack in racks)
            {
                rack.Shelves = rack.Shelves.Where(s => s.Number == level).ToList();
            }
            // Отримати всі Cells у яких Bins з речами
            var Bins = racks.SelectMany(r => r.Shelves)
                                     .SelectMany(s => s.Bins)
                                     .ToList();
            foreach (var rack in racks)
            {
                rack.Shelves = rack.Shelves.Where(s => s.Number == level).ToList();
            }

            for (int i = 0; i < wh.Width * wh.Length; i++)
            {
                var cell = new CellDTO { Id = i + 1 };

                // Перевірка чи є ця комірка в Aisles
                foreach (var aisle in aisles)
                {
                    if (aisle.Cells.Any(c => c.Number == cell.Id))
                    {
                        cell.IsAisle = true;
                        break;
                    }
                }

                // Перевірка чи є ця комірка в Racks
                if(Bins.Any(c => c.Cell.Number == cell.Id))
                {
                    cell.IsRack = true;
                    var Bin = Bins.FirstOrDefault(c => c.Cell.Number == cell.Id);
                    cell.LabeledText = Bin.Number.ToString();
                    if (Bin.InventoryItemId != null)
                    {
                        cell.IsNotEmpty = true;
                        cell.InventoryItemId = Bin.InventoryItemId;
                    }
                }

                cells.Add(cell);
            }
            return cells;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
