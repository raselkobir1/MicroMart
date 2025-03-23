using FluentValidation;
using Auth.API.Domain.Dto.Common.PaginatedResult;

namespace Auth.API.Domain.Dtos
{
    public class UserFilterDto: BaseFilterDto
    {
        public string? UserName { get; set; } 
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
