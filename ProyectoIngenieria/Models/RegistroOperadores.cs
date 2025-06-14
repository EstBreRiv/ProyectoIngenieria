using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoIngenieria.Models;

public partial class RegistroOperadores
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
    public DateOnly Fecha { get; set; }

    [Required(ErrorMessage = "Se debe elegir un operador.")]
    public int OperadorCedula { get; set; }

    public virtual Operador Operador { get; set; } = null!;

    [Required(ErrorMessage = "Se debe elegir un vehículo para operar.")]
    public int VehiculoId { get; set; }

    public virtual Vehiculo Vehiculo { get; set; } = null!;
}
