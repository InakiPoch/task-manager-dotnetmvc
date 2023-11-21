using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class LoginController : Controller {
    private readonly ILogger<UserController> _logger;
    UserRepository userRepository;

    public LoginController(ILogger<UserController> logger) {
        userRepository = new UserRepository();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel user) {
        var loguedUser = userRepository.GetAll().FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
        if(loguedUser == null) return RedirectToAction("Index");
        LogUser(loguedUser);
        return RedirectToRoute(new { controller = "User", action = "Index" });

    }

    private void LogUser(User user) {
        HttpContext.Session.SetString("Id", user.Id.ToString());
        HttpContext.Session.SetString("Usuario", user.Username);
        HttpContext.Session.SetString("Password", user.Password);
        HttpContext.Session.SetString("Role", Enum.GetName(user.Role));
    } 

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
