using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class DocumentoOperadorVM
    {
        [ValidateNever]
        public DocumentoOperador DocumentoOperador { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> DocumentoOperadorList { get; set; }

        [NotMapped]
        public IFormFile Archivo { get; set; }
    }
}
