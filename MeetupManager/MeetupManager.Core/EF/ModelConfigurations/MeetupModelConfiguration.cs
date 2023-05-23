namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class MeetupModelConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> builder)
    {
        builder
            .ToTable("meetups");

        builder
            .Property(_ => _.Id)
            .HasColumnName("id");

        builder
            .Property(_ => _.Name)
            .HasColumnName("name")
            .IsRequired();

        builder
            .Property(_ => _.Topic)
            .HasColumnName("topic")
            .IsRequired();

        builder
            .Property(_ => _.Description)
            .HasColumnName("description")
            .IsRequired();

        builder
            .Property(_ => _.Schedule)
            .HasColumnName("schedule")
            .IsRequired();

        builder
            .Property(_ => _.Address)
            .HasColumnName("address")
            .IsRequired();

        builder
            .Property(_ => _.Spending)
            .HasColumnName("spending")
            .IsRequired();
    }
}
