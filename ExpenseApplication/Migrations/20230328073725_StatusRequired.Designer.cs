﻿// <auto-generated />
using System;
using ExpenseApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseApplication.Migrations
{
    [DbContext(typeof(MasrafAppContext))]
    [Migration("20230328073725_StatusRequired")]
    partial class StatusRequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExpenseApplication.Models.Expense", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ExpenseID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("ExpenseDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ExpenseFormId")
                        .HasColumnType("int")
                        .HasColumnName("ExpenseFormID");

                    b.Property<string>("ReasonRejection")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ExpenseId")
                        .HasName("PK__Expenses__1445CFF3939CDAE8");

                    b.HasIndex("ExpenseFormId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("ExpenseApplication.Models.ExpenseForm", b =>
                {
                    b.Property<int>("ExpenseFormId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ExpenseFormID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseFormId"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("StatusID");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("ExpenseFormId")
                        .HasName("PK__ExpenseF__C5ECDAF530C9669A");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("ExpenseForms");
                });

            modelBuilder.Entity("ExpenseApplication.Models.ExpenseHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ActionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExpenseFormId")
                        .HasColumnType("int")
                        .HasColumnName("ExpenseFormID");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("StatusID");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("Id")
                        .HasName("PK__ExpenseH__3214EC27256B1F09");

                    b.HasIndex("ExpenseFormId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("ExpenseHistory", (string)null);
                });

            modelBuilder.Entity("ExpenseApplication.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId")
                        .HasName("PK__Roles__8AFACE1A0B3BF962");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ExpenseApplication.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("ExpenseApplication.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("ManagerID");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleID");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CCAC5B1EBF7E");

                    b.HasIndex("ManagerId");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "Username" }, "UQ__Users__536C85E41B3C797F")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExpenseApplication.Models.Expense", b =>
                {
                    b.HasOne("ExpenseApplication.Models.ExpenseForm", "ExpenseForm")
                        .WithMany("Expenses")
                        .HasForeignKey("ExpenseFormId")
                        .HasConstraintName("FK__Expenses__Expens__6383C8BA");

                    b.Navigation("ExpenseForm");
                });

            modelBuilder.Entity("ExpenseApplication.Models.ExpenseForm", b =>
                {
                    b.HasOne("ExpenseApplication.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("ExpenseApplication.Models.User", "User")
                        .WithMany("ExpenseForms")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__ExpenseFo__UserI__3E52440B");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExpenseApplication.Models.ExpenseHistory", b =>
                {
                    b.HasOne("ExpenseApplication.Models.ExpenseForm", "ExpenseForm")
                        .WithMany("ExpenseHistories")
                        .HasForeignKey("ExpenseFormId")
                        .HasConstraintName("FK__ExpenseHi__Expen__5FB337D6");

                    b.HasOne("ExpenseApplication.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.HasOne("ExpenseApplication.Models.User", "User")
                        .WithMany("ExpenseHistories")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__ExpenseHi__UserI__619B8048");

                    b.Navigation("ExpenseForm");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExpenseApplication.Models.User", b =>
                {
                    b.HasOne("ExpenseApplication.Models.User", "Manager")
                        .WithMany("InverseManager")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK__Users__ManagerID__38996AB5");

                    b.HasOne("ExpenseApplication.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Users_Roles");

                    b.Navigation("Manager");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ExpenseApplication.Models.ExpenseForm", b =>
                {
                    b.Navigation("ExpenseHistories");

                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("ExpenseApplication.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ExpenseApplication.Models.User", b =>
                {
                    b.Navigation("ExpenseForms");

                    b.Navigation("ExpenseHistories");

                    b.Navigation("InverseManager");
                });
#pragma warning restore 612, 618
        }
    }
}
