using tl2_tp10_2023_InakiPoch.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateBoardViewModel {
    public string ErrorMessage { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public string Description { get; set; }

    public UpdateBoardViewModel() {}

    public UpdateBoardViewModel(Board board) {
        Id = board.Id;
        Name = board.Name;
        Description = board.Description;
    }
}