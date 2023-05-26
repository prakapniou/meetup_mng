namespace MeetupManager.Infrastructure.Utilities.Validation;

public sealed class SponsorDtoValidator:AbstractValidator<SponsorDto>
{
    public SponsorDtoValidator()
    {
        RuleFor(_=>_.Name).NotEmpty().NotNull();
    }
}
