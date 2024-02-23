using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Data.Contexts;
using System.Collections.Generic;

namespace Payment.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly PaymentDbContext Db;
        protected readonly DbSet<TEntity> DbSet;
    }
}
