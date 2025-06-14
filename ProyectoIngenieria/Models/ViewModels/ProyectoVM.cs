using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class ProyectoVM
    {
        [ValidateNever]
        public Proyecto Proyecto { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ProyectoList { get; set; }
    }
}
