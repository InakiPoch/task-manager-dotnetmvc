using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class UserController : Controller {
    private readonly ILogger<UserController> _logger;
    private IUserRepository userRepository;
    private RoleCheck roleCheck;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, RoleCheck roleCheck) {
        this.userRepository = userRepository;
        this.roleCheck = roleCheck;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        return View(new GetUsersViewModel(userRepository.GetAll()));
    }

    [HttpGet]
    public IActionResult Add(string errorMessage = null) { 
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(!roleCheck.IsAdmin()) return RedirectToAction("Index");
        var model = new AddUserViewModel() {
            ErrorMessage = errorMessage
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Add(AddUserViewModel user) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var newUser = new User() {
                Username = user.Username,
                Password = user.Password,
                Role = user.Role
            };
            if(userRepository.UserExists(newUser)) {
                throw new Exception("Usuario ya existe");
            }
            userRepository.Add(newUser);
            return RedirectToAction("Index");
        } catch (Exception e) {
            user.ErrorMessage = "Usuario ya existente";
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Add", user);
    }

    [HttpGet]
    public IActionResult Update(int id, string errorMessage = null) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(!roleCheck.IsAdmin()) return RedirectToAction("Index");
        var model = new UpdateUserViewModel(userRepository.GetById(id)) {
            ErrorMessage = errorMessage
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(UpdateUserViewModel user) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var targetUser = userRepository.GetById(user.Id);
            var updatedUser = new User() {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Password = targetUser.Password
            };
            if(AlreadyExists(updatedUser, targetUser)) {
                throw new Exception("No se puede actualizar el nombre. Usuario ya existente");
            }
            userRepository.Update(user.Id, updatedUser);
            return RedirectToAction("Index");
        } catch (Exception e) {
            user.ErrorMessage = "No se puede actualizar el nombre. Usuario ya existente";
            _logger.LogError(e.ToString());
        }
        return View("Update", user);
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        if(!roleCheck.IsAdmin()) return RedirectToAction("Index");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            userRepository.Delete(id);  
            if(id == roleCheck.LoggedUserId()) {
                return RedirectToRoute(new {controller = "Login", action = "Index"});
            }
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    private bool AlreadyExists(User updatedUser, User user) => updatedUser.Username != user.Username && userRepository.UserExists(updatedUser);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
