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
                var vehiculos = unitOfWork.Vehiculo.GetAll().ToList();

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
                            Descripcion = $"El vehículo con placa {vehiculo.Placa} debe realizar su inspección técnica este mes ({hoy:MMMM yyyy}).",
                            Fecha = hoy, // fecha en la que se genera
                            VehiculoId = vehiculo.Id,
                            Leida = false
                        };

                        unitOfWork.Notificacion.Add(notificacion);
                    }
                }

                unitOfWork.Save();

            }

            _logger.LogInformation("Tarea completada.");

            // Esperar 24 horas hasta la siguiente ejecución
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}