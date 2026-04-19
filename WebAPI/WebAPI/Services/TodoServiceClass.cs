using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IToDo
    {
        List<TodoItemClass> LoadExisingToDo();
        void SaveToDo();
        List<TodoItemClass> GetAllToDoLists();
        TodoItemClass? GetToDo(Guid id);
        bool AddNewDo(TodoItemClass item);
        //void AddNewDo(TodoItemClass item, out string errorMessage);
        bool UpdateToDo(Guid id, TodoItemClass item);
        bool DeleteToDo(Guid id);
    }

    public class TodoServiceClass: IToDo
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        private List<TodoItemClass> _todoItems;

        public TodoServiceClass(IWebHostEnvironment env)
        {
            var dir = Path.Combine(env.ContentRootPath, "data/todo");
            Directory.CreateDirectory(dir);
            _filePath = Path.Combine(dir, "todosClass.json");
            if (!File.Exists(_filePath)) SaveToDo();
            _todoItems = LoadExisingToDo();
        }

        //private List<TodoItemClass> Load()
        public List<TodoItemClass> LoadExisingToDo()
        {
            try
            {
                if (!File.Exists(_filePath)) return new List<TodoItemClass>();
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TodoItemClass>>(json) ?? new List<TodoItemClass>();
            }
            catch
            {
                return new List<TodoItemClass>();
            }
        }

        //private void Save()
        public void SaveToDo()
        {
            var json = JsonSerializer.Serialize(_todoItems, new JsonSerializerOptions{ WriteIndented = true});
            File.WriteAllText(_filePath, json);
        }

        public List<TodoItemClass> GetAllToDoLists() => _todoItems;

        public TodoItemClass? GetToDo(Guid id) => _todoItems.FirstOrDefault(i => i.Id == id);

        //public bool AddNewDo(TodoItemClass item, out string errorMessage)
        public bool AddNewDo(TodoItemClass item)
        {
            lock (_lock)
            {
                //string errorMessage = string.Empty;
                if (_todoItems.Any(x => x.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    //errorMessage = "";
                    return false;
                }
                _todoItems.Add(item);
                SaveToDo();                
                return true;
            }
        }

        public bool UpdateToDo(Guid id, TodoItemClass item)
        {
            lock (_lock)
            {
                var idx = _todoItems.FindIndex(x => x.Id == id);
                if (idx == -1) return false;
                _todoItems[idx] = new TodoItemClass
                {
                    Id = id,
                    Title = item.Title,
                    IsDone = item.IsDone
                };
                SaveToDo();
                return true;
            }
        }

        public bool DeleteToDo(Guid id)
        {
            lock (_lock)
            {
                var item = _todoItems.FirstOrDefault(x => x.Id == id);
                if (item == null) return false;
                _todoItems.Remove(item);
                SaveToDo();
                return true;
            }
        }
    }
}
