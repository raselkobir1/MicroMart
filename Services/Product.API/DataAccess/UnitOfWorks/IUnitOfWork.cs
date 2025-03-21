
using Product.API.DataAccess.Interfaces;

namespace Product.API.DataAccess.UnitOfWorks
{

    public interface IUnitOfWork : IDisposable
    {
        int Save();
        Task<int> SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        long GetLoggedInUserId();
        long GetLoggedInUserRoleId();
        string GetLoggedInUserName();
        (bool, string) HasDependency(string schemaName, string table, string id);
        IProductRepository Products { get; } 
        //IInventoryHistoryRepository InventoryHistory { get; }  
        
    }
}
