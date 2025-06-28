using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoIngenieria.Models;

public partial class ProyectoIngenieriaDbContext : DbContext
{
    public ProyectoIngenieriaDbContext()
    {
    }

    public ProyectoIngenieriaDbContext(DbContextOptions<ProyectoIngenieriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatalogoMantenimiento> CatalogoMantenimientos { get; set; }

    public virtual DbSet<CatalogoRepuesto> CatalogoRepuestos { get; set; }

    public virtual DbSet<DocumentoOperador> DocumentoOperadors { get; set; }

    public virtual DbSet<DocumentoVehiculo> DocumentoVehiculos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<HorasTrabajo> HorasTrabajos { get; set; }

    public virtual DbSet<LugarTrabajo> LugarTrabajos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<Operador> Operadors { get; set; }

    public virtual DbSet<OperadorMantenimiento> OperadorMantenimientos { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<RegistroCombustible> RegistroCombustibles { get; set; }

    public virtual DbSet<RegistroMantenimiento> RegistroMantenimientos { get; set; }

    public virtual DbSet<RegistroOperadore> RegistroOperadores { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<RepuestosMantenimiento> RepuestosMantenimientos { get; set; }

    public virtual DbSet<TipoTrabajo> TipoTrabajos { get; set; }

    public virtual DbSet<TipoVehiculo> TipoVehiculos { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ProyectoIngenieria;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogoMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CATALOGO_MANTENIMIENTO_pk");

            entity.ToTable("CATALOGO_MANTENIMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogoRepuesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CATALOGO_REPUESTO_pk");

            entity.ToTable("CATALOGO_REPUESTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrecioEstimado)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Estimado");
        });

        modelBuilder.Entity<DocumentoOperador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DOCUMENTO_OPERADOR_pk");

            entity.ToTable("DOCUMENTO_OPERADOR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.OperadorCedula).HasColumnName("OPERADOR_Cedula");
            entity.Property(e => e.Ruta).HasMaxLength(255);

            entity.HasOne(d => d.OperadorCedulaNavigation).WithMany(p => p.DocumentoOperadors)
                .HasForeignKey(d => d.OperadorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DOCUMENTO_OPERADOR_OPERADOR");
        });

        modelBuilder.Entity<DocumentoVehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DOCUMENTO_VEHICULO_pk");

            entity.ToTable("DOCUMENTO_VEHICULO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Ruta).HasMaxLength(255);
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.DocumentoVehiculos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DOCUMENTO_MAQUINA");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EMPRESA_pk");

            entity.ToTable("EMPRESA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<HorasTrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("HORAS_TRABAJO_pk");

            entity.ToTable("HORAS_TRABAJO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HorometroFinal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Horometro_Final");
            entity.Property(e => e.HorometroInicial)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Horometro_Inicial");
            entity.Property(e => e.LugarTrabajoId).HasColumnName("LUGAR_TRABAJO_ID");
            entity.Property(e => e.PrecioHora)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Hora");
            entity.Property(e => e.ProyectoId).HasColumnName("PROYECTO_ID");
            entity.Property(e => e.TipoTrabajoId).HasColumnName("TIPO_TRABAJO_ID");
            entity.Property(e => e.TotalGanancia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Ganancia");
            entity.Property(e => e.TotalHoras)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Horas");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.LugarTrabajo).WithMany(p => p.HorasTrabajos)
                .HasForeignKey(d => d.LugarTrabajoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORAS_TRABAJO_LUGAR_TRABAJO");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.HorasTrabajos)
                .HasForeignKey(d => d.ProyectoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORAS_TRABAJO_PROYECTO");

            entity.HasOne(d => d.TipoTrabajo).WithMany(p => p.HorasTrabajos)
                .HasForeignKey(d => d.TipoTrabajoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORAS_TRABAJO_TIPO_TRABAJO");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.HorasTrabajos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORAS_TRABAJO_MAQUINA");
        });

        modelBuilder.Entity<LugarTrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LUGAR_TRABAJO_pk");

            entity.ToTable("LUGAR_TRABAJO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Canton).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Provincia).HasMaxLength(50);
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MARCA_pk");

            entity.ToTable("MARCA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NombreMarca)
                .HasMaxLength(50)
                .HasColumnName("Nombre_Marca");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NOTIFICACION_pk");

            entity.ToTable("NOTIFICACION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Titulo).HasMaxLength(50);
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NOTIFICACION_MAQUINA");
        });

        modelBuilder.Entity<Operador>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("OPERADOR_pk");

            entity.ToTable("OPERADOR");

            entity.Property(e => e.Cedula).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(15);
            entity.Property(e => e.TipoColaborador)
                .HasMaxLength(39)
                .HasColumnName("Tipo_Colaborador");
        });

        modelBuilder.Entity<OperadorMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OPERADOR_MANTENIMIENTO_pk");

            entity.ToTable("OPERADOR_MANTENIMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CatalogoMantenimientoId).HasColumnName("CATALOGO_MANTENIMIENTO_ID");
            entity.Property(e => e.HorasTrabajo)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Horas_Trabajo");
            entity.Property(e => e.OperadorCedula).HasColumnName("OPERADOR_Cedula");
            entity.Property(e => e.RegistroMantenimientoId).HasColumnName("REGISTRO_MANTENIMIENTO_ID");

            entity.HasOne(d => d.CatalogoMantenimiento).WithMany(p => p.OperadorMantenimientos)
                .HasForeignKey(d => d.CatalogoMantenimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OPERADOR_MANTENIMIENTO_CATALOGO_MANTENIMIENTO");

            entity.HasOne(d => d.OperadorCedulaNavigation).WithMany(p => p.OperadorMantenimientos)
                .HasForeignKey(d => d.OperadorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OPERADOR_MANTENIMIENTO_OPERADOR");

            entity.HasOne(d => d.RegistroMantenimiento).WithMany(p => p.OperadorMantenimientos)
                .HasForeignKey(d => d.RegistroMantenimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OPERADOR_MANTENIMIENTO_REGISTRO_MANTENIMIENTO");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PROYECTO_pk");

            entity.ToTable("PROYECTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cliente).HasMaxLength(50);
            entity.Property(e => e.FechaFin).HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.NombreProyecto)
                .HasMaxLength(100)
                .HasColumnName("Nombre_Proyecto");
        });

        modelBuilder.Entity<RegistroCombustible>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("REGISTRO_COMBUSTIBLE_pk");

            entity.ToTable("REGISTRO_COMBUSTIBLE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaCompra).HasColumnName("Fecha_Compra");
            entity.Property(e => e.LitrosComprados)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Litros_Comprados");
            entity.Property(e => e.PrecioLitro)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Litro");
            entity.Property(e => e.TotalPagado)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Pagado");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.RegistroCombustibles)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_COMBUSTIBLE_MAQUINA");
        });

        modelBuilder.Entity<RegistroMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("REGISTRO_MANTENIMIENTO_pk");

            entity.ToTable("REGISTRO_MANTENIMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.RegistroMantenimientos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_MANTENIMIENTO_MAQUINA");
        });

        modelBuilder.Entity<RegistroOperadore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("REGISTRO_OPERADORES_pk");

            entity.ToTable("REGISTRO_OPERADORES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.OperadorCedula).HasColumnName("OPERADOR_Cedula");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.OperadorCedulaNavigation).WithMany(p => p.RegistroOperadores)
                .HasForeignKey(d => d.OperadorCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_OPERADORES_OPERADOR");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.RegistroOperadores)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_OPERADORES_VEHICULO");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("REPORTE_pk");

            entity.ToTable("REPORTE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaReporte).HasColumnName("Fecha_Reporte");
            entity.Property(e => e.GastoCombustible)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Gasto_Combustible");
            entity.Property(e => e.GastoMantenimiento)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Gasto_Mantenimiento");
            entity.Property(e => e.IngresoTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ingreso_Total");
            entity.Property(e => e.PeriodoFin).HasColumnName("Periodo_Fin");
            entity.Property(e => e.PeriodoInicio).HasColumnName("Periodo_Inicio");
            entity.Property(e => e.TotalHoras)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Horas");
            entity.Property(e => e.Utilidad).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REPORTE_VEHICULO");
        });

        modelBuilder.Entity<RepuestosMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("REPUESTOS_MANTENIMIENTO_pk");

            entity.ToTable("REPUESTOS_MANTENIMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CatalogoRepuestoId).HasColumnName("CATALOGO_REPUESTO_ID");
            entity.Property(e => e.RegistroMantenimientoId).HasColumnName("REGISTRO_MANTENIMIENTO_ID");

            entity.HasOne(d => d.CatalogoRepuesto).WithMany(p => p.RepuestosMantenimientos)
                .HasForeignKey(d => d.CatalogoRepuestoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REPUESTOS_MANTENIMIENTO_CATALOGO_REPUESTO");

            entity.HasOne(d => d.RegistroMantenimiento).WithMany(p => p.RepuestosMantenimientos)
                .HasForeignKey(d => d.RegistroMantenimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REPUESTOS_MANTENIMIENTO_REGISTRO_MANTENIMIENTO");
        });

        modelBuilder.Entity<TipoTrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TIPO_TRABAJO_pk");

            entity.ToTable("TIPO_TRABAJO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<TipoVehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TIPO_VEHICULO_pk");

            entity.ToTable("TIPO_VEHICULO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(50);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VEHICULO_pk");

            entity.ToTable("VEHICULO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID");
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.MarcaId).HasColumnName("MARCA_ID");
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placa).HasMaxLength(20);
            entity.Property(e => e.TipoVehiculoId).HasColumnName("TIPO_VEHICULO_ID");

            entity.HasOne(d => d.Empresa).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MAQUINA_EMPRESA");

            entity.HasOne(d => d.Marca).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.MarcaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VEHICULO_MARCA");

            entity.HasOne(d => d.TipoVehiculo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.TipoVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VEHICULO_TIPO_VEHICULO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
