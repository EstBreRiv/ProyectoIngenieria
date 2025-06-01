using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class CatalogoMantenimientoVM
    {
        [ValidateNever]
        public CatalogoMantenimiento CatalogoMantenimiento { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> CatalogoMantenimientoList { get; set; }
    }
}
