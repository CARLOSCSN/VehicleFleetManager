using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDapper.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}