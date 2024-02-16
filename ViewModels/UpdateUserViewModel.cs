using tl2_tp10_2023_InakiPoch.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateUserViewModel {
    public string ErrorMessage { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public Role Role { get; set; }

    public UpdateUserViewModel() {}

    public UpdateUserViewModel(User user) {
        Id = user.Id;
        Username = user.Username;
        Role = user.Role;
    }
}