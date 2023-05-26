namespace MeetupManager.Infrastructure.Exceptions;

public sealed class ContentNotFoundException:Exception
{
    public ContentNotFoundException(string message):base(message) { }
}
