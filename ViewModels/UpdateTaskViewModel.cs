using tl2_tp10_2023_InakiPoch.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateTaskViewModel {
    public string ErrorMessage { get; set; }
    public bool IsOwner { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public TasksState State { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public Color Color { get; set; }


    public UpdateTaskViewModel() {}

    //If task owner, can update every field, othwerwise it can only update its state
    public UpdateTaskViewModel(Tasks task, bool isOwner) {
        Id = task.Id;
        Name = task.Name;
        State = task.State;
        Description = task.Description;
        Color = task.Color;
        IsOwner = isOwner;
    }
}