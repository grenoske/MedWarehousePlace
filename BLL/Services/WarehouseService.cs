using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using AutoMapper;
using System.Security.AccessControl;

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
            _unitOfWork.Racks.Add(rack);
            _unitOfWork.Save();
        }

        public void UpdateWarehouse(WarehouseDTO warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            _unitOfWork.Warehouses.Update(warehouse);
            _unitOfWork.Save();
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

    }
}
