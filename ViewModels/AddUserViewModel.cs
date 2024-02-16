using tl2_tp10_2023_InakiPoch.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AddUserViewModel {
    public string ErrorMessage { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public Role Role { get; set; }

    public AddUserViewModel() {}
}