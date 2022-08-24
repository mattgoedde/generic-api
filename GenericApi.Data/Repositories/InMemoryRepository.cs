using GenericApi.Data.Models.v1;

namespace GenericApi.Data.Repositories
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : IRecord, new()
    {
        private readonly IList<T> _data;

        public InMemoryRepository()
        {
            _data = new List<T>();
        }

        private int NextId
        {
            get
            {
                if (_data is null || _data.Count == 0) return 0;
                return _data.Max(x => x.Id) + 1;
            }
        }

        public async Task<int> Count() => _data.Count;

        public async Task<int> Count(Func<T, bool> expression) => _data.Where(expression).Count();

        public async Task<T?> Create(T record)
        {
            record.Id = NextId;
            _data.Add(record);

            return record;
        }

        public async Task<IEnumerable<T>?> Create(IEnumerable<T> records)
        {
            var result = new List<T>();

            foreach (T record in records)
            {
                record.Id = NextId;
                _data.Add(record);
                result.Add(record);
            }

            return result.AsEnumerable();
        }

        public async Task<T?> Delete(T record)
        {
            _data.Remove(_data.Single(r => r.Id == record.Id));
            return record;
        }

        public async Task<IEnumerable<T>?> Delete(IEnumerable<T> records)
        {
            var result = new List<T>();

            foreach (T record in records)
            {
                _data.Remove(_data.Single(r => r.Id == record.Id));
                result.Add(record);
            }

            return result.AsEnumerable();
        }

        public async Task<IEnumerable<T>?> Read()
        {
            return _data.AsEnumerable();
        }

        public async Task<IEnumerable<T>?> Read(Func<T, bool> expression)
        {
            return _data.Where(expression);
        }

        public async Task<T?> Read(int id)
        {
            return _data.SingleOrDefault(r => r.Id == id);
        }

        public async Task<T?> Update(T record)
        {
            var existingRecord = _data.SingleOrDefault(r => r.Id == record.Id);
            if (existingRecord is null) return default;
            if (_data.Remove(existingRecord))
            {
                _data.Add(record);
                return record;
            }
            else return default;
        }

        public async Task<IEnumerable<T>?> Update(IEnumerable<T> records)
        {
            var result = new List<T>();

            foreach (T record in records)
            {
                var updatedRecord = await Update(record);
                if (updatedRecord is null) continue;
                result.Add(updatedRecord);
            }

            return result.AsEnumerable();
        }
    }
}
