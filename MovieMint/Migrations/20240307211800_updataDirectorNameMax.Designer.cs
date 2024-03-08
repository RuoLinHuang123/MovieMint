﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieMint.Models;

#nullable disable

namespace MovieMint.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240307211800_updataDirectorNameMax")]
    partial class updataDirectorNameMax
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieMint.Models.Certificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("MovieMint.Models.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("MovieMint.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MovieMint.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CertificateId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DirectorId")
                        .HasColumnType("int");

                    b.Property<long?>("Gross")
                        .HasColumnType("bigint");

                    b.Property<decimal>("IMDBRating")
                        .HasColumnType("decimal(2, 1)");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int?>("MetaScore")
                        .HasColumnType("int");

                    b.Property<int>("NoOfVotes")
                        .HasColumnType("int");

                    b.Property<string>("Overview")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("PosterLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReleasedYear")
                        .HasColumnType("int");

                    b.Property<string>("Runtime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeriesTitle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("CertificateId");

                    b.HasIndex("DirectorId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieMint.Models.Movies_Genres", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MovieId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies_Genres");
                });

            modelBuilder.Entity("MovieMint.Models.Movies_Stars", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("StarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MovieId", "StarId");

                    b.HasIndex("StarId");

                    b.ToTable("Movies_Stars");
                });

            modelBuilder.Entity("MovieMint.Models.Star", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Stars");
                });

            modelBuilder.Entity("MovieMint.Models.Movie", b =>
                {
                    b.HasOne("MovieMint.Models.Certificate", "Certificate")
                        .WithMany("Movies")
                        .HasForeignKey("CertificateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MovieMint.Models.Director", "Director")
                        .WithMany("Movies")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Certificate");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("MovieMint.Models.Movies_Genres", b =>
                {
                    b.HasOne("MovieMint.Models.Genre", "Genre")
                        .WithMany("Movies_Genres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieMint.Models.Movie", "Movie")
                        .WithMany("Movies_Genres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieMint.Models.Movies_Stars", b =>
                {
                    b.HasOne("MovieMint.Models.Movie", "Movie")
                        .WithMany("Movies_Stars")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieMint.Models.Star", "Star")
                        .WithMany("Movies_Stars")
                        .HasForeignKey("StarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Star");
                });

            modelBuilder.Entity("MovieMint.Models.Certificate", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("MovieMint.Models.Director", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("MovieMint.Models.Genre", b =>
                {
                    b.Navigation("Movies_Genres");
                });

            modelBuilder.Entity("MovieMint.Models.Movie", b =>
                {
                    b.Navigation("Movies_Genres");

                    b.Navigation("Movies_Stars");
                });

            modelBuilder.Entity("MovieMint.Models.Star", b =>
                {
                    b.Navigation("Movies_Stars");
                });
#pragma warning restore 612, 618
        }
    }
}
