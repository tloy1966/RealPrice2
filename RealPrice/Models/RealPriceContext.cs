using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace RealPrice.Models
{
    
    public partial class RealPriceContext : DbContext
    {
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<MainData> MainData { get; set; }
        public virtual DbSet<Mrtgeo> Mrtgeo { get; set; }
        public virtual DbSet<RegData> RegData { get; set; }
        public virtual DbSet<SummaryData> SummaryData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Authority.DB.getConnect(false));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DisMrt).HasColumnName("disMRT");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("DISTRICT")
                    .HasMaxLength(50);

                entity.Property(e => e.DurationMrt).HasColumnName("durationMRT");

                entity.Property(e => e.Location1)
                    .HasColumnName("LOCATION")
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<MainData>(entity =>
            {
                entity.HasIndex(e => e.CaseF)
                    .HasName("idx_CASE_F");

                entity.HasIndex(e => e.Id2)
                    .HasName("uIdx1")
                    .IsUnique();

                entity.HasIndex(e => e.Location)
                    .HasName("idx_Location");

                entity.HasIndex(e => e.Sdate)
                    .HasName("idx_SDATE");

                entity.HasIndex(e => e.SellType)
                    .HasName("idx_SellType");

                entity.HasIndex(e => new { e.City, e.SellType })
                    .HasName("idx_City2");

                entity.HasIndex(e => new { e.City, e.SellType, e.District, e.CaseT })
                    .HasName("idx_City3");

                entity.HasIndex(e => new { e.Buitype, e.District, e.Fdate, e.Landa, e.Location, e.Sdate, e.Tprice, e.Uprice, e.IsActive, e.Pbuild, e.SellType, e.City })
                    .HasName("nci_wi_MainData_E208311DD5ADAC07791D0DA998ABA4E8");

                entity.HasIndex(e => new { e.Buitype, e.District, e.Fdate, e.Landa, e.Location, e.Sdate, e.Uprice, e.IsActive, e.Pbuild, e.SellType, e.City, e.Tprice })
                    .HasName("nci_wi_MainData_87A8CBF7B9A76A00E4AEA30E886D7797");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BuildB).HasColumnName("BUILD_B");

                entity.Property(e => e.BuildL).HasColumnName("BUILD_L");

                entity.Property(e => e.BuildP)
                    .HasColumnName("BUILD_P")
                    .HasMaxLength(50);

                entity.Property(e => e.BuildR).HasColumnName("BUILD_R");

                entity.Property(e => e.Buitype)
                    .HasColumnName("BUITYPE")
                    .HasMaxLength(200);

                entity.Property(e => e.CaseF)
                    .HasColumnName("CASE_F")
                    .HasMaxLength(50);

                entity.Property(e => e.CaseT)
                    .HasColumnName("CASE_T")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("DISTRICT")
                    .HasMaxLength(50);

                entity.Property(e => e.Farea)
                    .HasColumnName("FAREA")
                    .HasColumnType("decimal");

                entity.Property(e => e.Fdate)
                    .HasColumnName("FDATE")
                    .HasColumnType("date");

                entity.Property(e => e.Furniture).HasColumnName("FURNITURE");

                entity.Property(e => e.Id2)
                    .IsRequired()
                    .HasColumnName("ID2")
                    .HasMaxLength(400);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Landa)
                    .HasColumnName("LANDA")
                    .HasColumnType("decimal");

                entity.Property(e => e.LandaX)
                    .HasColumnName("LANDA_X")
                    .HasMaxLength(50);

                entity.Property(e => e.LandaY)
                    .HasColumnName("LANDA_Y")
                    .HasMaxLength(50);

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Location)
                    .HasColumnName("LOCATION")
                    .HasMaxLength(400);

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Mbuild)
                    .HasColumnName("MBUILD")
                    .HasMaxLength(300);

                entity.Property(e => e.Parea)
                    .HasColumnName("PAREA")
                    .HasColumnType("decimal");

                entity.Property(e => e.Parktype)
                    .HasColumnName("PARKTYPE")
                    .HasMaxLength(50);

                entity.Property(e => e.Pbuild)
                    .HasColumnName("PBUILD")
                    .HasMaxLength(50);

                entity.Property(e => e.Pprice).HasColumnName("PPRICE");

                entity.Property(e => e.Rmnote)
                    .HasColumnName("RMNOTE")
                    .HasMaxLength(400);

                entity.Property(e => e.Rule)
                    .HasColumnName("RULE")
                    .HasMaxLength(50);

                entity.Property(e => e.Sbuild)
                    .HasColumnName("SBUILD")
                    .HasMaxLength(200);

                entity.Property(e => e.Scnt)
                    .HasColumnName("SCNT")
                    .HasMaxLength(200);

                entity.Property(e => e.Sdate)
                    .HasColumnName("SDATE")
                    .HasColumnType("date");

                entity.Property(e => e.SellType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Tbuild)
                    .HasColumnName("TBUILD")
                    .HasMaxLength(50);

                entity.Property(e => e.Tprice).HasColumnName("TPRICE");

                entity.Property(e => e.Uprice)
                    .HasColumnName("UPRICE")
                    .HasColumnType("decimal");
            });

            modelBuilder.Entity<Mrtgeo>(entity =>
            {
                entity.HasKey(e => e.Mrt)
                    .HasName("PKMrt");

                entity.ToTable("MRTGeo");

                entity.Property(e => e.Mrt)
                    .HasColumnName("MRT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.PlaceId)
                    .IsRequired()
                    .HasColumnName("place_ID")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<RegData>(entity =>
            {
                entity.HasIndex(e => new { e.City, e.SellType, e.District, e.PBuild, e.PLocation })
                    .HasName("idx_uni")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("DISTRICT")
                    .HasMaxLength(50);

                entity.Property(e => e.Modifydate)
                    .HasColumnName("modifydate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PBuild)
                    .IsRequired()
                    .HasColumnName("pBuild")
                    .HasMaxLength(50);

                entity.Property(e => e.PBuildB).HasColumnName("p_build_B");

                entity.Property(e => e.PBuildL).HasColumnName("p_build_L");

                entity.Property(e => e.PBuildP).HasColumnName("p_build_P");

                entity.Property(e => e.PBuildR).HasColumnName("p_build_R");

                entity.Property(e => e.PDayFromBuild).HasColumnName("p_dayFromBuild");

                entity.Property(e => e.PLanda).HasColumnName("p_landa");

                entity.Property(e => e.PLocation)
                    .IsRequired()
                    .HasColumnName("P_LOCATION")
                    .HasMaxLength(400);

                entity.Property(e => e.PRmnote).HasColumnName("p_rmnote");

                entity.Property(e => e.PRule).HasColumnName("p_rule");

                entity.Property(e => e.PUprice).HasColumnName("p_uprice");

                entity.Property(e => e.SellType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SummaryData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CaseT)
                    .HasColumnName("CASE_T")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("DISTRICT")
                    .HasMaxLength(50);

                entity.Property(e => e.Location)
                    .HasColumnName("LOCATION")
                    .HasMaxLength(400);

                entity.Property(e => e.Sdate)
                    .HasColumnName("SDATE")
                    .HasColumnType("date");

                entity.Property(e => e.SellType)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}