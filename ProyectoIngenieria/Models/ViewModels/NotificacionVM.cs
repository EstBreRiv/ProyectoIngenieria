using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoIngenieria.Models.ViewModels
{
    public class NotificacionVM
    {
        [ValidateNever]
        public Notificacion Notificacion { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> NotificacionesList { get; set; }
    }
}
