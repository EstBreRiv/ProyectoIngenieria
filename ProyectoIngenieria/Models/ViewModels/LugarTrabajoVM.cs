using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class LugarTrabajoVM
    {
        [ValidateNever]
        public LugarTrabajo lugarTrabajo { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> LugaresTrabajoList { get; set; }
    }
}
