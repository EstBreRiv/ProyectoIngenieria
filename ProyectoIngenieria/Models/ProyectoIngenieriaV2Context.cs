using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoIngenieria.Models;

public partial class ProyectoIngenieriaV2Context : DbContext
{
    public ProyectoIngenieriaV2Context()
    {
    }

    public ProyectoIngenieriaV2Context(DbContextOptions<ProyectoIngenieriaV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ProyectoIngenieriaV2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PROYECTO_pk");

            entity.ToTable("PROYECTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cliente).HasMaxLength(100);
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.NombreProyecto)
                .HasMaxLength(100)
                .HasColumnName("Nombre_Proyecto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
