using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroOperadoresVM
    {
        [ValidateNever]
        public RegistroOperadores RegistroOperador { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> RegistroOperadoresList { get; set; }

        [ValidateNever]
        public int VehiculoId { get; set; }
    }
}
