namespace User.API.Domain.Dtos
{
    public class IdExistsResponseDto
    {
        public bool DoesAllIdExists { get; set; }
        public List<long>? NotExistsList { get; set; }
    }
}
