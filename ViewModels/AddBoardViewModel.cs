using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AddBoardViewModel {
    public string ErrorMessage { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int OwnerId { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Description { get; set; }

    public AddBoardViewModel() {}

    public AddBoardViewModel(int userId) { //Passes board owner's id
        OwnerId = userId;
    }
}