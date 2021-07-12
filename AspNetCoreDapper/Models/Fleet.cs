using System;
using System.ComponentModel.DataAnnotations;
using AspNetCoreDapper.CustomValidation;

namespace AspNetCoreDapper.Models
{
    public class Fleet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Código")]
        public string Code { get; set; }
        
        public bool IsEnabled { get; set; }

        public bool Current { get; set; }       
    }
}