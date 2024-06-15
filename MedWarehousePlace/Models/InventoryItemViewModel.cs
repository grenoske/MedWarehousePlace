using BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PL.Models
{
    public class InventoryItemViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemViewModel? Item { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Container { get; set; }
        public string Location { get; set; }
        public string? LocationDest {  get; set; }
        public string Status { get; set; }
        public IEnumerable<SelectListItem>? ItemList { get; set; }

        public static explicit operator InventoryItemViewModel(InventoryItemDTO itemDto)
        {
            if (itemDto == null) return null;
            var iitemViewModel = new InventoryItemViewModel
            {
                Id = itemDto.Id,
                ItemId = itemDto.ItemId,
                Quantity = itemDto.Quantity,
                ExpiryDate = itemDto.ExpiryDate,
                Container = itemDto.Container,
                Location = itemDto.Location,
                Status = itemDto.Status,
                LocationDest = itemDto.LocationDest
            };
            if (itemDto.Item != null) iitemViewModel.Item = (ItemViewModel)itemDto.Item;

            return iitemViewModel;
        }

        public static explicit operator InventoryItemDTO(InventoryItemViewModel itemViewModel)
        {
            if (itemViewModel == null) return null;
            var iitemDto = new InventoryItemDTO
            {
                Id = itemViewModel.Id,
                ItemId = itemViewModel.ItemId,
                Quantity = itemViewModel.Quantity,
                ExpiryDate = itemViewModel.ExpiryDate,
                Container = itemViewModel.Container,
                Location = itemViewModel.Location,
                Status = itemViewModel.Status
            };
            if (itemViewModel.Item != null) iitemDto.Item = (ItemDTO)itemViewModel.Item;

            return iitemDto;
        }
    }
}
