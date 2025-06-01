using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroMantenimientoVM
    {
        [ValidateNever]
        public RegistroMantenimiento RegistroMantenimiento { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> RegistroMantenmientoList { get; set; }
    }
}
