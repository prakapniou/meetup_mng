namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class MeetupModelConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> builder)
    {
        builder
            .HasMany(_ => _.Speakers)
            .WithMany();

        builder
            .HasMany(_ => _.Sponsors)
            .WithMany();
                
        builder
            .Property(_ => _.Name)
            .IsRequired();

        builder
            .Property(_ => _.Topic)
            .IsRequired();

        builder
            .Property(_ => _.Description)
            .IsRequired();

        builder
            .Property(_ => _.Schedule)
            .IsRequired();

        builder
            .Property(_ => _.Address)
            .IsRequired();

        builder
            .Property(_ => _.Spending)
            .IsRequired();
    }
}
