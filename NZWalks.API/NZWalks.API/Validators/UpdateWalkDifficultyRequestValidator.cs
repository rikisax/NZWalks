using FluentValidation;
using NZWalks.API.Models.DTO;
using System.Data;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>

    {
        public UpdateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
