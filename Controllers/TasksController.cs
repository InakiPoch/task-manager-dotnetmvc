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
    public IActionResult Add(string errorMessage = null) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        var model = new AddTaskViewModel(boardRepository.GetByUser(roleCheck.LoggedUserId())) {
            ErrorMessage = errorMessage
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Add(AddTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        task.BoardsAvailable = boardRepository.GetByUser(roleCheck.LoggedUserId()); //Othwerwise BoardsAvailable is null
        try {
            var newTask = new Tasks() {
                Name = task.Name,
                Description = task.Description,
                State = TasksState.Ideas, //By default
                Color = task.Color,
                BoardId = task.BoardId
            };
            if(tasksRepository.TaskExists(newTask)) {
                throw new Exception("Tarea ya existente");
            }
            tasksRepository.Add(newTask.BoardId, newTask);
            return RedirectToAction("Index");
        } catch (Exception e) {
            task.ErrorMessage = "Tarea ya existente";
            _logger.LogError(e.ToString());
        }
        return View("Add", task);
    }

    [HttpGet]
    public IActionResult Update(int id, string errorMessage = null) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        var task = tasksRepository.GetById(id);
        var userBoards = boardRepository.GetByUser(roleCheck.LoggedUserId());
        foreach(Board board in userBoards) {
            if(board.Id == task.BoardId)
                return View(new UpdateTaskViewModel(task, true) { ErrorMessage = errorMessage });
        }
        return View(new UpdateTaskViewModel(task, false) { ErrorMessage = errorMessage });
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
            if(AlreadyExists(updatedTask, targetTask)) {
                throw new Exception("No se puede actualizar. Tarea ya existente");
            }
            tasksRepository.Update(task.Id, updatedTask);
            return RedirectToAction("Index");
        } catch (Exception e) {
            task.ErrorMessage = "No se puede actualizar. Tarea ya existente";
            _logger.LogError(e.ToString());
        }
        return View("Update", task);
    }

    [HttpGet]
    public IActionResult AssignTask(int taskId) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        return View(new AssignTaskViewModel(taskId, userRepository.GetAll()));        
    }

    [HttpPost]
    public IActionResult AssignTask(AssignTaskViewModel task) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var user = userRepository.GetById(task.User);
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

    private bool AlreadyExists(Tasks updatedTask, Tasks task) => updatedTask.Name != task.Name && tasksRepository.TaskExists(updatedTask);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    } 
}
