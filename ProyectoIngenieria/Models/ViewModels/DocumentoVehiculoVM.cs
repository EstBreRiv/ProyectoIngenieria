using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class DocumentoVehiculoVM
    {
        [ValidateNever]
        public DocumentoVehiculo DocumentoVehiculo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> DocumentoVehiculoList { get; set; }
    }
}
