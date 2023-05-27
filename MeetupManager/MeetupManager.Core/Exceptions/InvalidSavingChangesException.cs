namespace MeetupManager.Core.Exceptions;

public sealed class InvalidSavingChangesException:Exception
{
    public InvalidSavingChangesException(string message) : base(message) { }
}
