using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class TasksController : Controller {
    private readonly ILogger<TasksController> _logger;
    TasksRepository tasksRepository;

    public TasksController(ILogger<TasksController> logger) {
        tasksRepository = new TasksRepository();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(UserIsAdmin()) return View(new GetTasksViewModel(tasksRepository.GetAll()));
        var loggedUserId = Convert.ToInt32(HttpContext.Session.GetString("Id"));
        return View(new GetTasksViewModel(tasksRepository.GetByUser(loggedUserId)));
    }

    [HttpGet]
    public IActionResult Add() => View(new AddTaskViewModel());

    [HttpPost]
    public IActionResult Add(AddTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var newTask = new Tasks() {
            Name = task.Name,
            Description = task.Description,
            State = TasksState.Ideas,
            Color = task.Color,
            BoardId = task.BoardId
        };
        tasksRepository.Add(newTask.BoardId, newTask);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) => View(new UpdateTaskViewModel(tasksRepository.GetById(id)));

    [HttpPost]
    public IActionResult Update(UpdateTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var targetTask = tasksRepository.GetAll().FirstOrDefault(t => t.Id == task.Id);
        var updatedTask = new Tasks() {
            Id = task.Id,
            BoardId = targetTask.BoardId,
            Name = task.Name,
            State = task.State,
            Description = task.Description,
            Color = task.Color,
            AssignedUserId = targetTask.AssignedUserId
        };
        tasksRepository.Update(task.Id, updatedTask);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        tasksRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool NotLogged() => string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")); 
    private bool UserIsAdmin() => HttpContext.Session.GetString("Role") == Enum.GetName(Role.Admin);
 
}
