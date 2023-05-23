namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class SpeakerModelConfiguration : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder
            .ToTable("speakers");

        builder
            .Property(_ => _.Id)
            .HasColumnName("id");

        builder
            .Property(_ => _.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}
