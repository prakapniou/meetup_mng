namespace MeetupManager.Infrastructure.Utilities.Validation;

public sealed class SpeakerDtoValidator:AbstractValidator<SpeakerDto>
{
    public SpeakerDtoValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
    }
}
