using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class UserController : Controller {
    private readonly ILogger<UserController> _logger;
    UserRepository userRepository;

    public UserController(ILogger<UserController> logger) {
        userRepository = new UserRepository();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(!Logged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        return View(new GetUsersViewModel(userRepository.GetAll()));
    }

    [HttpGet]
    public IActionResult Add() { 
        if(!UserIsAdmin()) return RedirectToAction("Index");
        return View(new AddUserViewModel());
    }

    [HttpPost]
    public IActionResult Add(AddUserViewModel user) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var newUser = new User() {
            Username = user.Username,
            Password = user.Password,
            Role = user.Role
        };
        userRepository.Add(newUser);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) {
        if(!UserIsAdmin()) return RedirectToAction("Index");
        return View(new UpdateUserViewModel(userRepository.GetById(id)));
    }

    [HttpPost]
    public IActionResult Update(UpdateUserViewModel user) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var targetUser = userRepository.GetAll().FirstOrDefault(u => u.Id == user.Id);
        userRepository.Update(targetUser.Id, targetUser);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        if(!UserIsAdmin()) return RedirectToAction("Index");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        userRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool UserIsAdmin() => HttpContext.Session.GetString("Role") == Enum.GetName(Role.Admin);
    private bool Logged() => HttpContext.Session != null; 
}
