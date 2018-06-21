﻿using Framework.Configuration.Domain;
using Framework.Data.Entity.EntityFramework;
using Framework.Identity.Domain;
using Framework.Infrastructure;
using Framework.Localization.Domain;
using Framework.Logging.Domain;
using Framework.Tasks.Domain;
using Framework.Tenants.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LanguageEntity = Framework.Localization.Domain.Language;

namespace Framework.Identity
{
    public abstract class FrameworkIdentityDbContext<TUser, TRole>
        : IdentityDbContext<TUser, TRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
        where TUser : FrameworkIdentityUser
        where TRole : FrameworkIdentityRole
    {
        #region Constructors

        public FrameworkIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion Constructors

        public DbSet<LanguageEntity> Languages { get; set; }

        public DbSet<LocalizableProperty> LocalizableProperties { get; set; }

        public DbSet<LocalizableString> LocalizableStrings { get; set; }

        public DbSet<LogEntry> Log { get; set; }

        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //var usersTable = modelBuilder.Entity<TUser>().ToTable("AspNetUsers");
            //usersTable.HasMany(x => x.Roles).WithRequired().HasForeignKey(x => x.UserId);
            //usersTable.HasMany(x => x.Claims).WithRequired().HasForeignKey(x => x.UserId);
            //usersTable.HasMany(x => x.Logins).WithRequired().HasForeignKey(x => x.UserId);

            //usersTable.Property(x => x.TenantId)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 1 }));

            //usersTable.Property(x => x.UserName)
            //    .IsRequired()
            //    .HasMaxLength(256)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 2 }));

            //usersTable.Property(x => x.Email).HasMaxLength(256);

            //modelBuilder.Entity<IdentityUserRole>().HasKey(x => new
            //{
            //    UserId = x.UserId,
            //    RoleId = x.RoleId
            //}).ToTable("AspNetUserRoles");

            //modelBuilder.Entity<IdentityUserLogin>().HasKey(x => new
            //{
            //    LoginProvider = x.LoginProvider,
            //    ProviderKey = x.ProviderKey,
            //    UserId = x.UserId
            //}).ToTable("AspNetUserLogins");

            //modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            //var rolesTable = modelBuilder.Entity<TRole>().ToTable("AspNetRoles");

            //rolesTable.Property(x => x.TenantId)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true, Order = 1 }));

            //rolesTable.Property(x => x.Name)
            //    .IsRequired()
            //    .HasMaxLength(256)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true, Order = 2 }));

            //rolesTable.HasMany(x => x.Users).WithRequired().HasForeignKey(x => x.RoleId);

            var configurations = EngineContext.Current.ResolveAll<IFrameworkEntityTypeConfiguration>();

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.ApplyConfiguration(configuration);
            }
        }
    }
}