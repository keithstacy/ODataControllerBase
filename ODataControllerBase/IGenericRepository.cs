using System;

namespace HCLib.ODataControllerBase
{
	interface IGenericRepository<TEntity>
	 where TEntity : ModelBase
	{
		void Delete(object id);
		void Delete(TEntity entityToDelete);
		void Dispose();
		bool Exists(object id);
		System.Collections.Generic.IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
		System.Data.Entity.DbSet<TEntity> GetAll();
		TEntity GetById(object id);
		void Insert(TEntity entity);
		void Update(TEntity entityToUpdate);
	}
}
