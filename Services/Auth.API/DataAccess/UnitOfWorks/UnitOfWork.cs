using System.Security.Claims;
using Auth.API.DataAccess.DataContext;
using Auth.API.DataAccess.Implementations;
using Auth.API.DataAccess.Interfaces;
using Auth.API.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace Auth.API.DataAccess.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _dbContext;
        public readonly ReadDbContext _readDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public long GetLoggedInUserId()
        {
            var userId = "1"; //_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();
            return Convert.ToInt64(userId);
        }
        public long GetLoggedInUserRoleId()
        {
            var roleId = _httpContextAccessor?.HttpContext?.User.FindFirstValue("RoleId");
            if (string.IsNullOrWhiteSpace(roleId))
                throw new UnauthorizedAccessException();
            return Convert.ToInt64(roleId);
        }

        public string GetLoggedInUserName()
        {
            var userName = _httpContextAccessor?.HttpContext?.User.FindFirstValue("UserName");
            if (!string.IsNullOrWhiteSpace(userName))
                return userName;

            return "Default";
        }

        public (bool, string) HasDependency(string schemaName, string table, string id)
        {
            var msg = _dbContext.Database
                        .SqlQueryRaw<HasDependencyDto>(
                            "SELECT * FROM public.\"admin_CheckDependency\"(@schemaname, @tablename, @id)",
                            new NpgsqlParameter("@schemaname", schemaName),
                            new NpgsqlParameter("@tablename", table),
                            new NpgsqlParameter("@id", id))
                        .FirstOrDefault();

            return (msg?.hasdependency ?? false, msg?.dependencemessage ?? "");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public UnitOfWork(ApplicationDbContext dbContext, ReadDbContext readDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _readDbContext = readDbContext;
            _httpContextAccessor = httpContextAccessor;

            Users = new UserRepository(dbContext, readDbContext);
            //InventoryHistory = new InventoryHistoryRepository(dbContext, readDbContext);
        }

        public IUserRepository Users { get; private set; } 

        //public IInventoryHistoryRepository InventoryHistory { get; private set; }
    }
}
