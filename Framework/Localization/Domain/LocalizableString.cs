﻿using System;
using System.Runtime.Serialization;
using Framework.Data.Entity.EntityFramework;
using Framework.Tenants.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Localization.Domain
{
    [DataContract]
    public class LocalizableString : ITenantEntity
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int? TenantId { get; set; }

        [DataMember]
        public string CultureCode { get; set; }

        [DataMember]
        public string TextKey { get; set; }

        [DataMember]
        public string TextValue { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class LocalizableStringMap : IEntityTypeConfiguration<LocalizableString>, IFrameworkEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<LocalizableString> builder)
        {
            builder.ToTable("Framework_LocalizableStrings");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.CultureCode).HasMaxLength(10).IsUnicode(false);
            builder.Property(m => m.TextKey).IsRequired().IsUnicode(true);
            builder.Property(m => m.TextValue).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}