using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class MainPageController : Controller {
    private readonly ILogger<UserController> _logger;
    private IUserRepository userRepository;
    private ITasksRepository tasksRepository;
    private IBoardRepository boardRepository;
    private RoleCheck roleCheck;

    public MainPageController(ILogger<UserController> logger, IUserRepository userRepository, ITasksRepository tasksRepository, 
                                IBoardRepository boardRepository, RoleCheck roleCheck) {
        this.userRepository = userRepository;
        this.tasksRepository = tasksRepository;
        this.boardRepository = boardRepository;
        this.roleCheck = roleCheck;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index" });
        var userId = roleCheck.LoggedUserId();
        int personalBoards = boardRepository.GetByUser(userId).Count;
        int personalTasks = tasksRepository.GetByUser(userId).Count;
        int assignedTasks = tasksRepository.GetByAssigned(userId).Count;
        int totalTasks = tasksRepository.GetAll().Count;
        int totalBoards = boardRepository.GetAll().Count;
        int totalUsers = userRepository.GetAll().Count;
        return View(new MainPageViewModel(personalBoards, personalTasks,assignedTasks, totalTasks, totalBoards, totalUsers));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
