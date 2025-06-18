using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class RegistroMantenimientoVM
    {
        [ValidateNever]
        public RegistroMantenimiento RegistroMantenimiento { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaVehiculos { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaCatalogoMantenimiento { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaOperadores { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ListaRepuestos { get; set; }
    }
}
