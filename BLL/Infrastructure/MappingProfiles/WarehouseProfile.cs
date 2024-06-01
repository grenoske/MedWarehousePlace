using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Infrastructure.MappingProfiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
            CreateMap<Cell, CellDTO>().ReverseMap();
            CreateMap<Aisle, AisleDTO>().ReverseMap();
            CreateMap<Rack, RackDTO>().ReverseMap();
            CreateMap<Shelf, ShelfDTO>().ReverseMap();
            CreateMap<Bin, BinDTO>().ReverseMap();
        }
    }
}