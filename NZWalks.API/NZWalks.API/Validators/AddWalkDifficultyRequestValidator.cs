using FluentValidation;
using NZWalks.API.Models.DTO;
using System.Data;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.AddWalkDifficultyRequest>

    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
