namespace MeetupManager.Web.Models;

/// <summary>
/// 
/// </summary>
public sealed class ErrorDetails
{
    /// <summary>
    /// 
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string StackTrace { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() => JsonConvert.SerializeObject(this);
}
