using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class datawatchContext : DbContext
    {
        public datawatchContext()
        {
        }

        public datawatchContext(DbContextOptions<datawatchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brakerecognition> Brakerecognitions { get; set; }
        public virtual DbSet<Bumprecognition> Bumprecognitions { get; set; }
        public virtual DbSet<Gpsrecord> Gpsrecords { get; set; }
        public virtual DbSet<SatictisAnalysisdataAcc> SatictisAnalysisdataAccs { get; set; }
        public virtual DbSet<SatictisAnalysisdataWft> SatictisAnalysisdataWfts { get; set; }
        public virtual DbSet<Speeddistribution> Speeddistributions { get; set; }
        public virtual DbSet<Streeringrecognition> Streeringrecognitions { get; set; }
        public virtual DbSet<SysAuthority> SysAuthorities { get; set; }
        public virtual DbSet<Throttlerecognition> Throttlerecognitions { get; set; }
        public virtual DbSet<Vehicletable> Vehicletables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=Mxz04122465;database=datawatch", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Brakerecognition>(entity =>
            {
                entity.ToTable("brakerecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.BrakeAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Bumprecognition>(entity =>
            {
                entity.ToTable("bumprecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.BumpAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Gpsrecord>(entity =>
            {
                entity.ToTable("gpsrecord");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Lat).HasColumnType("double(64,5)");

                entity.Property(e => e.Lon).HasColumnType("double(64,5)");

                entity.Property(e => e.Speed).HasColumnType("double(64,0)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SatictisAnalysisdataAcc>(entity =>
            {
                entity.ToTable("satictis_analysisdata_acc");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Chantitle)
                    .HasMaxLength(64)
                    .HasColumnName("chantitle");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Max)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("max");

                entity.Property(e => e.Min)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("min");

                entity.Property(e => e.Range)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("range");

                entity.Property(e => e.Rms)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("rms");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SatictisAnalysisdataWft>(entity =>
            {
                entity.ToTable("satictis_analysisdata_wft");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Chantitle)
                    .HasMaxLength(64)
                    .HasColumnName("chantitle");

                entity.Property(e => e.Damage)
                    .HasColumnType("double(30,10)")
                    .HasColumnName("damage");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Max)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("max");

                entity.Property(e => e.Min)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("min");

                entity.Property(e => e.Range)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("range");

                entity.Property(e => e.Rms)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("rms");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Speeddistribution>(entity =>
            {
                entity.ToTable("speeddistribution");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Above120).HasColumnType("double(20,5)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");

                entity.Property(e => e._010)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("0-10");

                entity.Property(e => e._100110)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("100-110");

                entity.Property(e => e._1020)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("10-20");

                entity.Property(e => e._110120)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("110-120");

                entity.Property(e => e._2030)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("20-30");

                entity.Property(e => e._3040)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("30-40");

                entity.Property(e => e._4050)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("40-50");

                entity.Property(e => e._5060)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("50-60");

                entity.Property(e => e._6070)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("60-70");

                entity.Property(e => e._7080)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("70-80");

                entity.Property(e => e._8090)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("80-90");

                entity.Property(e => e._90100)
                    .HasColumnType("double(20,5)")
                    .HasColumnName("90-100");
            });

            modelBuilder.Entity<Streeringrecognition>(entity =>
            {
                entity.ToTable("streeringrecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.AngularAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.Speed).HasColumnType("double(64,2)");

                entity.Property(e => e.SteeringAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.SteeringDirection).HasColumnType("tinyint(2)");

                entity.Property(e => e.StrgWhlAng).HasColumnType("double(64,2)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<SysAuthority>(entity =>
            {
                entity.ToTable("sys_authority");

                entity.Property(e => e.Id)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("ID");

                entity.Property(e => e.AuthorityKey).HasColumnType("int(10)");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Throttlerecognition>(entity =>
            {
                entity.ToTable("throttlerecognition");

                entity.Property(e => e.Id)
                    .HasMaxLength(64)
                    .HasColumnName("id");

                entity.Property(e => e.Accelerograph).HasColumnType("double(64,1)");

                entity.Property(e => e.Datadate)
                    .HasColumnType("datetime")
                    .HasColumnName("datadate");

                entity.Property(e => e.Filename)
                    .HasMaxLength(64)
                    .HasColumnName("filename");

                entity.Property(e => e.LastingTime).HasColumnType("double(64,2)");

                entity.Property(e => e.Reverse).HasColumnType("tinyint(2)");

                entity.Property(e => e.Speed).HasColumnType("double(64,1)");

                entity.Property(e => e.ThrottleAcc).HasColumnType("double(64,2)");

                entity.Property(e => e.VehicleId)
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Vehicletable>(entity =>
            {
                entity.ToTable("vehicletable");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Area).HasMaxLength(64);

                entity.Property(e => e.Country).HasMaxLength(64);

                entity.Property(e => e.Remarks).HasMaxLength(255);

                entity.Property(e => e.State).HasColumnType("tinyint(3) unsigned zerofill");

                entity.Property(e => e.VehicleId)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("VehicleID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
