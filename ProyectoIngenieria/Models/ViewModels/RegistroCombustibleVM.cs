using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroCombustibleVM
    {
        [ValidateNever]
        public RegistroCombustible RegistroCombustible { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RegistroCombustibleList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaVehiculos { get; set; }
    }
}
