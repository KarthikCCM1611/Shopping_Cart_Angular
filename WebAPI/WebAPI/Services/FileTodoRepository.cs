using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ITodoRepository
    {
        IEnumerable<TodoItem> GetAll();
        TodoItem? Get(Guid id);
        TodoItem Add(TodoItem item);
        bool Update(Guid id, TodoItem item);
        bool Delete(Guid id);
    }

    public class FileTodoRepository : ITodoRepository
    {
        private readonly string _path = "";
        private readonly object _lock = new();
        private List<TodoItem> _cache = new();

        public FileTodoRepository(IWebHostEnvironment webHostEnvironment)
        {
            _path = Path.Combine(webHostEnvironment.ContentRootPath, "data/todo", "todos.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            if (File.Exists(_path))
                _cache = JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(_path)) ?? new();
            else
                Persist();   // create empty file on first run
        }

        public IEnumerable<TodoItem> GetAll() => _cache;

        public TodoItem? Get(Guid id) => _cache.FirstOrDefault(x => x.Id == id);


        public TodoItem Add(TodoItem item)
        {
            lock (_lock)
            {
                _cache.Add(item);
                Persist();
                return item;
            }
        }

        public bool Update(Guid id, TodoItem item)
        {
            lock (_lock)
            {
                var idx = _cache.FindIndex(x => x.Id == id);
                if (idx == -1) return false;
                _cache[idx] = item with { Id = id };
                Persist();
                return true;
            }
        }

        public bool Delete(Guid id)
        {
            lock (_lock)
            {
                var removed = _cache.RemoveAll(x => x.Id == id) > 0;
                if (removed) Persist();
                return removed;
            }
        }

        private void Persist()
        {
            try
            {
                var json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_path, json);
            }
            catch (Exception ex)
            {
                // TODO: log ex (Serilog / ILogger) – but swallow to keep API alive
            }
        }
    }
}
