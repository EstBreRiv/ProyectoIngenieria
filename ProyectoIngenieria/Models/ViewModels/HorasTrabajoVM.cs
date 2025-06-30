using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoIngenieria.Models;
using System.Collections.Generic;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class HorasTrabajoVM
    {
        public HorasTrabajo HorasTrabajo { get; set; } = new();

        [ValidateNever]
        public IEnumerable<SelectListItem> LugarTrabajoList { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> TipoTrabajoList { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> ProyectoList { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> VehiculoList { get; set; } = new List<SelectListItem>();

    }
}
