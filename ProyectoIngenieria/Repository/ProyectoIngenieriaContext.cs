using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProyectoIngenieria.Models;
using Microsoft.AspNetCore.Identity;

namespace ProyectoIngenieria.Repository;

public partial class ProyectoIngenieriaContext : IdentityDbContext
{
  
    public ProyectoIngenieriaContext(DbContextOptions<ProyectoIngenieriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatalogoMantenimiento> CatalogoMantenimientos { get; set; }

    public virtual DbSet<DocumentoOperador> DocumentoOperadors { get; set; }

    public virtual DbSet<DocumentoVehiculo> DocumentoVehiculos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<HorasTrabajo> HorasTrabajos { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<Operador> Operadors { get; set; }

    public virtual DbSet<RegistroCombustible> RegistroCombustibles { get; set; }

    public virtual DbSet<RegistroMantenimiento> RegistroMantenimientos { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=.;Database=ProyectoIngenieria;Integrated Security=true;TrustServerCertificate=true;"
    //        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

    modelBuilder.Entity<CatalogoMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CATALOGO_MANTENIMIENTO_pk");

            entity.ToTable("CATALOGO_MANTENIMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(100);
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
            entity.Property(e => e.Lugar).HasMaxLength(200);
            entity.Property(e => e.PrecioHora)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_Hora");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.HorasTrabajos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORAS_TRABAJO_MAQUINA");
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
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.Operadors)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OPERADOR_MAQUINA");
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
            entity.Property(e => e.CatalogoMantenimientoId).HasColumnName("CATALOGO_MANTENIMIENTO_ID");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");

            entity.HasOne(d => d.CatalogoMantenimiento).WithMany(p => p.RegistroMantenimientos)
                .HasForeignKey(d => d.CatalogoMantenimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_MANTENIMIENTO_CATALOGO_MANTENIMIENTO");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.RegistroMantenimientos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTRO_MANTENIMIENTO_MAQUINA");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VEHICULO_pk");

            entity.ToTable("VEHICULO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID");
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placa).HasMaxLength(20);
            entity.Property(e => e.Tipo).HasMaxLength(30);

            entity.HasOne(d => d.Empresa).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MAQUINA_EMPRESA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
