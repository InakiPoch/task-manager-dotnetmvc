using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class AddTaskViewModel {
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }

    public AddTaskViewModel() {}

}