using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class EmpresaVM
    {
        [ValidateNever]
        public Empresa Empresa { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> EmpresasList { get; set; }
    }
}
