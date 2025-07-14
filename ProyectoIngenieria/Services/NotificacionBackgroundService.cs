using ProyectoIngenieria.Repository.Interfaces;
using ProyectoIngenieria.Models;
using System.Globalization;

public class NotificacionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificacionBackgroundService> _logger;

    public NotificacionBackgroundService(IServiceProvider serviceProvider, ILogger<NotificacionBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando tarea de revisión de notificaciones...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
                var vehiculos = unitOfWork.Vehiculo.GetAll(includeProperties: "TipoVehiculo").ToList();

                //BLOQUE DE NOTIFICACION DE INSPECCION VEHICULAR
                foreach (var vehiculo in vehiculos)
                {
                    if (string.IsNullOrEmpty(vehiculo.Placa))
                        continue;

                    char ultimoDigito = vehiculo.Placa.LastOrDefault();
                    if (!char.IsDigit(ultimoDigito))
                        continue;

                    int mesRevision = ultimoDigito == '0' ? 10 : int.Parse(ultimoDigito.ToString());

                    // Generar solo si HOY es el primer día del mes de revisión
                    if (hoy.Month != mesRevision || hoy.Day != 1)
                        continue;

                    // Validar que no exista ya una notificación generada ese día
                    bool yaExiste = unitOfWork.Notificacion.GetAll()
                        .Any(n =>
                            n.VehiculoId == vehiculo.Id &&
                            n.Titulo.StartsWith("Inspección vehicular") &&
                            n.Fecha == hoy
                        );

                    if (!yaExiste)
                    {
                        var notificacion = new Notificacion
                        {
                            Titulo = "Inspección vehicular próxima",
                            Descripcion = $"El vehículo con placa {vehiculo.Placa} debe realizar su inspección técnica este mes ({hoy.ToString("MMMM yyyy", new CultureInfo("es-ES"))}).",
                            Fecha = hoy, // fecha en la que se genera
                            VehiculoId = vehiculo.Id,
                            Leida = false
                        };

                        unitOfWork.Notificacion.Add(notificacion);
                    }
                }

                unitOfWork.Save();

                //BLOQUE DE NOTIFICACION DE CAMBIO DE ACEITE
                foreach (var vehiculo in vehiculos)
                {
                    //Solo aplicar lógica de cambio de aceite si es tipo "Maquinaria"
                    if (vehiculo.TipoVehiculo.Tipo != "Maquinaria")
                        continue;

                    var registrosMantenimiento = unitOfWork.RegistroMantenimiento
                        .GetAll(r => r.VehiculoId == vehiculo.Id, includeProperties: "OperadorMantenimientos.CatalogoMantenimiento")
                        .ToList();

                    var cambiosDeAceite = registrosMantenimiento
                        .Where(r => r.OperadorMantenimientos
                            .Any(om => om.CatalogoMantenimiento.Nombre == "Cambio de aceite"))
                        .OrderByDescending(r => r.Fecha)
                        .ToList();

                    var ultimoCambio = cambiosDeAceite.FirstOrDefault();

                    if (ultimoCambio != null)
                    {
                        var proximoCambio = ultimoCambio.Fecha.AddMonths(3);

                        //SOLO SI HOY es exactamente la fecha esperada del próximo cambio
                        if (hoy == proximoCambio)
                        {
                            if (!YaExisteNotificacionCambioAceite(vehiculo.Id, proximoCambio, unitOfWork))
                            {
                                CrearNotificacionCambioAceite(vehiculo.Id, vehiculo.Placa, hoy, unitOfWork);
                            }
                        }
                    }
                    else
                    {
                        // Nunca se ha hecho cambio de aceite
                        // Verifica si ya se generó una notificación relacionada
                        var yaExiste = unitOfWork.Notificacion.GetAll()
                            .Any(n =>
                                n.VehiculoId == vehiculo.Id &&
                                n.Titulo == "Cambio de aceite" &&
                                n.Descripcion.Contains("no tiene ningún cambio de aceite registrado"));

                        if (!yaExiste)
                        {
                            var noti = new Notificacion
                            {
                                Titulo = "Cambio de aceite",
                                Descripcion = $"El vehículo con placa/serie {vehiculo.Placa} no tiene ningún cambio de aceite registrado. Es necesario registrar el último cuanto antes.",
                                Fecha = hoy,
                                VehiculoId = vehiculo.Id,
                                Leida = false
                            };

                            unitOfWork.Notificacion.Add(noti);
                        }
                    }
                }

                unitOfWork.Save(); // Guardar las notificaciones de aceite también

            }

            _logger.LogInformation("Tarea completada.");

            // Esperar 24 horas hasta la siguiente ejecución
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    //Método que permite buscar si ya se generó una notificacion para ese cambio de aceite
    private bool YaExisteNotificacionCambioAceite(int vehiculoId, DateOnly fechaEsperada, IUnitOfWork unitOfWork)
    {
        return unitOfWork.Notificacion.GetAll()
            .Any(n =>
                n.VehiculoId == vehiculoId &&
                n.Titulo == "Cambio de aceite" &&
                n.Fecha == fechaEsperada
            );
    }

    //Se utiliza en caso de necesitar generar una notificacion de cambio de aceite
    private void CrearNotificacionCambioAceite(int vehiculoId, string placa, DateOnly hoy, IUnitOfWork unitOfWork)
    {
        var noti = new Notificacion
        {
            Titulo = "Cambio de aceite",
            Descripcion = $"El vehículo con placa/serie {placa} requiere cambio de aceite. Han pasado más de 3 meses desde el último mantenimiento.",
            Fecha = hoy,
            VehiculoId = vehiculoId,
            Leida = false
        };

        unitOfWork.Notificacion.Add(noti);
    }

}