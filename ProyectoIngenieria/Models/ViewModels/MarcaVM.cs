using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class MarcaVM
    {
        [ValidateNever]
        public Marca marca { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MarcasList { get; set; }
    }
}
