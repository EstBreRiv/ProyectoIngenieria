//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace ProyectoIngenieria.Models;

//public partial class ProyectoIngenieriaV2Context : DbContext
//{
//    public ProyectoIngenieriaV2Context()
//    {
//    }

//    public ProyectoIngenieriaV2Context(DbContextOptions<ProyectoIngenieriaV2Context> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<HorasTrabajo> HorasTrabajos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ProyectoIngenieriaV2;Integrated Security=true;TrustServerCertificate=True");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<HorasTrabajo>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("HORAS_TRABAJO_pk");

//            entity.ToTable("HORAS_TRABAJO");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.HorometroFinal)
//                .HasColumnType("decimal(18, 2)")
//                .HasColumnName("Horometro_Final");
//            entity.Property(e => e.HorometroInicial)
//                .HasColumnType("decimal(18, 2)")
//                .HasColumnName("Horometro_Inicial");
//            entity.Property(e => e.LugarTrabajoId).HasColumnName("LUGAR_TRABAJO_ID");
//            entity.Property(e => e.PrecioHora)
//                .HasColumnType("decimal(18, 2)")
//                .HasColumnName("Precio_Hora");
//            entity.Property(e => e.ProyectoId).HasColumnName("PROYECTO_ID");
//            entity.Property(e => e.TipoTrabajoId).HasColumnName("TIPO_TRABAJO_ID");
//            entity.Property(e => e.TotalGanancia)
//                .HasColumnType("decimal(18, 2)")
//                .HasColumnName("Total_Ganancia");
//            entity.Property(e => e.TotalHoras)
//                .HasColumnType("decimal(18, 2)")
//                .HasColumnName("Total_Horas");
//            entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
