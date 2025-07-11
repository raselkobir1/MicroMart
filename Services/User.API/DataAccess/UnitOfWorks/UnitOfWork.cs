﻿using System.Security.Claims;
using User.API.DataAccess.DataContext;
using User.API.DataAccess.Implementations;
using User.API.DataAccess.Interfaces;
using User.API.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace User.API.DataAccess.UnitOfWorks
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
            var userId = _httpContextAccessor?.HttpContext?.Request.Headers["x-user-id"].ToString() ?? "1";
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();
            return Convert.ToInt64(userId);
        }
        public long GetLoggedInUserRoleId()
        {
            var roleId = _httpContextAccessor?.HttpContext?.Request.Headers["x-user-role"].ToString();
            if (string.IsNullOrWhiteSpace(roleId))
                throw new UnauthorizedAccessException();
            return Convert.ToInt64(roleId);
        }

        public string GetLoggedInUserName()
        {
            var userName = _httpContextAccessor?.HttpContext?.Request.Headers["x-user-name"].ToString();
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
