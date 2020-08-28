using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");

            builder.ToTable("Users");
        }
    }
}