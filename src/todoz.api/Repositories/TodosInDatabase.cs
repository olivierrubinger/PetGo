using Microsoft.EntityFrameworkCore;
using todoz.api.Controllers;
using todoz.api.Data;
using todoz.api.Models;

namespace todoz.api.Repositories;

public class TodosInDatabase(TodoContext context) : ITodosRepository
{
    private TodoContext _context = context;

    public List<Todo> GetAll() => [.. _context.Todos];

    public Todo? Get(int id) => _context.Todos.AsNoTracking().FirstOrDefault(t => t.Id == id);

    public void Add(Todo todo)
    {
        _context.Todos.Add(todo);
    }

    public void Delete(int id)
    {
        Todo? todo = Get(id);
        if (todo is null)
            return;

        _context.Todos.Remove(todo);
    }

    public void Update(Todo todo) => _context.Entry(todo).State = EntityState.Modified;

    public void Dispose() { _context.Dispose(); }

    public void Save()
    {
        _context.SaveChanges();
    }
}
