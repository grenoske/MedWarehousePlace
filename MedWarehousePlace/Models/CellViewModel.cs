using BLL.DTO;

namespace PL.Models
{
    public class CellViewModel
    {
        public int Id { get; set; }
        public int? InventoryItemId { get; set; }
        public string LabeledText { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAisle { get; set; }
        public bool IsRack { get; set; }
        public bool IsNotEmpty { get; set; }

        public static explicit operator CellViewModel(CellDTO cellDto)
        {
            return new CellViewModel
            {
                Id = cellDto.Id,
                LabeledText = cellDto.LabeledText,
                InventoryItemId = cellDto.InventoryItemId,
                IsAisle = cellDto.IsAisle,
                IsRack = cellDto.IsRack,
                IsNotEmpty = cellDto.IsNotEmpty
            };
        }
    }
}
