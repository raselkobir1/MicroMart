namespace User.API.DataAccess.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
