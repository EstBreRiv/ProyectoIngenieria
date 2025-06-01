using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class DocumentoOperadorVM
    {
        [ValidateNever]
        public DocumentoOperador DocumentoOperador { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> DocumentoOperadorList { get; set; }
    }
}
