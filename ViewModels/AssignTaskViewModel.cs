using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_InakiPoch.Models;


namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AssignTaskViewModel {
    public int TaskId { get; set; }
    public List<User> UsersAvailable { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int User { get; set; }

    public AssignTaskViewModel() {}

    public AssignTaskViewModel(int taskId, List<User> usersAvailable) {
        TaskId = taskId;
        UsersAvailable = usersAvailable;
    }
}