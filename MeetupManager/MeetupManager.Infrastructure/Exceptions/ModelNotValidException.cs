namespace MeetupManager.Infrastructure.Exceptions;

public sealed class ModelNotValidException:Exception
{
    public ModelNotValidException(string message):base(message) { }
}
