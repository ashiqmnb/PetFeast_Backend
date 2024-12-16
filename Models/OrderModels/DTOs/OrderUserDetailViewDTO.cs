﻿using PetFeast_Backend2.Models.CartModels.DTOs;

namespace PetFeast_Backend2.Models.OrderModels.DTOs
{
    public class OrderUserDetailViewDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderId { get; set; }
        public string? OrderStatus { get; set; }

        //public string? CustomerName { get; set; }
        //public string? CustomerEmail { get; set; }
        //public string? CustomerCity { get; set; }
        //public string? HomeAddress { get; set; }
        //public string? CustomerPhone { get; set; }
        public string? TransactionId { get; set; }
        public List<CartResDTO>? OrderProducts { get; set; }
    }
}