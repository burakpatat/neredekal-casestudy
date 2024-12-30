using FluentValidation;
using HotelService.Application.Mediator.Commands;

namespace HotelService.Application.Validations
{
    public class HotelValidator : AbstractValidator<CreateHotelCommand>
    {
        public HotelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Hotel name is required.");
            RuleFor(x => x.Representatives).NotEmpty().WithMessage("Hotel representatives are required.");
            RuleFor(x => x.ContactInfos).NotEmpty().WithMessage("Hotel contact info is required.");
        }
    }
}
