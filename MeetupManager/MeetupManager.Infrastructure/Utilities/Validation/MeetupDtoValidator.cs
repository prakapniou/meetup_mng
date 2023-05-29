namespace MeetupManager.Infrastructure.Utilities.Validation;

public sealed class MeetupDtoValidator:AbstractValidator<MeetupDto>
{
    public MeetupDtoValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().NotNull();
        RuleFor(_ => _.Topic).NotEmpty().NotNull();
        RuleFor(_ => _.Description).NotEmpty().NotNull();
        RuleFor(_ => _.Schedule).NotEmpty().NotNull();
        RuleFor(_ => _.Address).NotEmpty().NotNull();
        RuleFor(_ => _.Spending).NotEmpty().NotNull();
    }
}
