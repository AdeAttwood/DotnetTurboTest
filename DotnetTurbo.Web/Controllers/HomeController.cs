using System.Diagnostics;
using DotnetTurbo.Web.Models;
using DotnetTurbo.Web.Turbo;
using Microsoft.AspNetCore.Mvc;

namespace DotnetTurbo.Web.Controllers;

public class ControllerModel
{
    public TodoModel Todo { get; set; } = new();

    public IList<TodoModel> Todos { get; set; } = new List<TodoModel>();
}

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// This is a simple in-memory storage for demonstration purposes.
    /// </summary>
    private static List<TodoModel> _todos = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new ControllerModel { Todos = _todos });
    }

    [HttpPost]
    public IActionResult AddTodo(TodoModel model)
    {
        if (!ModelState.IsValid)
            return TurboStream.Update("new-todo-form", PartialView("_TodoForm", model));

        _todos.Add(model);
        this.ModelState.Clear();

        return new TurboStreamResults(
            TurboStream.Update("todo-list", PartialView("_TodoList", _todos)),
            TurboStream.Update("new-todo-form", PartialView("_TodoForm", new TodoModel()))
        );
    }

    public IActionResult ToggleTodo(string id)
    {
        foreach (var todo in _todos)
        {
            if (todo.ID == id)
            {
                todo.Done = !todo.Done;
                break;
            }
        }

        return TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
    }

    public IActionResult RemoveTodo(string id)
    {
        _todos.RemoveAll(todo => todo.ID == id);
        return TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
