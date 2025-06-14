using System.Linq.Expressions;

namespace DomainData.Repository
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public TModel GetById(int id, params Expression<Func<TModel, object>>[] includes);
        public List<TModel> GetAll(params Expression<Func<TModel, object>>[] includes);
        public IEnumerable<TModel> GetByFilter(Expression<Func<TModel, bool>> filter, params Func<IQueryable<TModel>, IQueryable<TModel>>[] includes);
        public void Attach(TModel model);

        public void Create(TModel model);

        public void Update(TModel model);

        public void Delete(int id);
        public TModel GetTrackedOrAttach(int id, params Expression<Func<TModel, object>>[] includes);
    }
}