﻿using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class Variant
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public int ProductId { get; set; }
    }
}
