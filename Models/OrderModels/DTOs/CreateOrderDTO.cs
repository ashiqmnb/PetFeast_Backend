﻿using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.OrderModels.DTOs
{
    public class CreateOrderDTO
    {
        //public string? CustomerName { get; set; }
        //public string? CustomerEmail { get; set; }
        //public string? CustomerPhone { get; set; }
        //public string? CustomerCity { get; set; }
        //public string? HomeAddress { get; set; }
        public int AddressId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderString { get; set; }
        public string? TransactionId { get; set; }
    }
}
