﻿using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class NotRegisteredOrder
    {
        [Required]
        [CustomPhone]
        public string Phone { get; set; }
        [Required]
        public string OrdersStringJSON { get; set; }
    }
}