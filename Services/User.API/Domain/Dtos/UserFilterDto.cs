using FluentValidation;
using User.API.Domain.Dto.Common.PaginatedResult;

namespace User.API.Domain.Dtos
{
    public class UserFilterDto: BaseFilterDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }  
    }
    public class UserFilterDtoValidator : AbstractValidator<UserFilterDto>
    {
        public UserFilterDtoValidator()
        {
            Include(new BaseFilterDtoValidator());
        }
    }
}
