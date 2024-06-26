﻿using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Infrastructure.MappingProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<InventoryItem, InventoryItemDTO>().ReverseMap();
        }
    }
}
