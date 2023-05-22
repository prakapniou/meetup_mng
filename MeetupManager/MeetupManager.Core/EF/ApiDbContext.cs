namespace MeetupManager.Core.EF;

public sealed class ApiDbContext:DbContext
{
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<Meetup> Meetups { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
