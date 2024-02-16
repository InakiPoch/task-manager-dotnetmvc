using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class LoginController : Controller {
    private readonly ILogger<UserController> _logger;
    private IUserRepository userRepository;
    private RoleCheck roleCheck;

    public LoginController(ILogger<UserController> logger, IUserRepository userRepository, RoleCheck roleCheck) {
        this.userRepository = userRepository;
        this.roleCheck = roleCheck;
        _logger = logger;
    }

    [HttpGet]
        public IActionResult Index(string errorMessage = null) {
            var model = new LoginViewModel() {
                ErrorMessage = errorMessage
            };
            return View(model);
        }

    [HttpPost]
    public IActionResult Login(LoginViewModel user) {
        try {
            var loggedUser = userRepository.FindAccount(user.Username, user.Password);
            LogUser(loggedUser);
            _logger.LogInformation("User " + loggedUser.Username + " logged successfully");
            return RedirectToRoute(new { controller = "MainPage", action = "Index" });
        } catch (Exception e) {
            user.ErrorMessage = "Nombre de Usuario o Contraseña incorrectos";
            _logger.LogError(e.ToString());
            _logger.LogWarning(
                "Invalid user loggin attempt - Username: " + user.Username + " / Password: " + user.Password
            );
        }
        return View("Index", user);
    }

    [HttpGet]
    public IActionResult Unlog(int loggedUserId) {
        try {
            var loggedUser = userRepository.GetById(loggedUserId);
            UnlogUser();
            _logger.LogInformation("User " + loggedUser.Username + " unlogged successfully");
            return RedirectToAction("Index");
        } catch (Exception e) {
            _logger.LogError(e.ToString());
            _logger.LogWarning("Couldn't unlog user");
            return RedirectToRoute(new { controller = "MainPage", action = "Index"});
        }
    }

    private void LogUser(User user) {
        HttpContext.Session.SetString("Id", user.Id.ToString());
        HttpContext.Session.SetString("User", user.Username);
        HttpContext.Session.SetString("Password", user.Password);
        HttpContext.Session.SetString("Role", Enum.GetName(user.Role));
    }

    private void UnlogUser() {
        HttpContext.Session.SetString("Id", string.Empty);
        HttpContext.Session.SetString("User", string.Empty);
        HttpContext.Session.SetString("Password", string.Empty);
        HttpContext.Session.SetString("Role", string.Empty);
    }  

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
