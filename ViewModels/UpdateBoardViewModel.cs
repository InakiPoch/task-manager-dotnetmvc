using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateBoardViewModel {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public UpdateBoardViewModel(Board board) {
        Id = board.Id;
        Name = board.Name;
        Description = board.Description;
    }
}