using Framework.Data.Entity.EntityFramework;
using Framework.Tenants.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainApp.Data.Domain
{
    public class UserProfileEntry : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class UserProfileEntryMap : IEntityTypeConfiguration<UserProfileEntry>, IFrameworkEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<UserProfileEntry> builder)
        {
            builder.ToTable(Constants.Tables.UserProfiles);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            builder.Property(x => x.Key).IsRequired().HasMaxLength(255).IsUnicode(true);
            builder.Property(x => x.Value).IsRequired().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}