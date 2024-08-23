using System.Diagnostics;

using DotnetTurbo.Web.Hubs;
using DotnetTurbo.Web.Models;
using DotnetTurbo.Web.Turbo;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotnetTurbo.Web.Controllers;

public class ControllerModel
{
    public TodoModel Todo { get; set; } = new();

    public IList<TodoModel> Todos { get; set; } = new List<TodoModel>();
}

public class HomeController(
    ILogger<HomeController> logger,
    IHubContext<TurboHub, ITurboHub> hubContext
    ) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IHubContext<TurboHub, ITurboHub> _hubContext = hubContext;

    /// <summary>
    /// This is a simple in-memory storage for demonstration purposes.
    /// </summary>
    private static readonly List<TodoModel> _todos = [];

    public IActionResult Index()
    {
        return View(new ControllerModel { Todos = _todos });
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(TodoModel model)
    {
        if (!ModelState.IsValid)
            return TurboStream.Update("new-todo-form", PartialView("_TodoForm", model));

        _todos.Add(model);
        this.ModelState.Clear();

        var updateResult = TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
        await _hubContext.Clients.All.TurboStream(
            await updateResult.RenderToString(ControllerContext)
        );

        return new TurboStreamResults(
            TurboStream.Update("todo-list", PartialView("_TodoList", _todos)),
            TurboStream.Update("new-todo-form", PartialView("_TodoForm", new TodoModel()))
        );
    }

    public async Task<IActionResult> ToggleTodo(string id)
    {
        foreach (var todo in _todos)
        {
            if (todo.ID == id)
            {
                todo.Done = !todo.Done;
                break;
            }
        }

        var updateResult = TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
        await _hubContext.Clients.All.TurboStream(
            await updateResult.RenderToString(ControllerContext)
        );

        return TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
    }

    public async Task<IActionResult> RemoveTodo(string id)
    {
        _todos.RemoveAll(todo => todo.ID == id);

        var updateResult = TurboStream.Update("todo-list", PartialView("_TodoList", _todos));
        await _hubContext.Clients.All.TurboStream(
            await updateResult.RenderToString(ControllerContext)
        );

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