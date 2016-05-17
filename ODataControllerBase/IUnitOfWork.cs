using System;
using System.Data.Entity;

namespace HCLib.ODataControllerBase
{
	public interface IUnitOfWork<TContext> : IDisposable 
		where TContext : DbContext
	{	
		
		DbContext DataContext { get; set; }
		void Save();

		GenericRepository<TEntity, TContext> GetRepository<TEntity>() where TEntity : ModelBase;
	}
}