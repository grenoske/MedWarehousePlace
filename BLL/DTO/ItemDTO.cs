﻿namespace BLL.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int TurnoverRate { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
