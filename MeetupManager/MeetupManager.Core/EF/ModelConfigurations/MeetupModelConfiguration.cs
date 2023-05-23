namespace MeetupManager.Core.EF.ModelConfigurations;

public sealed class MeetupModelConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> builder)
    {      
        builder
            .ToTable("meetups")
            .HasKey(_ => _.Id)
            .HasName("Id");

        builder
            .Property(_ => _.Id)
            .HasColumnName("Id");

        builder
            .Property(_ => _.Name)
            .HasColumnName("Name")
            .IsRequired();

        builder
            .Property(_ => _.Topic)
            .HasColumnName("Topic")
            .IsRequired();

        builder
            .Property(_ => _.Description)
            .HasColumnName("Description")
            .IsRequired();

        builder
            .Property(_ => _.Schedule)
            .HasColumnName("Schedule")
            .IsRequired();

        builder
            .Property(_ => _.Address)
            .HasColumnName("Address")
            .IsRequired();

        builder
            .Property(_ => _.Spending)
            .HasColumnName("Spending")
            .IsRequired();
    }
}
