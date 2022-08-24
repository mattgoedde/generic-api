using GenericApi.Data.Models.v1;

namespace GenericApi.Data.Repositories
{
    public interface IRepository<T>
        where T : IRecord, new()
    {
        public Task<int> Count();
        public Task<int> Count(Func<T, bool> expression);
        public Task<T?> Create(T record);
        public Task<IEnumerable<T>?> Create(IEnumerable<T> records);
        public Task<IEnumerable<T>?> Read();
        public Task<IEnumerable<T>?> Read(Func<T, bool> expression);
        public Task<T?> Read(int id);
        public Task<T?> Update(T record);
        public Task<IEnumerable<T>?> Update(IEnumerable<T> records);
        public Task<T?> Delete(T record);
        public Task<IEnumerable<T>?> Delete(IEnumerable<T> records);
    }
}