﻿using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateModifierExtraDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;

    }
}