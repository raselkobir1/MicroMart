using Auth.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Auth.API.DataAccess.Interfaces;

namespace Auth.API.DataAccess.DataContext
{
    public class CommonDbContext : DbContext, ICommonDbContext
    {
        public CommonDbContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public virtual DbSet<Domain.Entities.User> Users { get; set; }
        public virtual DbSet<VerificationCode> VerificationCode { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }

            //Shared Entities Exclude From Migrations
            //modelBuilder.Entity<Organization>().ToTable("Organizations", "public", t => t.ExcludeFromMigrations());

            base.OnModelCreating(modelBuilder);

        }
        private LambdaExpression CreateSoftDeleteFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var property = Expression.Property(parameter, "IsDeleted");
            var condition = Expression.Equal(property, Expression.Constant(false));

            return Expression.Lambda(condition, parameter);
        }
    }
}
