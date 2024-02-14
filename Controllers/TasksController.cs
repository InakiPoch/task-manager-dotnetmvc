using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class TasksController : Controller {
    private readonly ILogger<TasksController> _logger;
    private ITasksRepository tasksRepository;
    private IBoardRepository boardRepository;
    private IUserRepository userRepository;
    private RoleCheck roleCheck;

    public TasksController(ILogger<TasksController> logger, ITasksRepository tasksRepository, IBoardRepository boardRepository, 
                            IUserRepository userRepository, RoleCheck roleCheck) {
        this.tasksRepository = tasksRepository;
        this.boardRepository = boardRepository;
        this.userRepository = userRepository;
        this.roleCheck = roleCheck;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(roleCheck.IsAdmin()) {
            return View(new GetTasksViewModel(
                tasksRepository.GetByUser(roleCheck.LoggedUserId()),
                tasksRepository.GetByAssigned(roleCheck.LoggedUserId()),
                tasksRepository.GetAll()
            ));
        }
        return View(new GetTasksViewModel(
            tasksRepository.GetByUser(roleCheck.LoggedUserId()), 
            tasksRepository.GetByAssigned(roleCheck.LoggedUserId()), 
            new List<Tasks>()    
        ));
    }

    [HttpGet]
    public IActionResult Add() => View(new AddTaskViewModel(boardRepository.GetByUser(roleCheck.LoggedUserId())));

    [HttpPost]
    public IActionResult Add(AddTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var newTask = new Tasks() {
                Name = task.Name,
                Description = task.Description,
                State = TasksState.Ideas, //By default
                Color = task.Color,
                BoardId = task.BoardId
            };
            tasksRepository.Add(newTask.BoardId, newTask);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) {
        var task = tasksRepository.GetById(id);
        var userBoards = boardRepository.GetByUser(roleCheck.LoggedUserId());
        foreach(Board board in userBoards) {
            if(board.Id == task.BoardId)
                return View(new UpdateTaskViewModel(task, true));
        }
        return View(new UpdateTaskViewModel(task, false));
    }

    [HttpPost]
    public IActionResult Update(UpdateTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var targetTask = tasksRepository.GetById(task.Id);
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
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AssignTask(int taskId) => View(new AssignTaskViewModel(taskId));        

    [HttpPost]
    public IActionResult AssignTask(AssignTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var user = userRepository.GetByUsername(task.Username);
            tasksRepository.AssignTask(user.Id, task.TaskId);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
        
    }

    [HttpGet]
    public IActionResult Delete(int id) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            tasksRepository.Delete(id);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    } 
}
