using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class TipoTrabajoVM
    {
        [ValidateNever]
        public TipoTrabajo tipoTrabajo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> TiposTrabajoList { get; set; }
    }
}
