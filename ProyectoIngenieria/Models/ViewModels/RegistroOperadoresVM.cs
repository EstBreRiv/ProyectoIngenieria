using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroOperadoresVM
    {
        [ValidateNever]
        public RegistroOperadores RegistroOperadores { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> RegistroOperadoresList { get; set; }
    }
}
