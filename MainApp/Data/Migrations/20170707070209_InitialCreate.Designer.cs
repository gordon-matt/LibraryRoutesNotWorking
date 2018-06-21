using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FrameworkDemo.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FrameworkDemo.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170707070209_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("FrameworkDemo.Data.Domain.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("FrameworkDemo.Data.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

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

                    b.Property<int?>("TenantId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FrameworkDemo.Data.Domain.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("FrameworkDemo.Data.Domain.RolePermission", b =>
                {
                    b.Property<int>("PermissionId");

                    b.Property<string>("RoleId")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.HasKey("PermissionId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("FrameworkDemo.Data.Domain.UserProfileEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int?>("TenantId");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(true);

                    b.Property<string>("Value")
                        .IsRequired()
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Framework.Configuration.Domain.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int?>("TenantId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Value")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_Settings");
                });

            modelBuilder.Entity("Framework.Localization.Domain.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CultureCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<bool>("IsEnabled");

                    b.Property<bool>("IsRTL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int>("SortOrder");

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Framework_Languages");
                });

            modelBuilder.Entity("Framework.Localization.Domain.LocalizableProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CultureCode")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasMaxLength(512)
                        .IsUnicode(false);

                    b.Property<string>("Property")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<string>("Value")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_LocalizableProperties");
                });

            modelBuilder.Entity("Framework.Localization.Domain.LocalizableString", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CultureCode")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<int?>("TenantId");

                    b.Property<string>("TextKey")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<string>("TextValue")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_LocalizableStrings");
                });

            modelBuilder.Entity("Framework.Logging.Domain.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ErrorClass")
                        .HasMaxLength(512)
                        .IsUnicode(false);

                    b.Property<string>("ErrorMessage")
                        .IsUnicode(true);

                    b.Property<string>("ErrorMethod")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("ErrorSource")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<DateTime>("EventDateTime");

                    b.Property<string>("EventLevel")
                        .IsRequired()
                        .HasMaxLength(5)
                        .IsUnicode(false);

                    b.Property<string>("EventMessage")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<string>("InnerErrorMessage")
                        .IsUnicode(true);

                    b.Property<string>("MachineName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int?>("TenantId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_Log");
                });

            modelBuilder.Entity("Framework.Messaging.Domain.MessageTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<bool>("Enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<Guid?>("OwnerId");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int?>("TenantId");

                    b.HasKey("Id");

                    b.ToTable("Framework_MessageTemplates");
                });

            modelBuilder.Entity("Framework.Messaging.Domain.QueuedEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<string>("FromAddress")
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<string>("FromName")
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<string>("MailMessage")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<int>("Priority");

                    b.Property<DateTime?>("SentOnUtc");

                    b.Property<int>("SentTries");

                    b.Property<string>("Subject")
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int?>("TenantId");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<string>("ToName")
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_QueuedEmails");
                });

            modelBuilder.Entity("Framework.Tasks.Domain.ScheduledTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Enabled");

                    b.Property<DateTime?>("LastEndUtc");

                    b.Property<DateTime?>("LastStartUtc");

                    b.Property<DateTime?>("LastSuccessUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<int>("Seconds");

                    b.Property<bool>("StopOnError");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Framework_ScheduledTasks");
                });

            modelBuilder.Entity("Framework.Tenants.Domain.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hosts")
                        .HasMaxLength(1024)
                        .IsUnicode(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_Tenants");
                });

            modelBuilder.Entity("Framework.Web.Configuration.Domain.GenericAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasMaxLength(512)
                        .IsUnicode(false);

                    b.Property<string>("Property")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<string>("Value")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Framework_GenericAttributes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FrameworkDemo.Data.Domain.RolePermission", b =>
                {
                    b.HasOne("FrameworkDemo.Data.Domain.Permission", "Permission")
                        .WithMany("RolesPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationRole", "Role")
                        .WithMany("RolesPermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FrameworkDemo.Data.Domain.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
