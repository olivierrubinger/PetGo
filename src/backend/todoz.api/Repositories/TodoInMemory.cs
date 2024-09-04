using todoz.api.Controllers;
using todoz.api.Models;

namespace todoz.api.Repositories;

public class TodoInMemory : ITodoRepository
{
    private List<Todo>? Todos { get; }
    private int nextId = 3;
    public TodoInMemory()
    {
        Todos = new List<Todo>
        {
            new Todo { Id = 1, Title = "Aprender C#", Description = "Estudar a estrutura básica do C#", IsComplete = false },
            new Todo { Id = 2, Title = "Construir uma aplicação", Description = "Utilizar o .net", IsComplete = false }
        };
    }

    public List<Todo> GetAll()
    {
        return Todos;
    }

    public Todo? Get(int id) => Todos.FirstOrDefault(t => t.Id == id);

    public void Add(Todo todo)
    {
        todo.Id = nextId++;
        Todos.Add(todo);
    }

    public void Delete(int id)
    {
        var todo = Get(id);
        if (todo is null)
            return;

        Todos.Remove(todo);
    }

    public void Update(Todo todo)
    {
        var index = Todos.FindIndex(t => t.Id == todo.Id);
        if (index == -1)
            return;

        Todos[index] = todo;
    }
}
