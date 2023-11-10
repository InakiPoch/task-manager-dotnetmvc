using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp09_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class UserController : Controller {
    private readonly ILogger<UserController> _logger;
    UserRepository userRepository;

    public UserController(ILogger<UserController> logger) {
        userRepository = new UserRepository();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View(userRepository.GetAll());


    [HttpGet]
    public IActionResult Add() => View(new User());

    [HttpPost]
    public IActionResult Add(User user) {
        userRepository.Add(user);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) => View(userRepository.GetById(id));

    [HttpPost]
    public IActionResult Update(User user) {
        userRepository.Update(user.Id, user);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        userRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
