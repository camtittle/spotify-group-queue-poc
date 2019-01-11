﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Models;

namespace api.Migrations
{
    [DbContext(typeof(apiContext))]
    partial class apiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("api.Models.Party", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("api.Models.QueueItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedByUserId");

                    b.Property<string>("Artist");

                    b.Property<long>("DurationMillis");

                    b.Property<string>("ForPartyId");

                    b.Property<int>("Index");

                    b.Property<string>("SpotifyUri");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("ForPartyId");

                    b.ToTable("QueueItems");
                });

            modelBuilder.Entity("api.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrentPartyId");

                    b.Property<bool>("IsMember")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("CAST(CASE WHEN CurrentPartyId IS NULL THEN 0 ELSE 1 END AS BIT)");

                    b.Property<bool>("IsOwner")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("CAST(CASE WHEN OwnedPartyId IS NULL THEN 0 ELSE 1 END AS BIT)");

                    b.Property<bool>("IsPendingMember")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("CAST(CASE WHEN PendingPartyId IS NULL THEN 0 ELSE 1 END AS BIT)");

                    b.Property<DateTime>("JoinedPartyDateTime");

                    b.Property<string>("OwnedPartyId");

                    b.Property<string>("PendingPartyId");

                    b.Property<string>("SpotifyAccessToken");

                    b.Property<string>("SpotifyDeviceId");

                    b.Property<string>("SpotifyDeviceName");

                    b.Property<string>("SpotifyRefreshToken");

                    b.Property<DateTime>("SpotifyTokenExpiry");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CurrentPartyId");

                    b.HasIndex("OwnedPartyId")
                        .IsUnique()
                        .HasFilter("[OwnedPartyId] IS NOT NULL");

                    b.HasIndex("PendingPartyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api.Models.QueueItem", b =>
                {
                    b.HasOne("api.Models.User", "AddedByUser")
                        .WithMany("QueueItems")
                        .HasForeignKey("AddedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("api.Models.Party", "ForParty")
                        .WithMany("QueueItems")
                        .HasForeignKey("ForPartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("api.Models.User", b =>
                {
                    b.HasOne("api.Models.Party", "CurrentParty")
                        .WithMany("Members")
                        .HasForeignKey("CurrentPartyId");

                    b.HasOne("api.Models.Party", "OwnedParty")
                        .WithOne("Owner")
                        .HasForeignKey("api.Models.User", "OwnedPartyId");

                    b.HasOne("api.Models.Party", "PendingParty")
                        .WithMany("PendingMembers")
                        .HasForeignKey("PendingPartyId");
                });
#pragma warning restore 612, 618
        }
    }
}
