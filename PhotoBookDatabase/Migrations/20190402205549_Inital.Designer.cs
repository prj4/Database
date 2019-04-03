﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoBookDatabase.Data;

namespace PhotoBookDatabase.Migrations
{
    [DbContext(typeof(PhotoBookDbContext))]
    [Migration("20190402205549_Inital")]
    partial class Inital
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhotoBookDatabase.Model.Event", b =>
                {
                    b.Property<int>("Pin")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("HostId");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Pin");

                    b.HasIndex("HostId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Pin = 1,
                            Description = "Beskrivelse1",
                            EndDate = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            HostId = 1,
                            Location = "Lokation1",
                            Name = "Event1",
                            StartDate = new DateTime(2019, 4, 2, 22, 55, 49, 94, DateTimeKind.Local).AddTicks(7935)
                        },
                        new
                        {
                            Pin = 2,
                            Description = "Beskrivelse2",
                            EndDate = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            HostId = 2,
                            Location = "Lokation2",
                            Name = "Event2",
                            StartDate = new DateTime(2019, 4, 2, 22, 55, 49, 97, DateTimeKind.Local).AddTicks(5912)
                        },
                        new
                        {
                            Pin = 3,
                            Description = "Beskrivelse3",
                            EndDate = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            HostId = 3,
                            Location = "Lokation3",
                            Name = "Event3",
                            StartDate = new DateTime(2019, 4, 2, 22, 55, 49, 97, DateTimeKind.Local).AddTicks(5929)
                        });
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.EventGuest", b =>
                {
                    b.Property<int>("Event_Pin");

                    b.Property<int>("Guest_Id");

                    b.HasKey("Event_Pin", "Guest_Id");

                    b.HasIndex("Guest_Id");

                    b.ToTable("EventGuests");

                    b.HasData(
                        new
                        {
                            Event_Pin = 1,
                            Guest_Id = 4
                        },
                        new
                        {
                            Event_Pin = 2,
                            Guest_Id = 5
                        },
                        new
                        {
                            Event_Pin = 3,
                            Guest_Id = 6
                        });
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.Picture", b =>
                {
                    b.Property<int>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventPin");

                    b.Property<int>("Taker");

                    b.Property<string>("URL");

                    b.HasKey("PictureId");

                    b.HasIndex("EventPin");

                    b.HasIndex("Taker");

                    b.ToTable("Pictures");

                    b.HasData(
                        new
                        {
                            PictureId = 1,
                            EventPin = 1,
                            Taker = 1,
                            URL = "wwwroot/Images/1.png"
                        },
                        new
                        {
                            PictureId = 2,
                            EventPin = 2,
                            Taker = 2,
                            URL = "wwwroot/Images/2.png"
                        },
                        new
                        {
                            PictureId = 3,
                            EventPin = 3,
                            Taker = 3,
                            URL = "wwwroot/Images/3.png"
                        });
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.PictureTaker", b =>
                {
                    b.Property<int>("PictureTakerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.HasKey("PictureTakerId");

                    b.ToTable("PictureTakers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PictureTaker");
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.Guest", b =>
                {
                    b.HasBaseType("PhotoBookDatabase.Model.PictureTaker");

                    b.HasDiscriminator().HasValue("Guest");

                    b.HasData(
                        new
                        {
                            PictureTakerId = 4,
                            Name = "Guest1"
                        },
                        new
                        {
                            PictureTakerId = 5,
                            Name = "Guest2"
                        },
                        new
                        {
                            PictureTakerId = 6,
                            Name = "Guest3"
                        });
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.Host", b =>
                {
                    b.HasBaseType("PhotoBookDatabase.Model.PictureTaker");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("PW")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.HasDiscriminator().HasValue("Host");

                    b.HasData(
                        new
                        {
                            PictureTakerId = 1,
                            Name = "Host",
                            Email = "Email1@email.com",
                            PW = "PWPWPWPWPW1",
                            Username = "Username1"
                        },
                        new
                        {
                            PictureTakerId = 2,
                            Name = "Host2",
                            Email = "Email2@email.com",
                            PW = "PWPWPWPWPW2",
                            Username = "Username2"
                        },
                        new
                        {
                            PictureTakerId = 3,
                            Name = "Host3",
                            Email = "Email3@email.com",
                            PW = "PWPWPWPWPW3",
                            Username = "Username3"
                        });
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.Event", b =>
                {
                    b.HasOne("PhotoBookDatabase.Model.Host", "Host")
                        .WithMany("Events")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.EventGuest", b =>
                {
                    b.HasOne("PhotoBookDatabase.Model.Event", "Event")
                        .WithMany("EventGuests")
                        .HasForeignKey("Event_Pin")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PhotoBookDatabase.Model.Guest", "Guest")
                        .WithMany("EventGuests")
                        .HasForeignKey("Guest_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PhotoBookDatabase.Model.Picture", b =>
                {
                    b.HasOne("PhotoBookDatabase.Model.Event", "Event")
                        .WithMany("Pictures")
                        .HasForeignKey("EventPin")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PhotoBookDatabase.Model.PictureTaker", "PictureTaker")
                        .WithMany("Pictures")
                        .HasForeignKey("Taker")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}