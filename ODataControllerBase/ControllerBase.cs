using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace HCLib.ODataControllerBase
{
	public class ControllerBase<TContext, TEntity> : ODataController 
		where TContext : DbContext, new() 
		where TEntity : ModelBase
	{
		protected readonly ODataValidationSettings ValidationSettings = new ODataValidationSettings();
		private static readonly TContext Context = new TContext();
		protected readonly IUnitOfWork<TContext> Data = new UnitOfWork<TContext>(Context);


		// GET: odata/Requests
		[EnableQuery]
		public virtual IEnumerable<TEntity> Get()
		{
			var repo = Data.GetRepository<TEntity>();
			IQueryable<TEntity> retval = repo.GetAll();
			return retval.ToList();
		}

		[EnableQuery]
		public virtual TEntity Get([FromODataUri] int key)
		{
			var repo = Data.GetRepository<TEntity>();
			var retval = SingleResult.Create(new List<TEntity>() { repo.GetById(key) }.AsQueryable());
			return retval.Queryable.SingleOrDefault();
		}

		// PUT: odata/Requests(5)
		public virtual IHttpActionResult Put([FromODataUri] int key, TEntity entity)
		{
			Validate(entity);
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var updatedEntity = Data.GetRepository<TEntity>().Update(key, entity);
			if (updatedEntity == null) return NotFound();
			return Updated(updatedEntity);
		}

		// POST: odata/Requests
		[HttpPost]
		public virtual IHttpActionResult Post(TEntity entity)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var repo = Data.GetRepository<TEntity>();
			repo.Insert(entity);
			return Created(entity);
		}

		// DELETE: odata/Requests(5)
		public virtual IHttpActionResult Delete([FromODataUri] int key)
		{
			var repo = Data.GetRepository<TEntity>();
			var entity = repo.GetById(key);
			if (entity == null) return NotFound();
			repo.Delete(entity);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}