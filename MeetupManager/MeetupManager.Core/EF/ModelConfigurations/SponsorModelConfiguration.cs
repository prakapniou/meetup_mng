namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class SponsorModelConfiguration : IEntityTypeConfiguration<Sponsor>
{
    public void Configure(EntityTypeBuilder<Sponsor> builder)
    {
        builder
            .ToTable("sponsors");

        builder
            .Property(_ => _.Id)
            .HasColumnName("id");

        builder
            .Property(_ => _.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}
