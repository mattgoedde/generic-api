using GenericApi.Data.Models.v1;
using System.Text.Json;

namespace GenericApi.Data.Repositories
{
    public class FileRepository<T> : IRepository<T>
        where T : IRecord, new()
    {

        private readonly InMemoryRepository<T> _repo;
        private string _path;

        public FileRepository(string path)
        {
            _repo = new InMemoryRepository<T>();
            _path = path;
        }

        public Task<T> Create(T record) => _repo.Create(record);

        public Task<IEnumerable<T>> Create(IEnumerable<T> records) => _repo.Create(records);

        public Task<T> Delete(T record) => _repo.Delete(record);

        public Task<IEnumerable<T>> Delete(IEnumerable<T> records) => _repo.Delete(records);

        public async Task Save()
        {
            if (await _repo.Count() <= 0) return;

            var records = await _repo.Read();
            if (records is null) return;

            if (string.IsNullOrEmpty(_path)) _path = @".\";
            if (_path.Last() != '\\') _path = _path.Append('\\').ToString() ?? @".\";

            string fileName = $"{_path}FileRepository.json";

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            if (File.Exists(fileName)) File.Delete(fileName);

            foreach (T record in records)
                await File.AppendAllTextAsync(fileName, JsonSerializer.Serialize(record, new JsonSerializerOptions() { }));
        }

        public Task<IEnumerable<T>> Read() => _repo.Read();

        public Task<IEnumerable<T>> Read(Func<T, bool> expression) => _repo.Read(expression);

        public Task<T> Read(int id) => _repo.Read(id);

        public Task<T> Update(T record) => _repo.Update(record);

        public Task<IEnumerable<T>> Update(IEnumerable<T> records) => _repo.Update(records);

        public Task<int> Count() => _repo.Count();

        public Task<int> Count(Func<T, bool> expression) => _repo.Count(expression);
    }
}
