using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class HorasTrabajoVM
    {
        [ValidateNever]
        public HorasTrabajo HorasTrabajo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> HorasTrabajoList { get; set; }
    }
}
