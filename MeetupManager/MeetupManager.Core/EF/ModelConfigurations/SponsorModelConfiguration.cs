
namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class SponsorModelConfiguration : IEntityTypeConfiguration<Sponsor>
{
    public void Configure(EntityTypeBuilder<Sponsor> builder)
    {
        builder
            .ToTable("sponsors")
            .HasKey(_ => _.Id)
            .HasName("Id");

        builder
            .Property(_ => _.Id)
            .HasColumnName("Id");

        builder
            .Property(_ => _.Name)
            .HasColumnName("Name")
            .IsRequired();
    }
}
