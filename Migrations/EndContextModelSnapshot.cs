﻿// <auto-generated />
using System;
using EndApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EndApi.Migrations
{
    [DbContext(typeof(EndContext))]
    partial class EndContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("EndApi.Data.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AppRoles");
                });

            modelBuilder.Entity("EndApi.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("EndUserId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("EndApi.Data.EndUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("Email")
                        .HasMaxLength(300);

                    b.Property<string>("Initial")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Unicode")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.ToTable("EndUsers");
                });

            modelBuilder.Entity("EndApi.Data.Following", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired();

                    b.Property<string>("FollowedById");

                    b.Property<string>("FollowedUserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FollowedById");

                    b.HasIndex("FollowedUserId");

                    b.ToTable("Followings");
                });

            modelBuilder.Entity("EndApi.Data.Food", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("FoodDay");

                    b.Property<int?>("FoodTime")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PlanId");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("EndApi.Data.FoodPicture", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Extension")
                        .IsRequired();

                    b.Property<string>("FoodId");

                    b.Property<string>("FriendlName")
                        .IsRequired();

                    b.Property<string>("MIMEType")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.ToTable("FoodPictures");
                });

            modelBuilder.Entity("EndApi.Data.Measure", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsPeriodic");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Order");

                    b.Property<bool>("TakeLast");

                    b.Property<string>("Unit")
                        .IsRequired();

                    b.HasKey("Key");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("EndApi.Data.MeasurementRevision", b =>
                {
                    b.Property<string>("Key");

                    b.Property<string>("RevisionId");

                    b.Property<int?>("Order")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Key", "RevisionId");

                    b.HasIndex("RevisionId");

                    b.ToTable("MeasurementRevision");
                });

            modelBuilder.Entity("EndApi.Data.NutritionProperty", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Unit")
                        .HasMaxLength(30);

                    b.HasKey("Key");

                    b.ToTable("NutritionProperties");
                });

            modelBuilder.Entity("EndApi.Data.PlanNutritional", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime?>("CreateAt")
                        .IsRequired();

                    b.Property<bool>("IsReleased");

                    b.Property<int?>("PeriodDays");

                    b.Property<DateTime?>("ReleasedAt");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("EndApi.Data.PlanProperty", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PlanId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Key");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanProperties");
                });

            modelBuilder.Entity("EndApi.Data.Revision", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignedUserId");

                    b.Property<string>("Concept")
                        .HasMaxLength(100);

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<DateTime?>("Date")
                        .IsRequired();

                    b.Property<DateTime?>("NextRevisionDate")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Revision");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EndApi.Data.Following", b =>
                {
                    b.HasOne("EndApi.Data.EndUser", "FollowedBy")
                        .WithMany("Followings")
                        .HasForeignKey("FollowedById");

                    b.HasOne("EndApi.Data.EndUser", "FollowedUser")
                        .WithMany()
                        .HasForeignKey("FollowedUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EndApi.Data.Food", b =>
                {
                    b.HasOne("EndApi.Data.PlanNutritional", "Plan")
                        .WithMany("Foods")
                        .HasForeignKey("PlanId");
                });

            modelBuilder.Entity("EndApi.Data.FoodPicture", b =>
                {
                    b.HasOne("EndApi.Data.Food", "Food")
                        .WithMany("Pictures")
                        .HasForeignKey("FoodId");
                });

            modelBuilder.Entity("EndApi.Data.MeasurementRevision", b =>
                {
                    b.HasOne("EndApi.Data.Revision", "Revision")
                        .WithMany("Measurements")
                        .HasForeignKey("RevisionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EndApi.Data.PlanProperty", b =>
                {
                    b.HasOne("EndApi.Data.PlanNutritional", "Plan")
                        .WithMany("PlanProperties")
                        .HasForeignKey("PlanId");
                });

            modelBuilder.Entity("EndApi.Data.Revision", b =>
                {
                    b.HasOne("EndApi.Data.EndUser", "AssignedUser")
                        .WithMany()
                        .HasForeignKey("AssignedUserId");

                    b.HasOne("EndApi.Data.EndUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("EndApi.Data.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EndApi.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EndApi.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("EndApi.Data.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EndApi.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EndApi.Data.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
