namespace inventory_system_api.IRepository
{
    public interface IBaseRepository<T>
    {
       
        public Task<List<T>> Get();

        public Task<T> Get(int id);
        
        public int AddEdit(T entity);

        public Task<int> Delete(int id);

        public Task<List<T>> Navigate(int pageNo, int rowPerPage);
    }
}
