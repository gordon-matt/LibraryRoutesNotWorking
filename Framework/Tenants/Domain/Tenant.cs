using Extenso.Data.Entity;
using Framework.Data.Entity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Tenants.Domain
{
    public class Tenant : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        // TODO: Support SSL
        //public bool SslEnabled { get; set; }

        //public string SecureUrl { get; set; }

        public string Hosts { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class TenantMap : IEntityTypeConfiguration<Tenant>, IFrameworkEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Framework_Tenants");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(255).IsUnicode(true);
            //builder.Property(x => x.SecureUrl).HasMaxLength(255).IsUnicode(true);
            builder.Property(x => x.Hosts).HasMaxLength(1024).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}