using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class TipoVehiculoVM
    {
        [ValidateNever]
        public TipoVehiculo tipoVehiculo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> TiposVehiculoList { get; set; }
    }
}
