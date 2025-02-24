using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WaterManagement.Models;

public partial class WaterDbContext : DbContext
{
    public WaterDbContext()
    {
    }

    public WaterDbContext(DbContextOptions<WaterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clients> Clients { get; set; }

    public virtual DbSet<Pay> Pay { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:WaterConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clients>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Banco).HasMaxLength(255);
            entity.Property(e => e.Celular).HasMaxLength(255);
            entity.Property(e => e.Debt).HasColumnType("money");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.M3).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Multa).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Payed).HasColumnType("money");
            entity.Property(e => e.Recibo).HasMaxLength(255);
            entity.Property(e => e.Valor).HasColumnType("money");
        });

        modelBuilder.Entity<Pay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Pay_1");

            entity.ToTable("Pay");

            entity.Property(e => e.Banco).HasMaxLength(255);
            entity.Property(e => e.Data).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Meter).HasMaxLength(255);
            entity.Property(e => e.Nome).HasMaxLength(255);
            entity.Property(e => e.Recibo).HasMaxLength(255);
            entity.Property(e => e.Valor).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_USER_1");

            entity.ToTable("USER");

            entity.HasIndex(e => e.Email, "UQ__USER__A9D10534E5366674").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Celular).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Meter).HasMaxLength(255);
            entity.Property(e => e.Nome).HasMaxLength(255);
            entity.Property(e => e.State).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
