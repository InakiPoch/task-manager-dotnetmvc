using System.ComponentModel.DataAnnotations;


namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AssignTaskViewModel {
    public int TaskId { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Username { get; set; }

    public AssignTaskViewModel() {}

    public AssignTaskViewModel(int taskId) {
        TaskId = taskId;
    }
}