namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class SponsorModelConfiguration : IEntityTypeConfiguration<Sponsor>
{
    public void Configure(EntityTypeBuilder<Sponsor> builder)
    {       
        builder
            .Property(_ => _.Name)
            .IsRequired();
    }
}
