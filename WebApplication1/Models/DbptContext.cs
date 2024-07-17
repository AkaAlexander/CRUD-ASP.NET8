using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class DbptContext : DbContext
{
    public DbptContext()
    {
    }

    public DbptContext(DbContextOptions<DbptContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compania> Compania { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Geo> Geos { get; set; }

    public virtual DbSet<MiTabla> MiTablas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=DBPT;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compania>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Compania__1788CC4CC207AB17");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Bs).HasMaxLength(255);
            entity.Property(e => e.CatchPhrase).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.User).WithOne(p => p.Compania)
                .HasForeignKey<Compania>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compania__UserId__5EBF139D");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Direccio__1788CC4CF9CECA94");

            entity.ToTable("Direccion");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.Suite).HasMaxLength(255);
            entity.Property(e => e.Zipcode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.Direccion)
                .HasForeignKey<Direccion>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Direccion__UserI__59063A47");
        });

        modelBuilder.Entity<Geo>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("PK__Geo__68906D643A0FFA0E");

            entity.ToTable("Geo");

            entity.Property(e => e.DireccionId).ValueGeneratedNever();
            entity.Property(e => e.Lat).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.Lng).HasColumnType("decimal(10, 7)");

            entity.HasOne(d => d.Direccion).WithOne(p => p.Geo)
                .HasForeignKey<Geo>(d => d.DireccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Geo__DireccionId__5BE2A6F2");
        });

        modelBuilder.Entity<MiTabla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MiTabla__3214EC07DF386B58");

            entity.ToTable("MiTabla");

            entity.Property(e => e.Valor).HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC0764878166");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(255);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
