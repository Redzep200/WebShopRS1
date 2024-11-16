﻿using System.Text.Json.Serialization;

namespace WebShopSportskeOpreme.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
