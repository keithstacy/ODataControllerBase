using System;
using System.Data.Entity;

namespace HCLib.ODataControllerBase
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
		private TContext _context;

        public UnitOfWork() { }

		public UnitOfWork(TContext context)
        {
            _context = context;
        }

		public DbContext DataContext { get; set; }

	    public void Save()
        {
            _context.SaveChanges();
        }

	    public GenericRepository<TEntity, TContext> GetRepository<TEntity>() where TEntity : ModelBase
	    {
		    return new GenericRepository<TEntity, TContext>(new TContext());
	    }


	    private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
