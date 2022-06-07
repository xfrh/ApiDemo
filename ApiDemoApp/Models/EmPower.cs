﻿using System.ComponentModel.DataAnnotations;

namespace ApiDemoApp.Models
{
    public class EmPower
    {
        [Required]
        public string? ip { get; set; }
        [Required]
        public Boolean lockStatus { get; set; }
    }
}
