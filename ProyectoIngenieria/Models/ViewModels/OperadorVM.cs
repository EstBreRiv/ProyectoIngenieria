using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class OperadorVM
    {
        [ValidateNever]
        public Operador Operador { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> OperadoresList { get; set; }

        [ValidateNever]
        public int VehiculoId { get; set; }
    }
}
