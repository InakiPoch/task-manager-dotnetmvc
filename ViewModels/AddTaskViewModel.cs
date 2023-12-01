using tl2_tp10_2023_InakiPoch.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AddTaskViewModel {
    [Required(ErrorMessage = "Campo requerido")]
    public int BoardId { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Color { get; set; }

    public AddTaskViewModel() {}

}