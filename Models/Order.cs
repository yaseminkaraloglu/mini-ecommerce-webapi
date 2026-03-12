using System;
using System.Collections.Generic;


namespace MyE_CommerceWebAPI.Models
{
    public class Order
    {
        public int Id { get; set; }


        public int UserId { get; set; }


        public string Status { get; set; } = "Pending";


        public decimal TotalAmount { get; set; }


        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new();
    }
}