using System;
using System.Collections.Generic;
using ExpenseApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApplication.Data;

public partial class MasrafAppContext : DbContext
{
    public MasrafAppContext()
    {
    }

    public MasrafAppContext(DbContextOptions<MasrafAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseForm> ExpenseForms { get; set; }

    public virtual DbSet<ExpenseHistory> ExpenseHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Status> Statuses { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ISTTURNB439\\MSSQLSERVER01;Database=MasrafApp;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expenses__1445CFF3939CDAE8");

            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ExpenseDate).HasColumnType("datetime");
            entity.Property(e => e.ExpenseFormId).HasColumnName("ExpenseFormID");
            entity.Property(e => e.ReasonRejection).HasMaxLength(500);

            entity.HasOne(d => d.ExpenseForm).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseFormId)
                .HasConstraintName("FK__Expenses__Expens__6383C8BA");
        });


        modelBuilder.Entity<ExpenseForm>(entity =>
        {
            entity.HasKey(e => e.ExpenseFormId).HasName("PK__ExpenseF__C5ECDAF530C9669A");

            entity.Property(e => e.ExpenseFormId).HasColumnName("ExpenseFormID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");


            entity.HasOne(d => d.User).WithMany(p => p.ExpenseForms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpenseFo__UserI__3E52440B");
        });

        modelBuilder.Entity<ExpenseHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExpenseH__3214EC27256B1F09");

            entity.ToTable("ExpenseHistory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpenseFormId).HasColumnName("ExpenseFormID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ExpenseForm).WithMany(p => p.ExpenseHistories)
                .HasForeignKey(d => d.ExpenseFormId)
                .HasConstraintName("FK__ExpenseHi__Expen__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.ExpenseHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ExpenseHi__UserI__619B8048");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A0B3BF962");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC5B1EBF7E");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E41B3C797F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Users__ManagerID__38996AB5");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
