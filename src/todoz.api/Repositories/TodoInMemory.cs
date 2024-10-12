using todoz.api.Controllers;
using todoz.api.Models;

namespace todoz.api.Repositories;

public class TodosInMemory : ITodosRepository
{
    private List<Todo>? Todos { get; }
    private int nextId = 3;
    public TodosInMemory()
    {
        Todos =
        [
            new Todo { Id = 1, Title = "Aprender C#", Description = "Estudar a estrutura básica do C#", IsComplete = false },
            new Todo { Id = 2, Title = "Construir uma aplicação", Description = "Utilizar o .net", IsComplete = false }
        ];
    }

#pragma warning disable CS8603 // Possible null reference return.
    public List<Todo> GetAll() => Todos;
#pragma warning restore CS8603 // Possible null reference return.

    public Todo? Get(int id) => Todos.FirstOrDefault(t => t.Id == id);

    public void Add(Todo todo)
    {
        todo.Id = nextId++;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Todos.Add(todo);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public void Delete(int id)
    {
        var todo = Get(id);
        if (todo is null)
            return;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _ = Todos.Remove(todo);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public void Update(Todo todo)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var index = Todos.FindIndex(t => t.Id == todo.Id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (index == -1)
            return;

        Todos[index] = todo;
    }

    public void Dispose() { }

    public void Save() { }
}
