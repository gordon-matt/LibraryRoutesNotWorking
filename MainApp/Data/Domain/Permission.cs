﻿using System.Collections.Generic;
using Framework.Data.Entity.EntityFramework;
using Framework.Tenants.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameworkDemo.Data.Domain
{
    public class Permission : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public virtual ICollection<RolePermission> RolesPermissions { get; set; }
    }

    public class PermissionMap : IEntityTypeConfiguration<Permission>, IFrameworkEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(Constants.Tables.Permissions);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50).IsUnicode(true);
            builder.Property(x => x.Category).IsRequired().HasMaxLength(50).IsUnicode(true);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(128).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}