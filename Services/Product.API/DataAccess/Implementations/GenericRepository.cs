using Product.API.DataAccess.DataContext;
using Product.API.DataAccess.Interfaces;
using Product.API.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Product.API.DataAccess.Implementations
{
    public abstract class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly ReadDbContext _readDbContext;

        protected GenericRepository(ApplicationDbContext context, ReadDbContext readDbContext)
        {
            _dbContext = context;
            _readDbContext = readDbContext;
        }
        public void Add(Entity entity)
        {
            _dbContext.Set<Entity>().AddAsync(entity);
        }
        public void AddRange(List<Entity> entities)
        {
            _dbContext.Set<Entity>().AddRangeAsync(entities);
        }

        public void Update(Entity entity)
        {
            _dbContext.Set<Entity>().Update(entity);
        }
        public void UpdateRange(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();

        }

        public async Task<bool> Any(Expression<Func<Entity, bool>> predicate)
        {
            return await _dbContext.Set<Entity>().AnyAsync(predicate);
        }

        public async Task<TResult> Max<TResult>(Expression<Func<Entity, TResult>> where)
        {
            return await _dbContext.Set<Entity>().MaxAsync(where);
        }

        public async Task<IdExistsResponseDto> DoesIdsExistInDatabase(List<long> ids, Expression<Func<Entity, bool>>? filter = null)
        {
            var existingIdsQuery = _dbContext.Set<Entity>();
            if (filter != null)
                existingIdsQuery.Where(filter);

            var existingIds = await existingIdsQuery
                    .Select(GetIdExpression<Entity>())
                    .ToListAsync();

            var missingIds = ids.Except(existingIds).ToList();

            var response = new IdExistsResponseDto()
            {
                DoesAllIdExists = missingIds.Count == 0,
                NotExistsList = new List<long>(missingIds)
            };

            return response;
        }

        public async Task<Entity> GetById(long id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }

        public async Task<Entity> GetWhere(Expression<Func<Entity, bool>> where, params Expression<Func<Entity, object>>[] include)
        {
            var query = _dbContext.Set<Entity>().AsQueryable();
            if (include != null && include.Length != 0)
            {
                foreach (var item in include)
                {
                    query = query.Include(item);
                }
            }
            return await query.Where(where).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Entity>> GetAll()
        {
            return await _dbContext.Set<Entity>().ToListAsync();
        }

        public async Task<List<Entity>> GetAll(Expression<Func<Entity, bool>> predicate)
        {
            return await _dbContext.Set<Entity>().Where(predicate).ToListAsync();
        }


        #region Private Methods
        private static Expression<Func<T, long>> GetIdExpression<T>()
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, "Id");
            var lambda = Expression.Lambda<Func<T, long>>(property, parameter);

            return lambda;
        }

        Task<IdExistsResponseDto> IGenericRepository<Entity>.DoesIdsExistInDatabase(List<long> ids, Expression<Func<Entity, bool>>? filter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
