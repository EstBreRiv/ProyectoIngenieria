using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class DocumentoVehiculoVM
    {
        [ValidateNever]
        public DocumentoVehiculo DocumentoVehiculo { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> DocumentoVehiculoList { get; set; }

        [NotMapped]
        public IFormFile Archivo { get; set; }
    }
}
