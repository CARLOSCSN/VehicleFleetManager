using System;
using System.ComponentModel.DataAnnotations;
using AspNetCoreDapper.CustomValidation;

namespace AspNetCoreDapper.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Chassi")]
        [UniqueChassiOnly]
        public string Chassi { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo Veículo")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Número Passageiros")]
        public int NumberPassengers { get; set; }
        
        [Display(Name = "Cor")]
        public string Color { get; set; }
        public bool IsEnabled { get; set; }

        [Display(Name = "Frota")]
        public string CodeFleet { get; set; }         

        // Extra
        public VehicleType VehicleType { get; set; }
    }
}