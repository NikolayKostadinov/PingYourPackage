namespace PingYourPackage.Domain.Entities.Core
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using PingYourPackage.Domain.Entities.Extentions;

    public class EntityRepository<T> : IEntityRepository<T>
    where T : class, IEntity, new()
    {
        readonly DbContext entitiesContext;

        public EntityRepository(DbContext entitiesContext)
        {
            if (entitiesContext == null)
            {
                throw new ArgumentNullException("entitiesContext");
            }

            this.entitiesContext = entitiesContext;
        }

        public virtual IQueryable<T> GetAll()
        {
            return entitiesContext.Set<T>();
        }

        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = entitiesContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public T GetSingle(Guid key)
        {
            return GetAll().FirstOrDefault(x => x.Key == key);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return entitiesContext.Set<T>().Where(predicate);
        }

        public virtual PaginatedList<T> Paginate<TKey>( 
            int pageIndex, int pageSize, 
            Expression<Func<T, TKey>> keySelector)
        {
            return Paginate(pageIndex, pageSize, keySelector, null);
        }

        public virtual PaginatedList<T> Paginate<TKey>(
            int pageIndex, int pageSize,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AllIncluding(includeProperties).OrderBy(keySelector);
            query = (predicate == null) ? query : query.Where(predicate);
            return query.ToPaginatedList(pageIndex, pageSize);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = this.entitiesContext.Entry<T>(entity);
            this.entitiesContext.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = this.entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = this.entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void DeleteGraph(T entity)
        {
            DbSet<T> dbSet = this.entitiesContext.Set<T>();
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            this.entitiesContext.SaveChanges();
        } 
    }
}
