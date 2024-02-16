using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class LoginViewModel {
    public int Id { get; set; }
    public string ErrorMessage { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [PasswordPropertyText]
    public string Password { get; set; }

    public LoginViewModel() {}
}