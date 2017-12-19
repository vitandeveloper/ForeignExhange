﻿
namespace ForeignExhange.Models
{
    using SQLite;

    public class Rate
    {
        [PrimaryKey] //Convierte en clave primaria RadeId  para poder meter este modelo en una base de datos
        public int RateId { get; set; }
        public string Code { get; set; }
        public double TaxRate { get; set; }
        public string Name { get; set; }
    }
}
