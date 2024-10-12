using todoz.api.Models;

namespace todoz.api.Controllers
{
    public interface ITodosRepository
    {
        void Add(Todo todo);
        void Delete(int id);
        Todo? Get(int id);
        List<Todo> GetAll();
        void Update(Todo todo);
        void Dispose();
        void Save();
    }
}