using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");

            builder.ToTable("Teams");
        }
    }
}