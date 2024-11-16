﻿using WebShopSportskeOpreme.Interfaces;

namespace WebShopSportskeOpreme.Models
{
    public class Product : ISoftDeletableEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
