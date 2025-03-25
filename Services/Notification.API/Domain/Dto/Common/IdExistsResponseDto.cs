namespace Notification.API.Domain.Dto.Common
{
    public class IdExistsResponseDto
    {
        public bool DoesAllIdExists { get; set; }
        public List<long>? NotExistsList { get; set; }
    }
}
