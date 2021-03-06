using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AspNetCoreDapper.Repositories;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreDapper.CustomValidation
{
    public class UniqueChassiOnly : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string idValue = null;
            var containerType = validationContext.ObjectInstance.GetType();
            var propertyId = containerType.GetProperty("Id");

            if(propertyId != null){
                idValue = propertyId.GetValue(validationContext.ObjectInstance, null)?.ToString();
            }

            if(!string.IsNullOrWhiteSpace(value?.ToString())){
                var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
                var vehicleRepository = new VehicleRepository(configuration);
                var vehicleFound = vehicleRepository.FindByChassi(value.ToString());

                if ((string.IsNullOrWhiteSpace(idValue) || idValue == "0") && !string.IsNullOrWhiteSpace(vehicleFound?.Chassi))
                    return new ValidationResult("Este número do chassi já foi cadastrado.", new[] { validationContext.MemberName });

                if (!string.IsNullOrWhiteSpace(idValue) && idValue != "0" && !string.IsNullOrWhiteSpace(vehicleFound?.Chassi) && vehicleFound?.Id.ToString() != idValue)
                    return new ValidationResult("Este número do chassi já foi cadastrado.", new[] { validationContext.MemberName });
            }
            
            return null;
        }
    }
}