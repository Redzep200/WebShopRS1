﻿namespace WebShopSportskeOpreme.Models
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public string Name { get; set; }
        public string ZipCode { get; set; }
    }
}
