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

        [ValidateNever]
        public List<int> RepuestosSeleccionados { get; set; }
        
        [ValidateNever]
        public ICollection<OperadorMantenimiento> DetallesOperadores { get; set; }

        [ValidateNever]
        public string nombreVehiculo { get; set; }

        [ValidateNever]
        public string nombreOperador { get; set; }

        [ValidateNever]
        public string tipoMantenimiento { get; set; }

        [ValidateNever]
        public List<String> nombresProductos { get; set; } = new List<String>();

        [ValidateNever]
        public int vehiculoId { get; set; }

        [ValidateNever]
        public virtual Vehiculo vehiculo { get; set; }
    }
}
