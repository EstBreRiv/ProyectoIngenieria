using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class VehiculoVM
    {
        [ValidateNever]
        public Vehiculo Vehiculo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> VehiculosList { get; set; }
    }
}
