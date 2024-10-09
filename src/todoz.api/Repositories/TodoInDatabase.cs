using Microsoft.EntityFrameworkCore;
using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Models;

namespace todoz.api.Repositories;

public class TodoInDatabase : ITodoRepository
{
    private TodoContext _context;

    public TodoInDatabase(TodoContext context)
    {
        _context = context;
    }

    public List<Todo> GetAll()
    {
        return _context.Todos.ToList();
    }

    public Todo? Get(int id) => _context.Todos.AsNoTracking().FirstOrDefault(t => t.Id == id);

    public void Add(Todo todo)
    {
        _context.Todos.Add(todo);
    }

    public void Delete(int id)
    {
        var todo = Get(id);
        if (todo is null)
            return;

        _context.Todos.Remove(todo);
    }

    public void Update(Todo todo)
    {
        _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }

    public void Dispose() { _context.Dispose(); }

    public void Save()
    {
        _context.SaveChanges();
    }
}
