namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class SpeakerModelConfiguration : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder
            .Property(_ => _.Name)
            .IsRequired();
    }
}
