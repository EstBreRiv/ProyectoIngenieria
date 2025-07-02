using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroOperadoresVM
    {
        [ValidateNever]
        public RegistroOperadore RegistroOperador { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> RegistroOperadores { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaVehiculos { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaOperadores { get; set; }
    }
}
