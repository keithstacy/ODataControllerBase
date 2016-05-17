using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace HCLib.ODataControllerBase
{
    public class GenericRepository<TEntity, TContext> : IDisposable, IGenericRepository<TEntity>
		where TEntity : ModelBase 
		where TContext : DbContext
    {
        internal TContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(TContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual bool Exists(object id)
        {
            return DbSet.Find(id) != null;
        }

        public virtual DbSet<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Insert(TEntity entity)
        {
			try
			{
				DbSet.Add(entity);
				Context.SaveChanges();

			}
			catch (DbEntityValidationException e)
			{
				string msg = string.Empty;
				foreach (var item in e.EntityValidationErrors)
				{
					msg += item.Entry;
					msg += item.ValidationErrors.FirstOrDefault().ErrorMessage;
				}
				throw new Exception(msg);
			}
		}

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            DbSet.Remove(entityToDelete);
			Context.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
			Context.SaveChanges();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
	        DbSet.Find(entityToUpdate.Id);
        }

	    public TEntity Update(int key, TEntity updatedEntity)
	    {
		    Context.Entry(updatedEntity).State = EntityState.Modified;
		    Context.SaveChanges();
			return updatedEntity;
	    }

	    private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

	    public void Dispose()
	    {
		    Dispose(true);
		    GC.SuppressFinalize(this);
	    }
	}
}