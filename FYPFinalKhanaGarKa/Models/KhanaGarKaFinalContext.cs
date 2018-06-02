using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class KhanaGarKaFinalContext : DbContext
    {
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Chef> Chef { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DeliveryBoy> DeliveryBoy { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<NewsLetter> NewsLetter { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }

        public KhanaGarKaFinalContext(DbContextOptions<KhanaGarKaFinalContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.Cnic)
                    .IsRequired()
                    .HasColumnName("CNIC")
                    .HasMaxLength(13);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhonNo)
                    .IsRequired()
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<Chef>(entity =>
            {
                entity.HasIndex(e => e.Cnic)
                    .HasName("UQ__Chef__AA570FD4FF9E7A93")
                    .IsUnique();

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.About).HasMaxLength(300);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Cnic)
                    .IsRequired()
                    .HasColumnName("CNIC")
                    .HasMaxLength(13);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ImgUrl).HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Cnic)
                    .HasName("UQ__Customer__AA570FD466690C2F")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Cnic)
                    .IsRequired()
                    .HasColumnName("CNIC")
                    .HasMaxLength(13);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ImgUrl).HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(50);
            });

            modelBuilder.Entity<DeliveryBoy>(entity =>
            {
                entity.HasIndex(e => e.Cnic)
                    .HasName("UQ__Delivery__AA570FD4545CCB3C")
                    .IsUnique();

                entity.Property(e => e.DeliveryBoyId).HasColumnName("DeliveryBoyID");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Cnic)
                    .IsRequired()
                    .HasColumnName("CNIC")
                    .HasMaxLength(13);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ImgUrl).HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(50);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DishName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ImgUrl).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menu_Chef");
            });

            modelBuilder.Entity<NewsLetter>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ImgUrl).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.OfferName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Offer__chefId__34C8D9D1");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLine_Orders");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ChefId).HasColumnName("ChefID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DeliveryBoyId).HasColumnName("DeliveryBoyID");

                entity.Property(e => e.DeliveryTime).HasMaxLength(10);

                entity.Property(e => e.OrderDate).HasColumnType("datetime2(6)");

                entity.Property(e => e.Street).HasMaxLength(100);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ChefId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__ChefId__3D5E1FD2");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Customer__3F466844");

                entity.HasOne(d => d.DeliveryBoy)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryBoyId)
                    .HasConstraintName("FK__Orders__Delivery__3E52440B");
            });
        }
    }
}
